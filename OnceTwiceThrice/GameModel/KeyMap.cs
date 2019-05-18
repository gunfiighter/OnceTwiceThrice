using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class KeyMap
	{
		public bool Up;
		public bool Down;
		public bool Left;
		public bool Right;

        public bool Enable;

		public KeyMap()
		{
			Up = Down = Left = Right = false;
            Enable = true;
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
		
		public void TurnOff()
		{
			Up = Down = Right = Left = false;
		}

		public Keys GetAnyOnDirection()
		{
			if (Up) return Keys.Up;
			if (Down) return Keys.Down;
			if (Left) return Keys.Left;
			if (Right) return Keys.Right;
			return Keys.None;
		}
	}
}
