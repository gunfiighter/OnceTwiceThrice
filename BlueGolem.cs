using System;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class BlueGolemMob : MobBase, IMob
	{
        private int countTurn;
        private int currentTick;
        public BlueGolemMob(GameModel model, int X, int Y) : base(model, "BlueGolem/", X, Y)
		{
			OnCantMove += (key) =>
			{
                if (currentTick == Model.TickCount)
                {
                    if (countTurn == 4)
                    {
                        Destroy();
                        return;
                    }
                    countTurn++;
                }
                else
                    countTurn = 1;

                KeyMap.TurnOff();
                currentTick = Model.TickCount;
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