using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public interface IHero : IMovable
	{
		
	}

	public class HeroBase : MovableBase
	{
		public HeroBase(GameModel model, string ImageFile, int X, int Y) : base(model, ImageFile, X, Y)
		{
			;
		}

		public override double Speed
		{
			get { return 0.05; }
		}
	}
	
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
	}
	
	public class SkimletHero : HeroBase, IHero
	{
		public SkimletHero(GameModel model, int X, int Y): base(model, "Skimlet", X, Y)
		{
			;
		}
	}
}