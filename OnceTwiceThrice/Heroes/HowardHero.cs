using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OnceTwiceThrice
{
    public class HowardHero : HeroBase, IHero
    {
		public static string ImagePath = "Howard/";
		public HowardHero(GameModel model, int X, int Y) : base(model, ImagePath, X, Y)
        {
            ;
        }

        public override bool CanStep(IItems item)
        {
            if (item is ThreeItem)
                return true;
            return base.CanStep(item);
        }

        public void CreateSpell()
        {
            base.CreateSpell((x, y) => new HowardSpell(this, x, y, ImagePath));
        }
    }

    public class HowardSpell : SpellBase, ISpell
    {
        public HowardSpell(IHero hero, int X, int Y, string ImageFile) : base(hero, X, Y, ImageFile)
        {
            OnDestroy += () =>
            {
                if (Model.BackMap[X, Y] is GrassBackground)
                {
                    var stack = Model.ItemsMap[X, Y];

                    if (stack.Count == 0)// || stack.Peek() is StoneItem)
						stack.Push(new ThreeItem(Model, X, Y));
                }
            };
        }
    };
}