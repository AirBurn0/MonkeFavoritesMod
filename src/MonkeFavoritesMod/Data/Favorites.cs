using System.Collections.Generic;
using System.Reflection;
using SimpleJSON;

namespace MonkeFavoritesMod.Data
{
	[RegisterInState]
	public class Favorites : IWrapTypeOnSave, IManualSave
	{
		[Save]
		public List<string> Values = new List<string>();

		public JSONNode OnManualSave()
		{
			return new JSONObject { [nameof(Values)] = SaveToJSON.CreateNode(Values) };
		}
	}
}