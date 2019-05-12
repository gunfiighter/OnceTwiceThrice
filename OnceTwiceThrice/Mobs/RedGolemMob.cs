using System;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class RedGolemMob : MobBase, IMob
	{
		public RedGolemMob(GameModel model, int X, int Y): base(model, "RedGolem/", X, Y)
		{
			OnCantMove += (key) =>
			{
				KeyMap.TurnOff();
				switch (key)
				{
					case Keys.Up:
                        GoTo(Keys.Right); break;
					case Keys.Down:
                        GoTo(Keys.Left); break;
					case Keys.Right:
                        GoTo(Keys.Down); break;
					case Keys.Left:
                        GoTo(Keys.Up); break;
				}
			};

            GoTo(Keys.Down);
        }

		public override sbyte SlidesCount => 4;
		public override int SlideLatency => 13;
	}
}