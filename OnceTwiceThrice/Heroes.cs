using System.Windows.Forms;
using System.Xml.Linq;

namespace OnceTwiceThrice
{
	public interface IHero : IMovable
	{
		
	}
	public class HeroBase : MovableBase
	{
		public HeroBase(GameModel model, string ImageFile, int X, int Y) : base(model, ImageFile, X, Y)
		{
			OnDestroy += () => {
				model.GameOver(this);
			};
			OnStop += () =>
			{
				var itemsStack = model.ItemsMap[this.X, this.Y];
				if (itemsStack.Count > 0 && itemsStack.Peek() is DestinationItem)
					model.Win();
			};
			OnMoveStart += MoveStart;
		}
		
		public virtual void MoveStart()
		{
			foreach (var mob in Model.Mobs)
			{
				if ((mob.MX == MX && mob.MY == MY) || (mob.X == MX && mob.Y == MY))
				{
					this.Destroy();
				}
			}
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

		public override bool CanStep(IBackground back)
		{
			if (back is LavaBackground)
				return true;
			return base.CanStep(back);
		}
	}
	
	public class SkimletHero : HeroBase, IHero
	{
		public SkimletHero(GameModel model, int X, int Y): base(model, "Skimlet", X, Y)
		{
			;
		}
		
		public override bool CanStep(IBackground back)
		{
			if (back is WaterBackground)
				return true;
			return base.CanStep(back);
		}
	}
}