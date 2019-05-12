using System;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class BlueGolemMob : MobBase, IMob
	{
		public BlueGolemMob(GameModel model, int X, int Y) : base(model, "BlueGolem/", X, Y)
		{
			OnCantMove += (key) =>
			{
				KeyMap.TurnOff();
				switch (key)
				{
					case Keys.Up:
						GoTo(Keys.Left); break;
					case Keys.Down:
						GoTo(Keys.Right); break;
					case Keys.Right:
						GoTo(Keys.Up); break;
					case Keys.Left:
						GoTo(Keys.Down); break;
				}
			};

			GoTo(Keys.Down);
		}

		public override sbyte SlidesCount => 4;
		public override int SlideLatency => 13;
	}
}