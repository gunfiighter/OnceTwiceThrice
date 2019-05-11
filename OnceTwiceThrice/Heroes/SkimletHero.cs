using System;
using System.Collections.Generic;
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
            base.CreateSpell((x, y) => new SkimletSpell(this, x, y, "Skimlet/Spell/3"));
        }
    }

    public class SkimletSpell : SpellBase, ISpell
    {
        public SkimletSpell(IHero hero, int X, int Y, string ImageFile) : base(hero, X, Y, ImageFile)
        {
            var ItemStack = Model.ItemsMap[X, Y];

            if (ItemStack.Count > 0 && ItemStack.Peek() is FireItem)
                Model.ItemsMap[X, Y].Pop();
        }
    };
}