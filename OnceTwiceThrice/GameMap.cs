using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

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
//		public static Dictionary<char, Func<IItem>> item;
		public static Dictionary<char, Func<GameMap, int, int, ImageModel>> hero;

		static MapDecoder()
		{
			background = new Dictionary<char, Background>();
			background.Add('G', Background.Grass);
			background.Add('B', Background.Burned);
//			background.Add('W', Background.Water);
//			background.Add('L', Background.Lava);
//			background.Add('I', Background.Ice);
			
//			item = new Dictionary<char, Func<IItem>>();
//			item.Add('S', () => new StoneItem());
//			item.Add('T', () => new TreeItem());
//			item.Add('F', () => new FireItem());

			hero = new Dictionary<char, Func<GameMap, int, int, ImageModel>>();
			hero.Add('R', (map, x, y) => new ImageModel(map, "RedWizard", x, y));
		}
	} 
	
	public class GameMap
	{
		public MyForm form;
		public readonly int Width;
		public readonly int Height;
		
		public Background[,] BackMap;
		public List<ImageModel> Heroes;
		
		public GameMap(MyForm form, Lavel lavel)
		{
			this.form = form;
			Width = lavel.Background[0].Length;
			Height = lavel.Background.GetLength(0);
			BackMap = new Background[Width, Height];
			Heroes = new List<ImageModel>();
			
			for (var i = 0; i < Width; i++)
			for (var j = 0; j < Height; j++)
				BackMap[i, j] = MapDecoder.background[lavel.Background[j][i]];
			
			for (var i = 0; i < Width; i++)
			for (var j = 0; j < Height; j++)
			{
				var hero = lavel.Mobs[j][i];
				switch (hero)
				{
					case 'R': Heroes.Add(MapDecoder.hero[hero](this, i, j)); break;
				}
			}

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
			for (int i = 0; i < Width; i++)
			for (int j = 0; j < Height; j++)
				g.DrawImage(
					BackgroundImages.Images[(int)BackMap[i, j]], 
					i * MyForm.DrawingScope, 
					j * MyForm.DrawingScope);
		}
	}
}
