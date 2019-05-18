using System;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class HeroBase : MovableBase
	{
		public HeroBase(GameModel model, string ImageFile, int X, int Y) : base(model, ImageFile, X, Y)
		{
			OnDestroy += () => {
				model.GameOver(this);
			};
			OnStop += () =>
			{
				var itemsStack = model.Map[this.X, this.Y].Items;
				if (itemsStack.Count > 0 && itemsStack.Peek() is DestinationItem)
					model.Win();
			};
			OnMoveStart += MoveStart;
		}

		public override sbyte SlidesCount => 4;

		public void CreateSpell(Func<int, int, ISpell> spell)
        {
            var newX = 0;
            var newY = 0;
            Useful.XyPlusKeys(X, Y, this.GazeDirection, ref newX, ref newY);
            Model.Spells.AddLast(spell(newX, newY));
        }

		public virtual void MoveStart()
		{
			foreach (var mob in Model.Mobs)
			{
				if ((mob.MX == MX && mob.MY == MY) || (mob.X == MX && mob.Y == MY))
				{
					var needDeath = true;
					if (mob.CurrentAnimation.IsMoving && mob.GazeDirection == GazeDirection)
					{
						switch (GazeDirection)
						{
							case Keys.Right: if (X < mob.X) needDeath = false; break;
							case Keys.Left: if (X > mob.X) needDeath = false; break;
							case Keys.Up: if (Y > mob.Y) needDeath = false; break;
							case Keys.Down: if (Y < mob.Y) needDeath = false; break;
						}
					}
					if (needDeath)
						Destroy();
					return;
				}
			}
		}

        public override bool IceSlip => true;

        public override double Speed
		{
			get { return 0.05; }
		}
	}
}