using System;
using System.Drawing;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public interface IItems
	{
		Image Picture { get; }
		bool CanStep(MovableBase mob);//Можно ли наступить на объект
		bool CanStop(MovableBase mob); //Блокирует ли объект команды на движения мобов, находящемся на нем
		int X { get; }
		int Y { get; }
	}

	public class ItemBase
	{
		public int X { get; }
		public int Y { get; }

		public ItemBase(int X, int Y)
		{
			;
		}
	}
	
	public class StoneItem : ItemBase, IItems
	{
		public Image Picture { get; }
		public StoneItem(int X, int Y) : base(X, Y)
		{
			Picture = Helpful.GetImageByName("Stone");
		}

		public bool CanStep(MovableBase mob) => false;
		public bool CanStop(MovableBase mob) => true;
	}
	
	public class FireItem : ItemBase, IItems
	{
		public Image Picture { get; }
		public FireItem(int X, int Y) : base(X, Y)
		{
			Picture = Helpful.GetImageByName("Fire");
		}

		public bool CanStep(MovableBase mob) => false;
		public bool CanStop(MovableBase mob) => true;
	}
	
	public class DestinationItem : ItemBase, IItems
	{
		public Image Picture { get; }
		public DestinationItem(int X, int Y) : base(X, Y)
		{
			Picture = Helpful.GetImageByName("Destination_4");
		}

		public bool CanStep(MovableBase mob) => true;
		public bool CanStop(MovableBase mob) => true;
	}
	
	public class AgaricItem : ItemBase, IItems
	{
		public Image Picture { get; }
		private GameModel model;
		public int Interval;
		
		public AgaricItem(GameModel model, int X, int Y) : base(X, Y)
		{
			Picture = Helpful.GetImageByName("Agaric");
			this.model = model;
			Interval = 10;
			this.model.OnTick += () =>
			{
				Interval++;
				if (Interval == 100)
				{
					Interval = 0;
					var newMob = model.MapDecoder.hero['P'](model, X - 1, Y) as IMob;
					newMob.KeyMap.TurnOn(Keys.Left);
					newMob.MakeMove(Keys.Left);
					model.Mobs.AddLast(newMob);
				}
			};
		}

		public bool CanStep(MovableBase mob) => true;
		public bool CanStop(MovableBase mob) => true;
	}
}