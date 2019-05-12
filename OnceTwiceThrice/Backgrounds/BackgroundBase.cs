using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnceTwiceThrice
{
	public class BackgroundBase
	{
		public Image Picture { get; private set; }
		protected int animationCounter { get; set; }
		private readonly Image[] slides;
		private readonly int slidesCount;

		public BackgroundBase(string BackgroundName, int SlidesCount)
		{
			slides = new Image[SlidesCount];
			for (int i = 0; i < SlidesCount; i++)
				slides[i] = Useful.GetImageByName(BackgroundName + "/" + i);
			animationCounter = 0;
			Picture = slides[animationCounter];
			slidesCount = SlidesCount;
		}

		protected void ChangeSlide()
		{
			animationCounter++;
			if (animationCounter == slidesCount - 1)
				animationCounter = 0;
			Picture = slides[animationCounter];
		}
	}
}
