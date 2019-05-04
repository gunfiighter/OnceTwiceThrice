using System.Drawing;

namespace OnceTwiceThrice
{
	public interface IItems
	{
		Image Picture { get; }
		bool CanStep(MovableBase mob);
		bool CanStop(MovableBase mob);
	}
	
	public class StoneItem : IItems
	{
		public Image Picture { get; }
		public bool Enable { get; private set; }

		public StoneItem()
		{
			Picture = Helpful.GetImageByName("Stone");
			Enable = true;
		}

		public bool CanStep(MovableBase mob) => false;
		public bool CanStop(MovableBase mob) => true;

		public void TurnOn() => Enable = true;
		public void TurnOff() => Enable = false;
	}
	
	public class FireItem : IItems
	{
		public Image Picture { get; }
		public bool Enable { get; private set; }

		public FireItem()
		{
			Picture = Helpful.GetImageByName("Fire");
			Enable = true;
		}

		public bool CanStep(MovableBase mob) => false;
		public bool CanStop(MovableBase mob) => true;

		public void TurnOn() => Enable = true;
		public void TurnOff() => Enable = false;
	}
}