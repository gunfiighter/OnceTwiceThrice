using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OnceTwiceThrice
{	
	public static class Grass
	{
		public static string Name = "Grass";
	}
	
	public static class MapDecoder
	{
		public static Dictionary<char, IBackground> background;
		public static Dictionary<char, Func<IItems>> item;
		public static Dictionary<char, Func<GameModel, int, int, MovableBase>> hero;

		static MapDecoder()
		{
			background = new Dictionary<char, IBackground>();
			background.Add('G', new GrassBackground());
			background.Add('B', new BurnedBackground());
//			background.Add('W', Background.Water);
//			background.Add('L', Background.Lava);
//			background.Add('I', Background.Ice);
			
			item = new Dictionary<char, Func<IItems>>();
			item.Add('S', () => new StoneItem());
//			item.Add('T', () => new TreeItem());
			item.Add('F', () => new FireItem());

			hero = new Dictionary<char, Func<GameModel, int, int, MovableBase>>();
			hero.Add('M', (map, x, y) => new MatthiusHero(map, x, y));
			hero.Add('S', (map, x, y) => new SkimletHero(map, x, y));
			hero.Add('R', (map, x, y) => new RighterMob(map, x, y));
		}
	} 
	
	public class GameModel
	{
		public IHero CurrentHero;
		private IEnumerator<IHero> heroEnumerator;
		public readonly int Width;
		public readonly int Height;
		
		public IBackground[,] BackMap;
		public Stack<IItems>[,] ItemsMap;
		public List<IHero> Heroes;
		public List<IMob> Mobs;
		
		public GameModel(Lavel lavel)
		{
			Width = lavel.Background[0].Length;
			Height = lavel.Background.Count;
			
			BackMap = new IBackground[Width, Height];
			ItemsMap = new Stack<IItems>[Width, Height];
			ItemsMap.Foreach((x, y) =>
			{
				ItemsMap[x, y] = new Stack<IItems>();
			});
			Heroes = new List<IHero>();
			Mobs = new List<IMob>();
			
			//Заполнение фона
			BackMap.Foreach((x, y) => { BackMap[x, y] = MapDecoder.background[lavel.Background[y][x]]; });
			
			//Заполнение объектами
			lavel.Items.Foreach((x, y) =>
			{
				var item = lavel.Items[y][x];
				if (MapDecoder.item.ContainsKey(item))
					ItemsMap[x, y].Push(MapDecoder.item[item]());
			});
			
			//Заполнение мобами
			lavel.Mobs.Foreach((x, y) =>
			{
				var mobChar = lavel.Mobs[y][x];
				if (MapDecoder.hero.ContainsKey(mobChar))
				{
					var mob = MapDecoder.hero[mobChar](this, x, y);
					if (mob is IHero)
						Heroes.Add((IHero)mob);
					if (mob is IMob)
						Mobs.Add((IMob)mob);
				}
			});

			if (Heroes.Count == 0)
				throw new Exception("No heroes on the model");

			heroEnumerator = Heroes.GetEnumerator();
			SwitchHero();
		}

		public bool IsInsideMap(int x, int y)
		{
			return
				x >= 0 && x < Width &&
				y >= 0 && y < Height;
		}

		public bool IsInsideMap(int x, int y, Keys key)
		{
			int newX = 0;
			int newY = 0;
			Helpful.XYPlusKeys(x, y, key, ref newX, ref newY);
			
			return
				IsInsideMap(newX, newY);
		}

		public void DrawBackground(Graphics g)
		{
			BackMap.Foreach((x, y) =>
			{
				g.DrawImage(
					BackMap[x, y].Picture, 
					x * MyForm.DrawingScope, 
					y * MyForm.DrawingScope);
			});
		}

		public void DrawItems(Graphics g)
		{
			ItemsMap.Foreach((x, y) =>
			{
				foreach (var item in ItemsMap[x, y])
				{
					g.DrawImage(
                        item.Picture,
                        x * MyForm.DrawingScope,
                        y * MyForm.DrawingScope);
				}
			});
		}

		public void SwitchHero()
		{
			if (!heroEnumerator.MoveNext())
			{
				heroEnumerator = Heroes.GetEnumerator();
				if (!heroEnumerator.MoveNext())
					throw new Exception("No heroes in the model");
			}

			CurrentHero = heroEnumerator.Current;
		}
	}

	public static class TwoDimensionalExtension
	{
		public static void Foreach<T>(this T[,] array, Action<int, int> act)
		{
			for (var i = 0; i < array.GetLength(0); i++)
			for (var j = 0; j < array.GetLength(1); j++)
				act(i, j);
		}
	}
}
