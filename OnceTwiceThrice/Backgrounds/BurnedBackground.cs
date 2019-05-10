using System.Drawing;

namespace OnceTwiceThrice
{
	public class BurnedBackground : BackgroundBase, IBackground
	{
		public BurnedBackground(GameModel model) : base(1, "Burned")
		{
			;
		}

		public bool CanStep(MovableBase mob) => true;
		public bool CanStop(MovableBase mob) => true;
	}
}