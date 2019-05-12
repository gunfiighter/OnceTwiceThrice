using System.Drawing;

namespace OnceTwiceThrice
{
	public class WaterBackground : BackgroundBase, IBackground
	{
		public WaterBackground(GameModel model) : base("Water", 4)
		{
			model.OnTick += () =>
			{
				if (model.TickCount % 15 == 0)
					this.ChangeSlide();
			};
		}

		public bool CanStep(MovableBase mob) => false;
		public bool CanStop(MovableBase mob) => true;
	}
}