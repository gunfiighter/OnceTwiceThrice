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
	
	public class MapDecoder
	{
		public Dictionary<char, IBackground> background;
		public Dictionary<char, Func<int, int, IItems>> item;
		public Dictionary<char, Func<GameModel, int, int, MovableBase>> hero;

		public MapDecoder(GameModel model)
		{
			background = new Dictionary<char, IBackground>();
			background.Add('G', new GrassBackground());
			background.Add('B', new BurnedBackground());
			background.Add('W', new WaterBackground());
			background.Add('L', new LavaBackground());
//			background.Add('I', Background.Ice);
			
			item = new Dictionary<char, Func<int, int, IItems>>();
			item.Add('S', (x, y) => new StoneItem(x, y));
//			item.Add('T', (x, y) => new TreeItem(x, y));
			item.Add('F', (x, y) => new FireItem(x, y));
			item.Add('D', (x, y) => new DestinationItem(x, y));
			item.Add('A', (x, y) => new AgaricItem(model, x, y));

			hero = new Dictionary<char, Func<GameModel, int, int, MovableBase>>();
			hero.Add('M', (map, x, y) => new MatthiusHero(map, x, y));
			hero.Add('S', (map, x, y) => new SkimletHero(map, x, y));
			hero.Add('R', (map, x, y) => new RighterMob(map, x, y));
		}
	} 
	
	public class GameModel
	{
		public IHero CurrentHero;
		private IEnumerator<IHero> _heroEnumerator;
		public readonly int Width;
		public readonly int Height;
		
		public IBackground[,] BackMap;
		public Stack<IItems>[,] ItemsMap;
		public LinkedList<IHero> Heroes;
		public LinkedList<IMob> Mobs;

		public event Action OnWin;
		public event Action OnGameOver;
		public event Action OnTick;

		public void GameOver(object sender)
		{
			if (OnGameOver != null)
				OnGameOver();
		}

		public void Win()
		{
			foreach (var hero in Heroes)
			{
				var itemsStack = ItemsMap[hero.X, hero.Y];
				if (itemsStack.Count == 0 || !(itemsStack.Peek() is DestinationItem))
					return;
			}

			OnWin?.Invoke();
		}

		public void Tick()
		{
			OnTick?.Invoke();
		}
		
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
			Heroes = new LinkedList<IHero>();
			Mobs = new LinkedList<IMob>();
			
			var mapDecoder = new MapDecoder(this);
			
			//Заполнение фона
			BackMap.Foreach((x, y) => { BackMap[x, y] = mapDecoder.background[lavel.Background[y][x]]; });
			
			//Заполнение объектами
			lavel.Items.Foreach((x, y) =>
			{
				var item = lavel.Items[y][x];
				if (mapDecoder.item.ContainsKey(item))
					ItemsMap[x, y].Push(mapDecoder.item[item](x, y));
			});
			
			//Заполнение мобами
			lavel.Mobs.Foreach((x, y) =>
			{
				var mobChar = lavel.Mobs[y][x];
				if (mapDecoder.hero.ContainsKey(mobChar))
				{
					var mob = mapDecoder.hero[mobChar](this, x, y);
					if (mob is IHero)
						Heroes.AddLast(mob as IHero);
					if (mob is IMob)
						Mobs.AddLast(mob as IMob);
				}
			});

			if (Heroes.Count == 0)
				throw new Exception("No heroes on the model");

			_heroEnumerator = Heroes.GetEnumerator();
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
			var newX = 0;
			var newY = 0;
			Helpful.XyPlusKeys(x, y, key, ref newX, ref newY);
			
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
			if (!_heroEnumerator.MoveNext())
			{
				_heroEnumerator = Heroes.GetEnumerator();
				if (!_heroEnumerator.MoveNext())
					throw new Exception("No heroes in the model");
			}

			CurrentHero = _heroEnumerator.Current;
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
