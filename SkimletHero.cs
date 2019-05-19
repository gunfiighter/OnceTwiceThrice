using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OnceTwiceThrice
{
	public class SkimletHero : HeroBase, IHero
	{
		public static string ImagePath = "Skimlet/";
		public SkimletHero(GameModel model, int X, int Y): base(model, ImagePath, X, Y)
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
            base.CreateSpell((x, y) => new SkimletSpell(this, x, y, ImagePath));
        }

        public override bool IceSlip => false;
    }

    public class SkimletSpell : SpellBase, ISpell
    {
        public SkimletSpell(IHero hero, int X, int Y, string ImageFile) : base(hero, X, Y, ImageFile)
        {
            var dict = new Dictionary<IMob, Action>();
            var willDie = new List<IMob>();
            foreach (var mob in 
                Model.Mobs
                .Where(mob => mob.DestroyBySkimletSpell))
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
                if (mob is FrogMob && !mob.CurrentAnimation.IsMoving)
                {
                    var direction = GetOppositeDirection(this, mob);
                    if (direction != Keys.None)
                        mob.GoTo(direction);
                    break;
                }
                if (mob is CactusMob && !mob.CurrentAnimation.IsMoving)
                {
                    var direction = GetOppositeDirection(this, mob);
                    if (direction != Keys.None)
                        mob.GoTo(Useful.ReverseDirection(direction));
                        break;
                }
            }
            OnDestroy += () =>
            {
                var ItemStack = Model.Map[X, Y].Items;

                if (ItemStack.Count > 0 && ItemStack.Peek() is FireItem)
                    Model.Map[X, Y].Items.Peek().Destroy();

                foreach (var act in dict)
                    act.Key.OnMoveStart -= act.Value;
            };
        }

        private void RuleForCactusMob(IMob mob)
        {
            throw new NotImplementedException();
        }
    };
}