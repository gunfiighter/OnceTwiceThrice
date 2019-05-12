using System.Drawing;

namespace OnceTwiceThrice
{
	public class GrassBackground : BackgroundBase, IBackground 
	{
		public GrassBackground(GameModel model) : base("Grass", 1)
		{
			;
		}

		public bool CanStep(MovableBase mob) => true;
		public bool CanStop(MovableBase mob) => true;
	}
}