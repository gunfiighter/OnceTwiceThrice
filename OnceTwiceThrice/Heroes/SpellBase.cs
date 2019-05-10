using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnceTwiceThrice
{
	public class SpellBase
	{
		public Image Picture { get; }
		public int X { get; }
		public int Y { get; }
		public GameModel model;

		public SpellBase(GameModel model, int X, int Y, string ImageFile)
		{
			this.model = model;
			this.X = X;
			this.Y = Y;
			Picture = Useful.GetImageByName(ImageFile);
		}
	}
}
