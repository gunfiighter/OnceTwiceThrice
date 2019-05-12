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
        private Image _picture;
		public Image Picture
        {
            get => _picture;
            private set
            {
                NeedInvalidate = true;
                _picture = value;
            }
        }
        public int X { get; }
        public int Y { get; }

		protected int animationCounter { get; set; }
		private readonly Image[] slides;
		private readonly int slidesCount;
        public bool NeedInvalidate { get; set; }

		public BackgroundBase(int x, int y, string BackgroundName, int SlidesCount)
		{
            X = x;
            Y = y;
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
