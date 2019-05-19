using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnceTwiceThrice
{
	public class Death
	{
		public Image _picture;
		public Image Picture
		{
			get => _picture;
			set
			{
				NeedInvalidate = true;
				_picture = value;
			}
		}
		private Image[] slides;
		public GameModel Model;

		public readonly int X;
		public readonly int Y;
		private int startTick;
		private int slideCounter;
		private const int interval = 8;

		public bool NeedInvalidate { get; set; }


		public Death(GameModel model, int x, int y)
		{
			Model = model;
			X = x;
			Y = y;
			slides = new Image[10];
			for (int i = 0; i < 10; i++)
				slides[i] = Useful.GetImageByName("Death/" + i);

			startTick = Model.TickCount;
			Model.OnTick += onTick;
			onTick();
		}

		private void onTick()
		{
			if ((Model.TickCount - startTick) % interval == 0)
				ChangeSlide();
			if ((Model.TickCount - startTick) >= interval * 10)
			{
				Model.OnTick -= onTick;
				Model.Deaths.Remove(this);
			}
		}

		private void ChangeSlide()
		{
			Picture = slides[slideCounter];
			slideCounter = (slideCounter + 1) % 10;
		}
	}
}
