using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StringMap = System.Collections.Generic.List<string>;

namespace OnceTwiceThrice
{
	public class Lavel
	{
		public StringMap Background;
		public StringMap Items;
		public StringMap Mobs;

		public Lavel(StringMap background, StringMap items, StringMap mobs)
		{
			Background = background;
			Items = items;
			Mobs = mobs;
		}
	}
	
	public static class LavelsList
	{
		public static List<Lavel> Levels;
		static LavelsList() {
			Levels = new List<Lavel>();
			
			//Lavel 0
			Levels.Add(new Lavel( 
				new StringMap
				{
					"GBBGBBGBBGBB",
					"GBGGGBGBBGBB",
					"GGGBGBGBBGBB",
					"GBBBGGGBBGBB"
				}, new StringMap
				{
					"............",
					"....S.......",
					"............",
					"............"
				}, new StringMap
				{
					"............",
					".R..........",
					"............",
					"....R......."
				}
				));
		}
	}

	public static class StringMapExtension
	{
		public static void Foreach(this StringMap map, Action<int, int> act)
		{
			for (var y = 0; y < map.Count; y++)
			for (var x = 0; x < map[y].Length; x++)
				act(x, y);
		}
	} 
}