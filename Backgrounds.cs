using System.Drawing;

namespace OnceTwiceThrice
{
	public interface IBackground
	{
		Image Picture { get; }
		bool CanStep(MovableBase mob); //Можно ли наступить на объект
		bool CanStop(MovableBase mob); //Блокирует ли фон команды на движения мобов, находящемся на нем
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
	
	public class WaterBackground : IBackground
	{
		private Image picture = Helpful.GetImageByName("Water");
		public Image Picture
		{
			get { return picture; }
		}
		public bool CanStep(MovableBase mob) => false;
		public bool CanStop(MovableBase mob) => true;
	}
	
	public class LavaBackground : IBackground
	{
		private Image picture = Helpful.GetImageByName("Lava");
		public Image Picture
		{
			get { return picture; }
		}
		public bool CanStep(MovableBase mob) => false;
		public bool CanStop(MovableBase mob) => true;
	}
}