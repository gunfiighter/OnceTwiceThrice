using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class AgaricItem : ItemBase, IItem
	{
        private Keys lastDirection;
		public AgaricItem(GameModel model, int x, int y) : base(model, x, y, 4, "Agaric")
		{
            lastDirection = Keys.Up;
            model.OnTick += onTick;
		}

        public override void onTick()
        {
            if (Model.TickCount % 10 == 0)
            {
                this.ChangeSlide();
                if (this.animationCounter == 3)
                {
                    if (!checkDirection(lastDirection))
                        lastDirection = findTarget();
                    if (Useful.KeyIsMove(lastDirection))
                        Shoot(Model, X, Y, lastDirection);
                }
            }
        }

        private bool checkDirection(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                    for (var i = Y - 1; i >= 0; i--)
                        foreach (var mob in Model.MobMap[X, i])
                            if (mob is IHero)
                                return true;
                    break;
                case Keys.Down:
                    for (var i = Y + 1; i < Model.Height; i++)
                        foreach (var mob in Model.MobMap[X, i])
                            if (mob is IHero)
                                return true;
                    break;
                case Keys.Left:
                    for (var i = X - 1; i >= 0; i--)
                        foreach (var mob in Model.MobMap[i, Y])
                            if (mob is IHero)
                                return true;
                    break;
                case Keys.Right:
                    for (var i = X + 1; i < Model.Width; i++)
                        foreach (var mob in Model.MobMap[i, Y])
                            if (mob is IHero)
                                return true;
                    break;
            }
            return false;
        }

        private Keys findTarget()
        {
            if (checkDirection(Keys.Up))
                return Keys.Up;
            if (checkDirection(Keys.Down))
                return Keys.Down;
            if (checkDirection(Keys.Right))
                return Keys.Right;
            if (checkDirection(Keys.Left))
                return Keys.Left;
            return Keys.None;
        }

		private void Shoot(GameModel model, int startX, int startY, Keys direction)
		{
			var newMob = new SporeMob(model, startX, startY);
			model.Mobs.AddLast(newMob);
			newMob.GoTo(direction);
		}

		public bool CanStep(MovableBase mob) => false;
		public bool CanStop(MovableBase mob) => true;
	}
}