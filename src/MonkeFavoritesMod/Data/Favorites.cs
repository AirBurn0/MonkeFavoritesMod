using System.Collections.Generic;
using SimpleJSON;

namespace MonkeFavoritesMod.Data
{
	[RegisterInState]
	public class Favorites : IWrapTypeOnSave, IManualSave
	{
		[Save]
		public List<string> Values = [];

		public JSONNode OnManualSave()
		{
			return new JSONObject { [nameof(Values)] = SaveToJSON.CreateNode(Values) };
		}
	}
}