using System.Drawing;

namespace OnceTwiceThrice
{
	public class StoneItem : ItemBase, IItems
	{
		public StoneItem(GameModel model, int x, int y) : base(model, x, y, 1, "Stone")
		{
			if (Model.BackMap[X, Y] is GrassBackground)
                Model.BackMap[X, Y] = new BurnedBackground(Model);
		}

		public bool CanStep(MovableBase mob) => false;
		public bool CanStop(MovableBase mob) => true;
	}
}