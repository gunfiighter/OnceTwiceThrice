using System;
using System.Drawing;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class AgaricItem : ItemBase, IItems
	{
		public Image Picture { get; }
		public int Interval;
		
		public AgaricItem(GameModel model, int x, int y) : base(x, y)
		{
			Picture = Helpful.GetImageByName("Agaric");
			Interval = 10;
			model.OnTick += () =>
			{
				Interval++;
				if (Interval == 100)
				{
					Interval = 0;
					var newMob = new SporesMob(model, x - 1, y);
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