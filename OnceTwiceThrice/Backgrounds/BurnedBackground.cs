
namespace OnceTwiceThrice
{
	public class BurnedBackground : BackgroundBase, IBackground
	{
		public BurnedBackground(GameModel model, int x, int y) : base(x, y, "Burned", 1)
		{
			;
		}

		public bool CanStep(MovableBase mob) => true;
		public bool CanStop(MovableBase mob) => true;
	}
}