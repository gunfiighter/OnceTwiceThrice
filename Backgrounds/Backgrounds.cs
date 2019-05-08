using System.Drawing;

namespace OnceTwiceThrice
{
	public class LavaBackground : IBackground
	{
		private Image picture = Useful.GetImageByName("Lava");
		public Image Picture
		{
			get { return picture; }
		}
		public bool CanStep(MovableBase mob) => false;
		public bool CanStop(MovableBase mob) => true;
	}
}