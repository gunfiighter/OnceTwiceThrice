using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
//using StringMap = System.Collections.Generic.List<string>;

namespace OnceTwiceThrice
{
	public class Level
	{
		public string[] Background;
		public List<string[]> Items;
		public string[] Mobs;
        public List<int[]> Switchers;

		public Level(string[] background, List<string[]> items, string[] mobs, string[] additionally)
		{
			Background = background;
			Items = items;
			Mobs = mobs;

            Switchers = new List<int[]>();
            foreach (var line in additionally)
                Switchers.Add(line.Split().Select(c => int.Parse(c)).ToArray());
		}
	}
	
	public class LevelsList
	{
		public List<Level> Levels;
		public LevelsList() {
			Levels = new List<Level>();
			for (var i = 0; i < 10; i++)
				Levels.Add(LevelFromFile("../../Levels/Level" + i + ".txt"));
		}

		public Level LevelFromFile(string file)
		{
			var t = File.ReadAllText(file).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var size = t[0].Split();
            var width = int.Parse(size[0]);
            var height = int.Parse(size[1]);

            

            var backIndex = findStr(t, "//back") + 1;

            var itemList = new List<string[]>();
            for (int i = 0; i < t.Length; i++)
                if (t[i] == "//item")
                    itemList.Add(Useful.CutArray(t, i + 1, i + height));

            var mobIndex = findStr(t, "//mob") + 1;

            var switcherIndex = findStr(t, "//switcher") + 1;

			return new Level(
				Useful.CutArray(t, backIndex, backIndex + height - 1),
				itemList,
				Useful.CutArray(t, mobIndex, mobIndex + height - 1),
                Useful.CutArray(t, switcherIndex, t.Length - 1)
			);
		}

        private int findStr(string[] t, string str)
        {
            var result = 0;
            while (result < t.Length && t[result] != str)
                result++;
            return result;
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