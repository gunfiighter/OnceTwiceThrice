using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class AgaricItem : ItemBase, IItems
	{
		public AgaricItem(GameModel model, int x, int y) : base(x, y, 4, "Agaric")
		{
			
			model.OnTick += () =>
			{
				if (model.TickCount % 10 == 0) {
					this.ChangeSlide();
					if(this.animationCounter == 3)
						Shoot(model, x, y);
				}
			};
		}

		private void Shoot(GameModel model, int startX, int startY)
		{
			var newMob = new SporeMob(model, startX, startY);
			newMob.KeyMap.TurnOn(Keys.Left);
			newMob.MakeMove(Keys.Left);
			model.Mobs.AddLast(newMob);
		}

		public bool CanStep(MovableBase mob) => true;
		public bool CanStop(MovableBase mob) => true;
	}
}