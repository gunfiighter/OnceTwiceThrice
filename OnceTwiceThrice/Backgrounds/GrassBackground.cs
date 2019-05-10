using System.Drawing;

namespace OnceTwiceThrice
{
	public class GrassBackground : BackgroundBase, IBackground 
	{
		public GrassBackground(GameModel model) : base(1, "Grass")
		{
			;
		}

		public bool CanStep(MovableBase mob) => true;
		public bool CanStop(MovableBase mob) => true;
	}
}