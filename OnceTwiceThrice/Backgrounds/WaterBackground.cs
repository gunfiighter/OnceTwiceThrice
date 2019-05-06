using System.Drawing;

namespace OnceTwiceThrice
{
	public class WaterBackground : IBackground
	{
		public Image Picture { get; } = Helpful.GetImageByName("Water");

		public bool CanStep(MovableBase mob) => false;
		public bool CanStop(MovableBase mob) => true;
	}
}