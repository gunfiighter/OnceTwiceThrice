using System.Drawing;

namespace OnceTwiceThrice
{
	public class StoneItem : ItemBase, IItem
	{
		public StoneItem(GameModel model, int x, int y) : base(model, x, y, 1, "Stone")
		{
            ;
		}

		public bool CanStep(MovableBase mob) => false;
		public bool CanStop(MovableBase mob) => true;
	}
}