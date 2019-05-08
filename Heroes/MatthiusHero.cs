namespace OnceTwiceThrice
{
	public class MatthiusHero : HeroBase, IHero
	{
		public MatthiusHero(GameModel model, int X, int Y): base(model, "Matthius", X, Y)
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
	}
}