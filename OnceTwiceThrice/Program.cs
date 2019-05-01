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

	public class GameMap
	{
		public int Width;
		public int Height;
		public GameMap(int width, int height)
		{
			Width = width;
			Height = height;
		}

		public bool IsInsideMap(int x, int y)
		{
			return
				x >= 0 && x < Width &&
				y >= 0 && y < Height;
		}

		public bool IsInsideMap(int x, int y, Keys key)
		{
			var dx = 0;
			var dy = 0;
			switch (key)
			{
				case Keys.Up: dy = -1; break;
				case Keys.Down: dy = 1; break;
				case Keys.Left: dx = -1; break;
				case Keys.Right: dx = 1; break;
				default: throw new ArgumentException();
			}
			var newX = x + dx;
			var newY = y + dy;
			return
				IsInsideMap(newX, newY);
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
			}
		}
	}

	public class KeyMap
	{
		public bool Up;
		public bool Down;
		public bool Left;
		public bool Right;

		public KeyMap()
		{
			Up = Down = Left = Right = false;
		}

		public bool this[Keys key]
		{
			get
			{
				switch (key)
				{
					case Keys.Up: return Up;
					case Keys.Down: return Down;
					case Keys.Left: return Left;
					case Keys.Right: return Right;
				}
				return false;
			}
			private set
			{
				switch (key)
				{
					case Keys.Up: Up = value; break;
					case Keys.Down: Down = value; break;
					case Keys.Left: Left = value; break;
					case Keys.Right: Right = value; break;
				}
			}
		}

		public void TurnOn(Keys key)
		{
			this[key] = true;
		}

		public void TurnOff(Keys key)
		{
			this[key] = false;
		}
	}

	public class MyForm : Form
	{
		public KeyMap CurrentKeyMap;

		public MyForm()
		{
			DoubleBuffered = true;
			CurrentKeyMap = new KeyMap();
			var map = new GameMap(5, 5);
			

			var image = new ImageModel(this, map, "images/rocket.png", 0, 0);
			KeyDown += (sender, args) =>
			{
				var keyCode = args.KeyCode;
				CurrentKeyMap.TurnOn(keyCode);
				image.MakeMove(keyCode);
				Invalidate();
			};

			KeyUp += (sender, args) =>
			{
				CurrentKeyMap.TurnOff(args.KeyCode);
			};
			

			var times = 0;

			Paint += (sender, args) =>
			{
				var g = args.Graphics;
				times++;
				g.DrawImage(image.Image, new Point((int)((image.X + image.xf) * 50), (int)((image.Y + image.yf) * 50)));
			};

			var timer = new Timer();
			timer.Interval = 10;
			timer.Tick += (sender, args) =>
			{
				image.MakeAnimation();
				Invalidate();
			};
			timer.Start();

			Invalidate();
		}
	}

	static class Program
	{
		/// <summary>
		/// Главная точка входа для приложения.
		/// </summary>
		[STAThread]
		static void Main()
		{
			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MyForm());
		}
	}
}
