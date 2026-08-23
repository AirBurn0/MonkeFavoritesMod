using System.Collections.Generic;
using System.IO;
using ModConfigMenu;
using ModConfigMenu.Contracts;
using ModConfigMenu.Objects;

namespace MonkeFavoritesMod.Compatibility
{
	public class MCMCompat
	{
        private static string ConfigDir => Path.Combine(ModConfigMenu.Plugin.AllModsConfigPath, MonkeFavoritesMod.ModName);
		private static string ConfigPath => Path.Combine(ConfigDir, "config.json");
		public static MCMConfig? Config = null;
		
		public static void Init()
		{
			Directory.CreateDirectory(ConfigDir);
			MCMConfig.Load(ConfigPath);
			List<IConfigValue> configs =
            [
                new ConfigValue(nameof(MCMConfig.MessyFavoritesFristSort), Config?.MessyFavoritesFristSort, "mcm.MonkeFavoritesMod.header_sort", false, "mcm.MonkeFavoritesMod.messyFavoritesFristSort.desc", "mcm.MonkeFavoritesMod.messyFavoritesFristSort")
            ];
			ModConfigMenuAPI.RegisterModConfig(MonkeFavoritesMod.ModName, configs, OnSave);
		}

		public static bool OnSave(Dictionary<string, object> currentConfig, out string feedbackMessage)
		{
			if(Config is null)
			{
				feedbackMessage = $"{MonkeFavoritesMod.ModName} config is null.";
				return false;
			}
			if(currentConfig.TryGetValue(nameof(MCMConfig.MessyFavoritesFristSort), out object value) && value is bool v)
			{
				Config.MessyFavoritesFristSort = v;
			}
			Config.Save(Path.Combine(ConfigPath));
			feedbackMessage = $"{MonkeFavoritesMod.ModName} config saved.";
			return true;
		}

	}

}