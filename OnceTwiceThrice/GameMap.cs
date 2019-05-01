using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
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
}
