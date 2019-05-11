using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
//using StringMap = System.Collections.Generic.List<string>;

namespace OnceTwiceThrice
{
	public class Level
	{
		public string[] Background;
		public string[] Items;
		public string[] Mobs;

		public Level(string[] background, string[] items, string[] mobs)
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
			for (var i = 0; i < 2; i++)
				Levels.Add(LevelFromFile("../../Levels/Level" + i + ".txt"));
		}

		public Level LevelFromFile(string file)
		{
			var t = File.ReadAllText(file).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			var third = t.Length / 3;
			return new Level(
				Useful.CutArray(t, 0, third - 1),
				Useful.CutArray(t, third, third * 2 - 1),
				Useful.CutArray(t, third * 2, third * 3 - 1)
			);
		}
	}

	public static class StringMapExtension
	{
		public static void Foreach(this string[] map, Action<int, int> act)
		{
			var xLength = map[0].Length;
			for (var y = 0; y < map.Length; y++)
				for (var x = 0; x < xLength; x++)
					act(x, y);
		}
	} 
}