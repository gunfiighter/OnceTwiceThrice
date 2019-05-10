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

		public void CreateSpell()
		{
			var newX = 0;
			var newY = 0;
			Useful.XyPlusKeys(X, Y, this.CurrentAnimation.Direction, ref newX, ref newY);
			Model.Spells.AddLast(new MatthiusSpell(Model, newX, newY, "Matthius/Spell/5"));
		}
	}
}