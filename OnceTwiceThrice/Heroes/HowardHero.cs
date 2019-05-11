using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OnceTwiceThrice
{
    public class HowardHero : HeroBase, IHero
    {
        public HowardHero(GameModel model, int X, int Y) : base(model, "Howard/", X, Y)
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
            base.CreateSpell((x, y) => new HowardSpell(this, x, y, "Howard/Spell/6"));
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

                    if (stack.Count == 0 || stack.Peek() is StoneItem)
                        Model.ItemsMap[X, Y].Push(new ThreeItem(X, Y));
                }
            };
        }
    };
}