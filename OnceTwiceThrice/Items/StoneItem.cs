using System.Drawing;

namespace OnceTwiceThrice
{
	public class StoneItem : ItemBase, IItems
	{
		public StoneItem(int x, int y) : base(x, y, 1, "Stone")
		{
			;
		}

		public bool CanStep(MovableBase mob) => false;
		public bool CanStop(MovableBase mob) => true;
	}
}