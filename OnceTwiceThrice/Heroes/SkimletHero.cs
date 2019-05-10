using System.Windows.Forms;
using System.Xml.Linq;

namespace OnceTwiceThrice
{
	public class SkimletHero : HeroBase, IHero
	{
		public SkimletHero(GameModel model, int X, int Y): base(model, "Skimlet/", X, Y)
		{
			;
		}
		
		public override bool CanStep(IBackground back)
		{
			if (back is WaterBackground)
				return true;
			return base.CanStep(back);
		}
	}
}