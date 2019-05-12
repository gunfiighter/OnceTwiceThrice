using System.Drawing;

namespace OnceTwiceThrice
{
	public class FireItem : ItemBase, IItems
	{
		public FireItem(GameModel model,int x, int y) : base(model, x, y, 4, "Fire")
		{
            if (Model.BackMap[X, Y] is GrassBackground)
                Model.BackMap[X, Y] = new BurnedBackground(Model, X, Y);
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