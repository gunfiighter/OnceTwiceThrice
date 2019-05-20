using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class RedGolemMob : MobBase, IMob
	{
        private int countTurn;
        private int currentTick;
		public RedGolemMob(GameModel model, int X, int Y): base(model, "RedGolem/", X, Y)
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