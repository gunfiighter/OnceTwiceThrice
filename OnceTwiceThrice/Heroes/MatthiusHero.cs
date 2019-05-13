using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class MatthiusHero : HeroBase, IHero
	{
		public static string ImagePath = "Matthius/";
		public MatthiusHero(GameModel model, int X, int Y): base(model, ImagePath, X, Y)
		{
			;
		}

		public override bool CanStep(IItem item)
		{
			if (item is FireItem)
				return true;
			return base.CanStep(item);
		}

		public override bool CanStep(IBackground back)
		{
			if (back is LavaBackground)
				return true;
			return base.CanStep(back);
		}

		public void CreateSpell()
		{
            base.CreateSpell((x, y) => new MatthiusSpell(this, x, y, ImagePath));
		}
	}

	public class MatthiusSpell : SpellBase, ISpell
	{
		public MatthiusSpell(IHero hero, int X, int Y, string imageFile) : base(hero, X, Y, imageFile)
		{
            var dict = new Dictionary<IMob, Action>();
            var willDie = new List<IMob>();
			foreach (var mob in Model.Mobs.Where(mob => mob.DestroyByMatthiusSpell))
			{
                if (Useful.CheckTouch(mob, X, Y))
                {
                    willDie.Add(mob);
                    continue;
                }
				dict.Add(mob, () =>
				{
					if (mob.MX == X && mob.MY == Y)
						mob.Destroy();
				});
				mob.OnMoveStart += dict[mob];
			}

            foreach (var mob in willDie)
                mob.Destroy();

            foreach (var mob in Model.Mobs.Where(mob => Math.Abs(mob.X - X) <= 1 && Math.Abs(mob.Y - Y) <= 1))
            {
                if (mob is PenguinMob && !mob.CurrentAnimation.IsMoving)
                {
                    var direction = GetOppositeDirection(this, mob);
                    if (direction != Keys.None)
                        mob.GoTo(direction);
                    break;
                }
            }

            OnDestroy += () =>
			{
                var ItemStack = Model.ItemsMap[X, Y];

                if (ItemStack.Count > 0 && ItemStack.Peek() is ThreeItem)
                    Model.ItemsMap[X, Y].Peek().Destroy();

                foreach (var act in dict)
					act.Key.OnMoveStart -= act.Value;
			};
		}
	};
}