namespace OnceTwiceThrice
{
	public class MatthiusHero : MovableBase
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
	}
	
	public class SkimletHero : MovableBase
	{
		public SkimletHero(GameModel model, int X, int Y): base(model, "Skimlet", X, Y)
		{
			;
		}
	}
}