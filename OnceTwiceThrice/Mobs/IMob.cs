using System;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public interface IMob : IMovable
	{
        void GoTo(Keys key);
	}
}