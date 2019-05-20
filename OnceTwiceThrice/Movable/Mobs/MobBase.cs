
namespace OnceTwiceThrice
{
	public class MobBase : MovableBase
	{
		public MobBase(GameModel model, string ImageFile, int X, int Y) : base(model, ImageFile, X, Y)
		{
			OnMoveStart += MoveStart;
            iMob = this as IMob;
		}

        public virtual void MoveStart()
        {
            //foreach (var hero in Model.Heroes)
            //{
            //    if ((hero.MX == MX && hero.MY == MY) || (hero.X == MX && hero.Y == MY))
            //    {
            //        var needDeath = true;
            //        if (hero.GazeDirection == GazeDirection)
            //        {
            //            switch (GazeDirection)
            //            {
            //                case Keys.Right: if (X > hero.X) needDeath = false; break;
            //                case Keys.Left: if (X < hero.X) needDeath = false; break;
            //                case Keys.Up: if (Y < hero.Y) needDeath = false; break;
            //                case Keys.Down: if (Y > hero.Y) needDeath = false; break;
            //            }
            //        }
            //        if (needDeath)
            //            hero.Destroy();
            //        return;
            //    }
            //}
        }

        public void TryKill(IMovable mob)
        {
            if (mob is IMob imob)
                TryKill(imob);
        }

        public void TryKill(IMob mob)
        {
            if (CanKill(mob))
                mob.Destroy();
        }

        public override double Speed
		{
			get { return 0.033; }
		}
	}
}