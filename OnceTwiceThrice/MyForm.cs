using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
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
}
