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
	
	public class LavelsList
	{
		public List<Lavel> Levels;
		public LavelsList() {
			Levels = new List<Lavel>();
			
			//Lavel 0
			Levels.Add(new Lavel( 
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
			Levels.Add(new Lavel( 
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
					"..DS..SSSSS",
					".SSS......S",
					"..........A",
					"SSS.......S",
					".....F.....",
					"...........",
					"..F........",
					"........FD.",
					"S..........",
				}, new StringMap
				{
					"...........",
					".......R...",
					"...........",
					"...........",
					"...........",
					"..MS.......",
					"...........",
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
			for (var y = 0; y < map.Count; y++)
			for (var x = 0; x < map[y].Length; x++)
				act(x, y);
		}
	} 
}