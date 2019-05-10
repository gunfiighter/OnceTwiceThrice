using System.Drawing;

namespace OnceTwiceThrice
{
	public class FireItem : ItemBase, IItems
	{
		public FireItem(GameModel model,int x, int y) : base(x, y, 4, "Fire")
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