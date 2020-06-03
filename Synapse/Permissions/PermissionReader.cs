using System.Collections.Generic;
using System.IO;
using System.Linq;
using Synapse.Api;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

// ReSharper disable All

namespace Synapse.Permissions
{
    internal static class PermissionReader
    {
        // Variables
        private static Yml _permissionsConfig;

        private static readonly string PermissionPath =
            Path.Combine(PluginManager.ServerConfigDirectory, "permissions.yml");

        // Methods
        internal static void Init()
        {
            if (!File.Exists(PermissionPath))
                File.Create(PermissionPath).Close();

            ReloadPermission();
        }

        internal static void ReloadPermission()
        {
            var yml = File.ReadAllText(PermissionPath);

            var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            _permissionsConfig = deserializer.Deserialize<Yml>(yml);

            foreach (var key in _permissionsConfig.Groups.Keys)
            {
                _permissionsConfig.Groups.TryGetValue(key, out var group);

                if (group == null) continue;
                foreach (var permission in group.Inheritance)
                {
                    _permissionsConfig.Groups.TryGetValue(permission, out var parentGroup);

                    if (parentGroup == null) continue;
                    foreach (var parentPermissions in parentGroup.Permissions.Where(parentPermissions =>
                        !group.Permissions.Contains(parentPermissions)))
                        group.Permissions.Add(parentPermissions);
                }
            }
        }

        private static Group GetDefaultGroup()
        {
            return (from gr in _permissionsConfig.Groups where gr.Value.Default select gr.Value).FirstOrDefault();
        }

        private static Group GetNwGroup()
        {
            return (from gr in _permissionsConfig.Groups where gr.Value.Northwood select gr.Value).FirstOrDefault();
        }

        internal static bool CheckPermission(ReferenceHub player, string permission)
        {
            if (player == null)
            {
                Log.Error("The player has not been found, therefor no permission check could be done!");
                return false;
            }


            if (string.IsNullOrEmpty(permission))
            {
                Log.Error("Permission checked was null.");
                return false;
            }

            var userGroup = ServerStatic.GetPermissionsHandler().GetUserGroup(player.GetUserId());

            Group group = null;
            if (userGroup != null)
            {
                var groupName = ServerStatic.GetPermissionsHandler()._groups
                    .FirstOrDefault(g => g.Value == player.serverRoles.Group).Key;
                if (_permissionsConfig == null)
                {
                    Log.Error("Permission config is null.");
                    return false;
                }

                if (!_permissionsConfig.Groups.Any())
                {
                    Log.Error("No permission group.");
                    return false;
                }

                if (!_permissionsConfig.Groups.TryGetValue(groupName, out group))
                {
                    Log.Error("Could not get permission value.");
                    return false;
                }
            }
            else
            {
                if (player.serverRoles.Staff || player.GetUserId().EndsWith("@northwood")) group = GetNwGroup();
                else group = GetDefaultGroup();
            }

            if (group != null)
            {
                if (permission.Contains("."))
                    if (group.Permissions.Any(s => s == ".*"))
                        return true;
                if (group.Permissions.Contains(permission.Split('.')[0] + ".*")) return true;

                if (group.Permissions.Contains(permission) || group.Permissions.Contains("*")) return true;
            }
            else
            {
                return false;
            }

            return false;
        }
    }

    internal class Yml
    {
        public Dictionary<string, Group> Groups { get; set; } = new Dictionary<string, Group>();
    }

    internal class Group
    {
        [YamlMember(Alias = "default")] public bool Default { get; set; } = false;

        [YamlMember(Alias = "northwood")] public bool Northwood { get; set; } = false;

        public List<string> Inheritance { get; set; } = new List<string>();
        public List<string> Permissions { get; set; } = new List<string>();
    }
}