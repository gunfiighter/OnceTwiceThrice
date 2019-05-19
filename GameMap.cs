using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OnceTwiceThrice
{
	public enum Background
	{
		Grass,
		Burned
	}

	public static class BackgroundImages
	{
		public static List<Image> Images;

		static BackgroundImages()
		{
			Images = new List<Image>();
			foreach (var item in Enum.GetValues(typeof(Background)))
				Images.Add(Image.FromFile("../../images/" + item.ToString() + ".png"));
		}
	} 
	
	public static class MapDecoder
	{
		public static Dictionary<char, Background> background;
		public static Dictionary<char, Func<IItems>> item;
		public static Dictionary<char, Func<GameMap, int, int, MovableBase>> hero;

		static MapDecoder()
		{
			background = new Dictionary<char, Background>();
			background.Add('G', Background.Grass);
			background.Add('B', Background.Burned);
//			background.Add('W', Background.Water);
//			background.Add('L', Background.Lava);
//			background.Add('I', Background.Ice);
			
			item = new Dictionary<char, Func<IItems>>();
			item.Add('S', () => new StoneItem());
//			item.Add('T', () => new TreeItem());
//			item.Add('F', () => new FireItem());

			hero = new Dictionary<char, Func<GameMap, int, int, MovableBase>>();
			hero.Add('R', (map, x, y) => new RedWizard(map, "RedWizard", x, y));
		}
	} 
	
	public class GameMap
	{
		public MyForm form;
		public readonly int Width;
		public readonly int Height;
		
		public Background[,] BackMap;
		public IItems[,] ItemsMap;
		public List<MovableBase> Heroes;
		
		public GameMap(MyForm form, Lavel lavel)
		{
			this.form = form;
			Width = lavel.Background[0].Length;
			Height = lavel.Background.Count;
			BackMap = new Background[Width, Height];
			ItemsMap = new IItems[Width, Height];
			Heroes = new List<MovableBase>();
			
			//Заполнение фона
			BackMap.Foreach((x, y) => { BackMap[x, y] = MapDecoder.background[lavel.Background[y][x]]; });
			
			//Заполнение объектами
			Action<char, int, int> AddItem = (c, x, y) => { ItemsMap[x, y] = MapDecoder.item[c](); };
			
			lavel.Items.Foreach((x, y) =>
			{
				var item = lavel.Items[y][x];
				switch (item)
				{
					case 'S': AddItem(item, x, y); break;
				}
			});
			
			//Заполнение мобами
			Action<char, int, int> AddHero = (c, x, y) => { Heroes.Add(MapDecoder.hero[c](this, x, y)); };
			
			lavel.Mobs.Foreach((x, y) =>
			{
				var hero = lavel.Mobs[y][x];
				switch (hero)
				{
					case 'R': AddHero(hero, x, y); break;
				}
			});

			if (Heroes.Count == 0)
				throw new Exception("No heroes on the map"); 
		}

		public bool IsInsideMap(int x, int y)
		{
			return
				x >= 0 && x < Width &&
				y >= 0 && y < Height;
		}

		public bool IsInsideMap(int x, int y, Keys key)
		{
			var dx = 0;
			var dy = 0;
			switch (key)
			{
				case Keys.Up: dy = -1; break;
				case Keys.Down: dy = 1; break;
				case Keys.Left: dx = -1; break;
				case Keys.Right: dx = 1; break;
				default: throw new ArgumentException();
			}
			var newX = x + dx;
			var newY = y + dy;
			return
				IsInsideMap(newX, newY);
		}

		public void DrawBackground(Graphics g)
		{
			BackMap.Foreach((x, y) =>
			{
				g.DrawImage(
					BackgroundImages.Images[(int)BackMap[x, y]], 
					x * MyForm.DrawingScope, 
					y * MyForm.DrawingScope);
			});
		}

		public void DrawItems(Graphics g)
		{
			ItemsMap.Foreach((x, y) =>
			{
				if (ItemsMap[x, y] != null)
					g.DrawImage(
						ItemsMap[x, y].Picture,
						x * MyForm.DrawingScope,
						y * MyForm.DrawingScope);
			});
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
