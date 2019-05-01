using System.Collections.Generic;

namespace OnceTwiceThrice
{
	public class Lavel
	{
		public string[] Background;
		public string[] Items;
		public string[] Mobs;

		public Lavel(string[] background, string[] items, string[] mobs)
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
				new[]
				{
					"GBBGBB",
					"GBGGGB",
					"GGGBGB",
					"GBBBGG"
				}, new[]
				{
					"......",
					"......",
					"......",
					"......"
				}, new []
				{
					"......",
					".R....",
					"......",
					"....R."
				}
				));
		}

	}
}