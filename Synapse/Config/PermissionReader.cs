using System.Collections.Generic;
using System.IO;
using System.Linq;
using Synapse.Api;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

// ReSharper disable All

namespace Synapse.Config
{
    public static class PermissionReader
    {
        // Variables
        private static Yml _permissionsConfig;

        // Methods
        internal static void Init()
        {
            if (!File.Exists(Files.PermissionFile))
                File.WriteAllText(Files.PermissionFile, "groups:\n    user:\n        default: true\n        permissions:\n        - plugin.permission\n    northwood:\n        northwood: true\n        permissions:\n        - plugin.permission\n    owner:\n        permissions:\n        - .*");

            ReloadPermission();
        }

        internal static void ReloadPermission()
        {
            var yml = File.ReadAllText(Files.PermissionFile);

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

        internal static bool CheckPermission(Player player, string permission)
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

            var userGroup = ServerStatic.GetPermissionsHandler().GetUserGroup(player.UserId);

            Group group = null;
            if (userGroup != null)
            {
                var groupName = ServerStatic.GetPermissionsHandler()._groups
                    .FirstOrDefault(g => g.Value == player.Rank).Key;
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
                    Log.Info($"TheServerGroup: {groupName} has no Permission Group!");
                    if (player.Hub.serverRoles.Staff || player.UserId.EndsWith("@northwood")) group = GetNwGroup();
                    else group = GetDefaultGroup();
                }
            }
            else
            {
                if (player.Hub.serverRoles.Staff || player.UserId.EndsWith("@northwood")) group = GetNwGroup();
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

        internal static bool CheckGroupPermission(string groupname,string permission)
        {
            if (string.IsNullOrEmpty(permission))
            {
                Log.Error("Permission checked was null.");
                return false;
            }

            if (string.IsNullOrEmpty(groupname))
            {
                Log.Error("GroupName checked was null.");
                return false;
            }

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

            Group group = null;
            if (!_permissionsConfig.Groups.TryGetValue(groupname, out group))
                group = GetDefaultGroup();

            if (group != null)
            {
                if (group.Permissions.Any(s => s == ".*")) return true;
                if (group.Permissions.Any(s => s == "*")) return true;
                if (group.Permissions.Contains(permission.Split('.')[0] + ".*")) return true;
                if (group.Permissions.Any(s => s == permission)) return true;
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