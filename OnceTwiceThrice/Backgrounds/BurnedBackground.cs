using System.Drawing;

namespace OnceTwiceThrice
{
	public class BurnedBackground : IBackground
	{
		public Image Picture { get; } = Useful.GetImageByName("Burned");

		public bool CanStep(MovableBase mob) => true;
		public bool CanStop(MovableBase mob) => true;
	}
}