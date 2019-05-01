using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class Animation
	{
		public bool IsMoving { get; set; }
		public Keys Direction { get; set; }
		public Animation()
		{
			IsMoving = false;
			Direction = Keys.None;
		}
	}

	public class ImageModel
	{
		public Image Image;
		public GameMap map;

		public int X;
		public int Y;

		public double xf;
		public double yf;

		public bool flag;
		private MyForm form;

		public Animation CurrentAnimation;
		private double animationStep = 0.05;

		public ImageModel(MyForm form, GameMap map, string ImageFile, int X, int Y)
		{
			this.form = form;
			this.map = map;
			Image = Image.FromFile("../../" + ImageFile);
			this.X = X;
			this.Y = Y;
			CurrentAnimation = new Animation();
		}

		public void MakeMove(Keys key)
		{
			if (CurrentAnimation.IsMoving)
				return;
			xf = yf = 0;

			if (map.IsInsideMap(X, Y, key))
			{
				CurrentAnimation.IsMoving = true;
				CurrentAnimation.Direction = key;
			}
		}

		public void MakeAnimation()
		{
			if (!CurrentAnimation.IsMoving)
				return;
			switch (CurrentAnimation.Direction)
			{
				case Keys.Up: yf += -animationStep; break;
				case Keys.Down: yf += animationStep; break;
				case Keys.Left: xf += -animationStep; break;
				case Keys.Right: xf += animationStep; break;
			}
			if (Math.Abs(xf) - 1 > -0.0001 || Math.Abs(yf) - 1 > -0.0001)
			{
				X = (int)Math.Round(X + xf, 0);
				Y = (int)Math.Round(Y + yf, 0);
				xf = yf = 0;
				if (!form.CurrentKeyMap[CurrentAnimation.Direction] || !map.IsInsideMap(X, Y, CurrentAnimation.Direction))
					CurrentAnimation.IsMoving = false;

				var nextDirection = form.CurrentKeyMap.GetAnyOnDirection();
				if (nextDirection != Keys.None)
					MakeMove(nextDirection);
			}
		}
	}
}
