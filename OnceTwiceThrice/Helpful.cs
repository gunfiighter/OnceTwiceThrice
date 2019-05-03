using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public static class Helpful
	{
		public static MovableBase GetNextHero(IEnumerator<MovableBase> enumerator)
		{
			if (enumerator.MoveNext())
				return enumerator.Current;
			enumerator.Reset();
			return GetNextHero(enumerator);
		}
		
		public static bool KeyIsMove(Keys key)
		{
			return
				key == Keys.Up ||
				key == Keys.Down ||
				key == Keys.Left ||
				key == Keys.Right;

		}

		public static Image GetImageByName(string name)
		{
			return Image.FromFile("../../images/" + name + ".png");
		}
	}
}