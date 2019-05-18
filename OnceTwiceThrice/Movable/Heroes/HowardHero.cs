using System;
using System.Collections.Generic;
using System.Linq;
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

        public override bool CanStep(IItem item)
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
            Action<int, int> trySearchThreeTermite = (x, y) =>
            {
                if (!Model.IsInsideMap(x, y))
                    return;
                foreach (var mob in Model.MobMap[x, y].Where(mob => mob is TermiteMob))
                {
                    (mob as TermiteMob).CheckerTurnOn();
                    break;
                }
            };

            Action<int, int> trySearchThreeHotGuy = (x, y) =>
            {
                if (!Model.IsInsideMap(x, y))
                    return;
                foreach (var mob in Model.MobMap[x, y].Where(mob => mob is HotGuyMob))
                {
                    (mob as HotGuyMob).TrySeachThree();
                    break;
                }
            };


            Action CreateThree = () =>
            {
                Model.ItemsMap[X, Y].Push(new ThreeItem(Model, X, Y));

                trySearchThreeHotGuy(X, Y - 1);
                trySearchThreeHotGuy(X, Y + 1);
                trySearchThreeHotGuy(X - 1, Y);
                trySearchThreeHotGuy(X + 1, Y);

                for (var delta = 1; delta <= 2; delta++)
                {
                    trySearchThreeTermite(X, Y - delta);
                    trySearchThreeTermite(X, Y + delta);
                    trySearchThreeTermite(X - delta, Y);
                    trySearchThreeTermite(X + delta, Y);
                }

            };

            OnDestroy += () =>
            {
                if (Model.BackMap[X, Y] is GrassBackground)
                {
                    var stack = Model.ItemsMap[X, Y];

                    if (stack.Count == 0 || 
                        stack.Peek() is SwitcherItem || 
                        stack.Peek() is SemiConductorItem ||
                        stack.Peek() is FlowItem)
                    {
                        var checkMob = false;
                        foreach (var mob in Model.MobMap[X, Y])
                            if (mob.CurrentAnimation.IsMoving)
                            {
                                checkMob = true;
                                break;
                            }
                        if (!checkMob)
                        {
                            if (stack.Count > 0) {
                                var onSwitcher = false;
                                var item = stack.Peek();
                                if (item is SwitcherItem)
                                    onSwitcher = true;
                                CreateThree();
                                if (onSwitcher)
                                    item.Step(Hero);
                            } else
                                CreateThree();
                        }
                    }
                }
            };
        }
    };
}