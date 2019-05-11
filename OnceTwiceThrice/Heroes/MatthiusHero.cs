using System;
using System.Collections.Generic;

namespace OnceTwiceThrice
{
	public class MatthiusHero : HeroBase, IHero
	{
		public static string ImagePath = "Matthius/";
		public MatthiusHero(GameModel model, int X, int Y): base(model, ImagePath, X, Y)
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
            base.CreateSpell((x, y) => new MatthiusSpell(this, x, y, ImagePath));
		}
	}

	public class MatthiusSpell : SpellBase, ISpell
	{
		public MatthiusSpell(IHero hero, int X, int Y, string imageFile) : base(hero, X, Y, imageFile)
		{
            var dict = new Dictionary<IMob, Action>();
			foreach (var mob in Model.Mobs)
                if (mob.Explodable)
			    {
				    dict.Add(mob, () =>
				    {
					    if ((mob.MX == X && mob.MY == Y) || (mob.X == X && mob.Y == Y))
						    mob.Destroy();
				    });
				    mob.OnMoveStart += dict[mob];
			    }

            var ItemStack = Model.ItemsMap[X, Y];

            if (ItemStack.Count > 0 && ItemStack.Peek() is ThreeItem)
                Model.ItemsMap[X, Y].Pop();

			OnDestroy += () =>
			{
				foreach (var act in dict)
					act.Key.OnMoveStart -= act.Value;
			};
		}
	};
}