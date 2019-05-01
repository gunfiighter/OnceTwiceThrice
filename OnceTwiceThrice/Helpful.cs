using System.Collections.Generic;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public static class Helpful
	{
		public static ImageModel GetNextHero(IEnumerator<ImageModel> enumerator)
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
	}
}