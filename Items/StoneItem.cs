using System.Drawing;

namespace OnceTwiceThrice
{
	public class StoneItem : ItemBase, IItems
	{
		public Image Picture { get; }
		public StoneItem(int x, int y) : base(x, y)
		{
			Picture = Useful.GetImageByName("Stone");
		}

		public bool CanStep(MovableBase mob) => false;
		public bool CanStop(MovableBase mob) => true;
	}
}