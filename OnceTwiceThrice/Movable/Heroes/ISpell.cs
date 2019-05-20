using System.Drawing;

namespace OnceTwiceThrice
{
	public interface ISpell
	{
		Image Picture { get; }
        int X { get; }
        int Y { get; }
        bool NeedInvalidate { get; set; }
    }
}
