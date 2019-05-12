using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class MobBase : MovableBase
	{
		public MobBase(GameModel model, string ImageFile, int X, int Y) : base(model, ImageFile, X, Y)
		{
			;
			OnMoveStart += MoveStart;
		}

		public virtual void MoveStart()
		{
			foreach (var hero in Model.Heroes)
			{
				if ((hero.MX == MX && hero.MY == MY) || (hero.X == MX && hero.Y == MY))
				{
					var needDeath = true;
					if (hero.GazeDirection == GazeDirection)
					{
						switch (GazeDirection)
						{
							case Keys.Right: if (X > hero.X) needDeath = false; break;
							case Keys.Left: if (X < hero.X) needDeath = false; break;
							case Keys.Up: if (Y < hero.Y) needDeath = false; break;
							case Keys.Down: if (Y > hero.Y) needDeath = false; break;
						}
					}
					if (needDeath)
						hero.Destroy();
					return;
				}
			}
		}

        public void GoTo(Keys direction)
        {
            KeyMap.TurnOff();
            KeyMap.TurnOn(direction);
            MakeMove(direction);
        }
        
		public override double Speed
		{
			get { return 0.033; }
		}
	}
}