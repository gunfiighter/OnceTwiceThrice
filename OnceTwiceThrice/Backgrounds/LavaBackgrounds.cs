using System.Drawing;

namespace OnceTwiceThrice
{
	public class LavaBackground : BackgroundBase, IBackground
	{
		public LavaBackground (GameModel model) : base("Lava", 4)
		{
			model.OnTick += () =>
			{
				if (model.TickCount % 20 == 0)
					this.ChangeSlide();
			};
		}

		public bool CanStep(MovableBase mob) => false;
		public bool CanStop(MovableBase mob) => true;
	}
}