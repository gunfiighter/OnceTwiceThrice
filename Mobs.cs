using System;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public interface IMob : IMovable
	{
		
	}

	public class MobBase : MovableBase
	{
		public MobBase(GameModel model, string ImageFile, int X, int Y) : base(model, ImageFile, X, Y)
		{
			;
			OnMoveStart += MoveStart;
		}

		public virtual void MoveStart()
		{
			foreach (var hero in Model.Heroes)
			{
				if ((hero.MX == MX && hero.MY == MY) || (hero.X == MX && hero.Y == MY))
				{
					hero.Destroy();
				}
			}
		}
		
		
		public override double Speed
		{
			get { return 0.04; }
		}
	}
	
	public class RighterMob : MobBase, IMob
	{
		public RighterMob(GameModel model, int X, int Y): base(model, "Righter", X, Y)
		{
			Action<Keys> TurnDirection = (key) =>
			{
				KeyMap.TurnOn(key);
				MakeMove(key);
			};

			TurnDirection(Keys.Down);
			
			OnCantMove += (key) =>
			{
				KeyMap.TurnOff();
				switch (key)
				{
					case Keys.Up:
						TurnDirection(Keys.Right); break;
					case Keys.Down: 
						TurnDirection(Keys.Left); break;
					case Keys.Right: 
						TurnDirection(Keys.Down); break;
					case Keys.Left: 
						TurnDirection(Keys.Up); break;
				}
			};
		}	
	}
	
	public class SporesMob : MobBase, IMob
	{
		public override bool SkinIgnoreDirection => true; 
		public SporesMob(GameModel model, int X, int Y): base(model, "Spores", X, Y)
		{
			OnCantMove += (key) =>
			{
				this.Destroy();
			};
		}

		public override bool CanStep(IBackground background) => true;
	}
}