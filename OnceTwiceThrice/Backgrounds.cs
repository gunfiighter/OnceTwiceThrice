using System.Drawing;

namespace OnceTwiceThrice
{
	public interface IBackground
	{
		Image Picture { get; }
		bool CanStep(MovableBase mob);
		bool CanStop(MovableBase mob);
	}

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
	
	public class BurnedBackground : IBackground
	{
		private Image picture = Helpful.GetImageByName("Burned");
		public Image Picture
		{
			get { return picture; }
		}
		public bool CanStep(MovableBase mob) => true;
		public bool CanStop(MovableBase mob) => true;
	}
}