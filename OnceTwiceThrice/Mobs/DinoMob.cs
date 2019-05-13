using System;
using System.Linq;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class DinoMob : MobBase, IMob
	{
		public static string ImagePath = "Dino/";

		public DinoMob(GameModel model, int x, int y) : base(model, ImagePath, x, y)
		{
			OnMoveStart += base.ForMoveStart;

			model.OnMobMapChange += (mob) =>
			{
				if (mob == this)
					return;
				var mobX = mob.MX;
				var mobY = mob.MY;
				if (Math.Abs(X - mobX) <= 2 && Math.Abs(Y - mobY) == 0)
				{
					if (mobX > X)
						GoTo(Keys.Right);
					if (mobX < X)
						GoTo(Keys.Left);
				}
				if (Math.Abs(X - mobX) == 0 && Math.Abs(Y - mobY) <= 2)
				{
					if (mobY > Y)
						GoTo(Keys.Down);
					if (mobY < Y)
						GoTo(Keys.Up);
				}
			};

			OnStop += () =>
			{
				if (checkDirection(Keys.Up))
				{
					GoTo(Keys.Up);
					return;
				}
				if (checkDirection(Keys.Down))
				{
					GoTo(Keys.Down);
					return;
				}
				if (checkDirection(Keys.Right))
				{
					GoTo(Keys.Right);
					return;
				}
				if (checkDirection(Keys.Left))
				{
					GoTo(Keys.Left);
					return;
				}
			};
		}

		private bool checkDirection(Keys key)
		{
			var x = X;
			var y = Y;
			for (int i = 0; i < 2; i++)
			{
				Useful.XyPlusKeys(x, y, key, ref x, ref y);
				if (!Model.IsInsideMap(x, y))
					return false;
				if (Model.MobMap[x, y].Count > 0)
					return true;
			}
			return false;
		}

		public override void ForStop() { }
        public override void ForMoveStart()
        {
            var newX = X;
            var newY = Y;
            Useful.XyPlusKeys(newX, newY, GazeDirection, ref newX, ref newY);
            var willDie = Model.MobMap[newX, newY].Where(mob => mob != this && !(mob is SporeMob)).ToArray();
            for (var i = 0; i < willDie.Length; i++)
                willDie[i].Destroy();
        }
        public override bool CanStep(IItem item)
        {
            if (item is ThreeItem)
                return true;
            return base.CanStep(item);
        }
        public override bool StandingAnimation => true;
        public override double Speed => 0.02;
        public override sbyte SlidesCount => 4;
        public override int SlideLatency => 13;
    }
}