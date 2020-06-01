using Synapse.Api;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Synapse.Permissions
{
    internal static class PermissionReader
    {
        //Variablen
        internal static YML permissionsconfig;
        internal static string permissionPath = Path.Combine(PluginManager.ServerConfigDirectory, "permissions.yml");

        //Methoden
        internal static void Init()
        {
            if (!File.Exists(permissionPath))
                File.Create(permissionPath).Close();

            ReloadPermission();
        }

        internal static void ReloadPermission()
        {
            string yml = File.ReadAllText(permissionPath);

            var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

            permissionsconfig = deserializer.Deserialize<YML>(yml);

            foreach (string key in permissionsconfig.groups.Keys)
            {
                permissionsconfig.groups.TryGetValue(key, out Group group);

                foreach (string permission in group.inheritance)
                {
                    Group ErbGruppe = null;
                    permissionsconfig.groups.TryGetValue(permission, out ErbGruppe);

                    if (ErbGruppe != null)
                        foreach (string Erbpermissons in ErbGruppe.permissions)
                            if (!group.permissions.Contains(Erbpermissons))
                                group.permissions.Add(Erbpermissons);

                }
            }
        }

		internal static Group GetDefaultGroup()
		{
			foreach (KeyValuePair<string, Group> gr in permissionsconfig.groups)
			{
				if (gr.Value.Default)
					return gr.Value;
			}
			return null;
		}

		internal static Group GetNWGroup()
		{
			foreach (KeyValuePair<string, Group> gr in permissionsconfig.groups)
			{
				if (gr.Value.Northwood)
					return gr.Value;
			}
			return null;
		}

		internal static bool CheckPermission(ReferenceHub player, string permission)
		{
			if (player == null)
			{
				Log.Error("Der Referncehub war Null weshalb man keine Permission heraus finden kann!");
				return false;
			}


			if (string.IsNullOrEmpty(permission))
			{
				Log.Error("Permission checked was null.");
				return false;
			}

			UserGroup userGroup = ServerStatic.GetPermissionsHandler().GetUserGroup(player.GetUserId());

			Group group = null;
			if (userGroup != null)
			{
				string groupName = ServerStatic.GetPermissionsHandler()._groups.FirstOrDefault(g => g.Value == player.serverRoles.Group).Key;
				if (permissionsconfig == null)
				{
					Log.Error("Permission Config ist null");
					return false;
				}

				if (!permissionsconfig.groups.Any())
				{
					Log.Error("Keine permissionconfig gruppe.");
					return false;
				}

				if (!permissionsconfig.groups.TryGetValue(groupName, out group))
				{
					Log.Error("Could not get permission value.");
					return false;
				}
			}
			else
			{
				if (player.serverRoles.Staff  || player.GetUserId().EndsWith("@northwood")) group = GetNWGroup();
				else group = GetDefaultGroup();
			}

			if (group != null)
			{
				if (permission.Contains("."))
				{
					if (group.permissions.Any(s => s == ".*"))
					{
						return true;
					}
				}
				if (group.permissions.Contains(permission.Split('.')[0] + ".*"))
				{
					return true;
				}

				if (group.permissions.Contains(permission) || group.permissions.Contains("*"))
				{
					return true;
				}
			}
			else
			{
				return false;
			}
			return false;
		}
	}

    internal class YML
    {
        public Dictionary<string, Group> groups { get; set; } = new Dictionary<string, Group>();
    }

    internal class Group
    {
        [YamlMember(Alias = "default")]
        public bool Default { get; set; } = false;
        [YamlMember(Alias = "northwood")]
        public bool Northwood { get; set; } = false;
        public List<string> inheritance { get; set; } = new List<string>();
        public List<string> permissions { get; set; } = new List<string>();
    }
}
