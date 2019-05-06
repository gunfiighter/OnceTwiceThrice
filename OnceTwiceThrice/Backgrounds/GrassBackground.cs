using System.Drawing;

namespace OnceTwiceThrice
{
	public class GrassBackground : IBackground
	{
		private Image picture = Helpful.GetImageByName("Grass");
		public Image Picture
		{
			get { return picture; }
		}

		public bool CanStep(MovableBase mob) => true;
		public bool CanStop(MovableBase mob) => true;
	}
}