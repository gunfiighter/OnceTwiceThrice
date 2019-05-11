using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StringMap = System.Collections.Generic.List<string>;

namespace OnceTwiceThrice
{
	public class Level
	{
		public StringMap Background;
		public StringMap Items;
		public StringMap Mobs;

		public Level(StringMap background, StringMap items, StringMap mobs)
		{
			Background = background;
			Items = items;
			Mobs = mobs;
		}
	}
	
	public class LevelsList
	{
		public List<Level> Levels;
		public LevelsList() {
			Levels = new List<Level>();
			
			//Level 0
			Levels.Add(new Level( 
				new StringMap
				{
					"GGWGLBGBBGBB",
					"GBGGGBGBBGBB",
					"GGGBGLLLBGBB",
					"GBBBGLGBBGWW"
				}, new StringMap
				{
					"F....AS.....",
					"F...S...F...",
					".........F.S",
					"......DS...D"
				}, new StringMap
				{
					"...........R",
					".M....S.....",
					"............",
					"............"
				}
			));
			
			//Level 1
			Levels.Add(new Level( 
				new StringMap
				{
					"BBBBLLGGGGG",
					"BBBBLLGGGGG",
					"BBBLLLGGGGG",
					"GGGGGGGGGGG",
					"GGGGGGGGGGG",
					"GGGGGGWWWWW",
					"BBBBBBWGGGG",
					"BBBBBBWGGGG",
					"BBBBBBWGGGG",
					
				}, new StringMap
				{
					"..DS..TSSSS",
					".SSS......S",
					"..........A",
					"SSS.....T.S",
					".....F.....",
					"...........",
					"..F........",
					"..D.....FD.",
					"S..........",
				}, new StringMap
				{
					"...........",
					".......R...",
					"...........",
					"...........",
					"...........",
					"..MS.......",
					"...H.......",
					".....R....R",
					"...........",
				}
			));
		}
	}

	public static class StringMapExtension
	{
		public static void Foreach(this StringMap map, Action<int, int> act)
		{
			var xLength = map[0].Length;
			for (var y = 0; y < map.Count; y++)
				for (var x = 0; x < xLength; x++)
					act(x, y);
		}
	} 
}