using System.Drawing;

namespace OnceTwiceThrice
{
	public class DestinationItem : ItemBase, IItems
	{
		public Image Picture { get; }
		public DestinationItem(int x, int y) : base(x, y)
		{
			Picture = Useful.GetImageByName("Destination_4");
		}

		public bool CanStep(MovableBase mob) => true;
		public bool CanStop(MovableBase mob) => true;
	}
}