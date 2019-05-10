using System;
using System.Collections.Generic;

namespace OnceTwiceThrice
{
	public class MatthiusHero : HeroBase, IHero
	{
		public MatthiusHero(GameModel model, int X, int Y): base(model, "Matthius/", X, Y)
		{
			;
		}

		public override bool CanStep(IItems item)
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
			var newX = 0;
			var newY = 0;
			Useful.XyPlusKeys(X, Y, this.CurrentAnimation.Direction, ref newX, ref newY);
			Model.Spells.AddLast(new MatthiusSpell(Model, newX, newY, "Matthius/Spell/3"));
		}
	}

	public class MatthiusSpell : SpellBase, ISpell
	{
		private int startTick;
		private Dictionary<IMob, Action> dict;

		public MatthiusSpell(GameModel model, int X, int Y, string ImageFile) : base(model, X, Y, ImageFile)
		{
			dict = new Dictionary<IMob, Action>();
			foreach (var mob in model.Mobs)
			{
				dict.Add(mob, () =>
				{
					if ((mob.MX == X && mob.MY == Y) || (mob.X == X && mob.Y == Y))
						mob.Destroy();
				});
				mob.OnMoveStart += dict[mob];
			}
			startTick = model.TickCount;

			model.OnTick += onTick;
		}

		private void onTick()
		{
			if (model.TickCount - startTick >= 200)
			{
				foreach (var act in dict)
					act.Key.OnMoveStart -= act.Value;
				model.OnTick -= onTick;
				model.Spells.Remove(this);
			}
		}
	};
}