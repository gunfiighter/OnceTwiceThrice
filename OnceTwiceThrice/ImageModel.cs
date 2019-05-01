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
		public Image Image {
			get
			{
				switch (lastDirection)
				{
					case Keys.Up: return goUp[0];
					case Keys.Down: return goDown[0];
					case Keys.Right: return goRight[0];
					case Keys.Left: return goLeft[0];
				}

				return goDown[0];
			}
		}

		private Keys lastDirection;
		
		private List<Image> goUp;
		private List<Image> goDown;
		private List<Image> goRight;
		private List<Image> goLeft;
		
	

		public GameMap map;

		public int X;
		public int Y;

		public double xf;
		public double yf;

		public bool flag;
		private MyForm form;

		public Animation CurrentAnimation;
		private double animationStep = 0.05;

		public ImageModel(GameMap map, string ImageFile, int X, int Y)
		{
			this.form = map.form;
			this.map = map;
			
			goUp = new List<Image>();
			goDown = new List<Image>();
			goRight = new List<Image>();
			goLeft = new List<Image>();
			
			goUp.Add(Image.FromFile("../../images/" + ImageFile + "Up.png"));
			goDown.Add(Image.FromFile("../../images/" + ImageFile + "Down.png"));
			goRight.Add(Image.FromFile("../../images/" + ImageFile + "Right.png"));
			goLeft.Add(Image.FromFile("../../images/" + ImageFile + "Left.png"));
			lastDirection = Keys.Down;
			
			this.X = X;
			this.Y = Y;
			CurrentAnimation = new Animation();
		}
		public void MakeMove(Keys key)
		{
			lastDirection = key;
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
				if (!form.CurrentKeyMap[CurrentAnimation.Direction] || 
				    !map.IsInsideMap(X, Y, CurrentAnimation.Direction))
					CurrentAnimation.IsMoving = false;

				var nextDirection = form.CurrentKeyMap.GetAnyOnDirection();
				if (nextDirection != Keys.None && this == form.CurrentHero)
					MakeMove(nextDirection);
			}
		}
	}
}
