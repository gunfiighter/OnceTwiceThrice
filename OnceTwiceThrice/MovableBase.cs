using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

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

	public class MovableBase
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
		
	

		public GameModel Model;

		public int X;
		public int Y;

		public double xf;
		public double yf;

		public bool flag;

		public Animation CurrentAnimation;

		public virtual double Speed
		{
			get { return 0.05; }
		}

		public MovableBase(GameModel model, string ImageFile, int X, int Y)
		{
			this.Model = model;
			
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
			if (!CanStep())
				return;
			lastDirection = key;
			if (CurrentAnimation.IsMoving)
				return;

			if (Model.IsInsideMap(X, Y, key))
			{
				xf = yf = 0;
				switch (key)
				{
					case Keys.Up: Y--; yf = 1; break;
					case Keys.Down: Y++; yf = -1; break;
					case Keys.Left: X--; xf = 1; break;
					case Keys.Right: X++; xf = -1; break;
				}
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
				case Keys.Up: yf += -Speed; break;
				case Keys.Down: yf += Speed; break;
				case Keys.Left: xf += -Speed; break;
				case Keys.Right: xf += Speed; break;
			}
			if (Math.Abs(xf) < 0.01 && Math.Abs(yf) < 0.01)
			{
//				X = (int)Math.Round(X + xf, 0);
//				Y = (int)Math.Round(Y + yf, 0);
				xf = yf = 0;
				CurrentAnimation.IsMoving = false;
				if (Model.KeyMap[CurrentAnimation.Direction] &&
				    Model.IsInsideMap(X, Y, CurrentAnimation.Direction) &&
				    this == Model.CurrentHero)
				{
					MakeMove(CurrentAnimation.Direction);
					return;
				}

				var nextDirection = Model.KeyMap.GetAnyOnDirection();
				if (nextDirection != Keys.None)
					MakeMove(nextDirection);
			}
		}

		//public virtual bool IsHero() => false;

		public virtual bool CanStep() => false;
	}
}
