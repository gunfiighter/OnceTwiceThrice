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
		public Dictionary<char, Func<int, int, IBackground>> background;
		public Dictionary<char, Func<int, int, IItems>> item;
		public Dictionary<char, Func<GameModel, int, int, MovableBase>> hero;

		public MapDecoder(GameModel model)
		{
			background = new Dictionary<char, Func<int, int, IBackground>>();
			background.Add('G', (x, y) => new GrassBackground(model, x, y));
			background.Add('B', (x, y) => new BurnedBackground(model, x, y));
			background.Add('W', (x, y) => new WaterBackground(model, x, y));
			background.Add('L', (x, y) => new LavaBackground(model, x, y));
			background.Add('I', (x, y) => new IceBackground(model, x, y));
			
			item = new Dictionary<char, Func<int, int, IItems>>();
			item.Add('S', (x, y) => new StoneItem(model, x, y));
			item.Add('T', (x, y) => new ThreeItem(model, x, y));
			item.Add('F', (x, y) => new FireItem(model, x, y));
			item.Add('D', (x, y) => new DestinationItem(model, x, y));
			item.Add('A', (x, y) => new AgaricItem(model, x, y));

			hero = new Dictionary<char, Func<GameModel, int, int, MovableBase>>();
			hero.Add('M', (map, x, y) => new MatthiusHero(map, x, y));
            hero.Add('S', (map, x, y) => new SkimletHero(map, x, y));
            hero.Add('H', (map, x, y) => new HowardHero(map, x, y));

			hero.Add('r', (map, x, y) => new RedGolemMob(map, x, y));
            hero.Add('s', (map, x, y) => new SharkMob(map, x, y));
            hero.Add('f', (map, x, y) => new FrogMob(map, x, y));
            hero.Add('p', (map, x, y) => new PenguinMob(map, x, y));
        }
	} 
	
	public class GameModel
	{
		public IHero CurrentHero;
		private IEnumerator<IHero> heroEnumerator;
		public readonly int Width;
		public readonly int Height;
		public int TickCount { get; private set; }

        public bool NeedInvalidate { get; set; }
		
		public IBackground[,] BackMap;
		public Stack<IItems>[,] ItemsMap;
        public LinkedList<IMovable>[,] MobMap;
		public LinkedList<IHero> Heroes;
		public LinkedList<IMob> Mobs;
		public LinkedList<ISpell> Spells;

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
			TickCount++;
			OnTick?.Invoke();
			
		}
		
		public GameModel(Level lavel)
		{
			Width = lavel.Background[0].Length;
			Height = lavel.Background.Length;
			
			BackMap = new IBackground[Width, Height];
			ItemsMap = new Stack<IItems>[Width, Height];
            MobMap = new LinkedList<IMovable>[Width, Height];
            ItemsMap.Foreach((x, y) =>
            {
                ItemsMap[x, y] = new Stack<IItems>();
            });
            MobMap.Foreach((x, y) =>
            {
                MobMap[x, y] = new LinkedList<IMovable>();
            });
            Heroes = new LinkedList<IHero>();
			Mobs = new LinkedList<IMob>();
			Spells = new LinkedList<ISpell>();
            
			
			var mapDecoder = new MapDecoder(this);
			
			//Заполнение фона
			BackMap.Foreach((x, y) => { BackMap[x, y] = mapDecoder.background[lavel.Background[y][x]](x, y); });
			
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
			var newX = 0;
			var newY = 0;
			Useful.XyPlusKeys(x, y, key, ref newX, ref newY);
			
			return
				IsInsideMap(newX, newY);
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
