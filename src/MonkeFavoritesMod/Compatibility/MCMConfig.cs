namespace MonkeFavoritesMod.Compatibility
{
	public class MCMConfig
	{
		public bool MessyFavoritesFristSort { set; get; } = false;

		public static void Load(string path)
		{
			MCMCompat.Config = System.IO.File.Exists(path) ? Newtonsoft.Json.JsonConvert.DeserializeObject<MCMConfig>(System.IO.File.ReadAllText(path)) ?? new() : new();
		}

		public void Save(string path)
        {
            var data = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            System.IO.File.WriteAllText(path, data);
        }

    }
}