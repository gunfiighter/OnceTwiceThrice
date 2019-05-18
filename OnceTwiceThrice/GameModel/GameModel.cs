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

    public class Cell
    {
        public class MyLinkedList<T>
        {
            private readonly LinkedList<T> _value;
            private event Action OnChange;
            public MyLinkedList(Action act)
            {
                _value = new LinkedList<T>();
                this.OnChange += act;
            }

            public void Add(T item)
            {
                _value.AddLast(item);
                OnChange?.Invoke();
            }

            public void Remove(T item)
            {
                _value.Remove(item);
                OnChange?.Invoke();
            }
        }

        private IBackground _back;
        public IBackground Back
        {
            get => _back;
            set
            {
                _back = value;
                OnBackChange?.Invoke();
            }
        }

        public MyLinkedList<IMob> Mobs;
        public MyLinkedList<IHero> Heroes;

        public readonly int X;
        public readonly int Y;

        public event Action OnMobsChange;
        public event Action OnHeroesChange;
        public event Action OnBackChange;

        private void MobsChange() => OnMobsChange?.Invoke();
        private void HeroesChange() => OnHeroesChange?.Invoke();

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
            Mobs = new MyLinkedList<IMob>(MobsChange);
            Heroes = new MyLinkedList<IHero>(HeroesChange);
        }
    }
	
	public class MapDecoder
	{
		public Dictionary<char, Func<int, int, IBackground>> Background;
		public Dictionary<char, Func<int, int, IItem>> Item;
		public Dictionary<char, Func<GameModel, int, int, MovableBase>> hero;

		public MapDecoder(GameModel model)
		{
			Background = new Dictionary<char, Func<int, int, IBackground>>();
			Background.Add('G', (x, y) => new GrassBackground(model, x, y));
			Background.Add('B', (x, y) => new BurnedBackground(model, x, y));
			Background.Add('W', (x, y) => new WaterBackground(model, x, y));
			Background.Add('L', (x, y) => new LavaBackground(model, x, y));
			Background.Add('I', (x, y) => new IceBackground(model, x, y));
			
			Item = new Dictionary<char, Func<int, int, IItem>>();
			Item.Add('S', (x, y) => new StoneItem(model, x, y));
			Item.Add('T', (x, y) => new ThreeItem(model, x, y));
			Item.Add('F', (x, y) => new FireItem(model, x, y));
			Item.Add('D', (x, y) => new DestinationItem(model, x, y));
            Item.Add('A', (x, y) => new AgaricItem(model, x, y));
            Item.Add('<', (x, y) => new FlowItem(model, x, y, Keys.Left, "Flow/Left"));
            Item.Add('>', (x, y) => new FlowItem(model, x, y, Keys.Right, "Flow/Right"));
            Item.Add('^', (x, y) => new FlowItem(model, x, y, Keys.Up, "Flow/Up"));
            Item.Add('v', (x, y) => new FlowItem(model, x, y, Keys.Down, "Flow/Down"));
            Item.Add('4', (x, y) => new SemiConductorItem(model, x, y, Keys.Left, "SemiConductor/Left"));
            Item.Add('6', (x, y) => new SemiConductorItem(model, x, y, Keys.Right, "SemiConductor/Right"));
            Item.Add('8', (x, y) => new SemiConductorItem(model, x, y, Keys.Up, "SemiConductor/Up"));
            Item.Add('2', (x, y) => new SemiConductorItem(model, x, y, Keys.Down, "SemiConductor/Down"));

            hero = new Dictionary<char, Func<GameModel, int, int, MovableBase>>();
			hero.Add('M', (map, x, y) => new MatthiusHero(map, x, y));
            hero.Add('S', (map, x, y) => new SkimletHero(map, x, y));
            hero.Add('H', (map, x, y) => new HowardHero(map, x, y));

			hero.Add('r', (map, x, y) => new RedGolemMob(map, x, y));
			hero.Add('b', (map, x, y) => new BlueGolemMob(map, x, y));
			hero.Add('s', (map, x, y) => new SharkMob(map, x, y));
            hero.Add('f', (map, x, y) => new FrogMob(map, x, y));
			hero.Add('p', (map, x, y) => new PenguinMob(map, x, y));
            hero.Add('d', (map, x, y) => new DinoMob(map, x, y));
            hero.Add('c', (map, x, y) => new CactusMob(map, x, y));
            hero.Add('m', (map, x, y) => new MonkeyMob(map, x, y));
            hero.Add('h', (map, x, y) => new HotGuyMob(map, x, y));
            hero.Add('t', (map, x, y) => new TermiteMob(map, x, y));
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

        public Cell[,] Map;
		
		public IBackground[,] BackMap;
		public Stack<IItem>[,] ItemsMap;
        public LinkedList<IMovable>[,] MobMap;
		public LinkedList<IHero> Heroes;
		public LinkedList<IMob> Mobs;
		public LinkedList<ISpell> Spells;
		public LinkedList<Death> Deaths;

		public event Action OnWin;
		public event Action OnGameOver;
		public event Action<IMovable> OnMobMapChange;
		public event Action OnTick;

		public void GameOver(object sender)
		{
			OnGameOver?.Invoke();
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

		public void MobMapChange(IMovable mob)
		{
			OnMobMapChange?.Invoke(mob);
		}

		public GameModel(Level level)
		{
			Width = level.Background[0].Length;
			Height = level.Background.Length;

            Map = new Cell[Width, Height];
            Map.Foreach((x, y) => Map[x, y] = new Cell(x, y));
			
			BackMap = new IBackground[Width, Height];
			ItemsMap = new Stack<IItem>[Width, Height];
            MobMap = new LinkedList<IMovable>[Width, Height];
            ItemsMap.Foreach((x, y) =>
            {
                ItemsMap[x, y] = new Stack<IItem>();
            });
            MobMap.Foreach((x, y) =>
            {
                MobMap[x, y] = new LinkedList<IMovable>();
            });
            Heroes = new LinkedList<IHero>();
			Mobs = new LinkedList<IMob>();
			Spells = new LinkedList<ISpell>();
			Deaths = new LinkedList<Death>();


			var mapDecoder = new MapDecoder(this);
			
			//Заполнение фона
			BackMap.Foreach((x, y) => { BackMap[x, y] = mapDecoder.Background[level.Background[y][x]](x, y); });

            //Заполнение объектами
            foreach (var itemMap in level.Items)
            {
                itemMap.Foreach((x, y) =>
                {
                    var item = itemMap[y][x];
                    if (mapDecoder.Item.ContainsKey(item))
                        ItemsMap[x, y].Push(mapDecoder.Item[item](x, y));
                });
            }
			
			//Заполнение мобами
			level.Mobs.Foreach((x, y) =>
			{
				var mobChar = level.Mobs[y][x];
				if (mapDecoder.hero.ContainsKey(mobChar))
				{
					var mob = mapDecoder.hero[mobChar](this, x, y);
					if (mob is IHero hero)
						Heroes.AddLast(hero);
					if (mob is IMob imob)
						Mobs.AddLast(imob);
				}
			});

			if (Heroes.Count == 0)
				throw new Exception("No heroes on the model");

            foreach (var switcher in level.Switchers)
            {
                ItemsMap[switcher[0], switcher[1]].Push(
                    new SwitcherItem(
                        this,
                        switcher[0],
                        switcher[1],
                        switcher[2],
                        switcher[3]));
            }

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
