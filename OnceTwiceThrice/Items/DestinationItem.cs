using System.Drawing;

namespace OnceTwiceThrice
{
	public class DestinationItem : ItemBase, IItems
	{
		public DestinationItem(GameModel model, int x, int y) : base(x, y, 4, "Destination")
		{
			model.OnTick += () =>
			{
				if (model.TickCount % 18 == 0)
					this.ChangeSlide();
			};
		}

		public bool CanStep(MovableBase mob) => true;
		public bool CanStop(MovableBase mob) => true;
	}
}