using System.Drawing;

namespace OnceTwiceThrice
{
	public class ItemBase
	{
		public int X { get; }
		public int Y { get; }
		public Image Picture { get; private set; }
		protected int animationCounter { get; set; }
		private sbyte animationMove = 1;
		private readonly Image[] slides;
		private readonly int slidesCount;

		public ItemBase(int x, int y, int SlidesCount, string mobName)
		{
			this.X = x;
			this.Y = y;
			slides = new Image[SlidesCount];
			for (int i = 0; i < SlidesCount; i++)
				slides[i] = Useful.GetImageByName(mobName + "/" + i);
			animationMove = 1;
			animationCounter = 0;
			Picture = slides[animationCounter];
			slidesCount = SlidesCount;
		}

		protected void ChangeSlide()
		{
			animationCounter += animationMove;
			if (animationCounter == slidesCount - 1)
				animationMove = -1;
			if (animationCounter == 0)
				animationMove = 1;			
			Picture = slides[animationCounter];
		}
	}
}