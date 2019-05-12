using System.Drawing;

namespace OnceTwiceThrice
{
	public class BurnedBackground : BackgroundBase, IBackground
	{
		public BurnedBackground(GameModel model) : base("Burned", 1)
		{
			;
		}

		public bool CanStep(MovableBase mob) => true;
		public bool CanStop(MovableBase mob) => true;
	}
}