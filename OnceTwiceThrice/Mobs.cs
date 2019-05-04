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
}