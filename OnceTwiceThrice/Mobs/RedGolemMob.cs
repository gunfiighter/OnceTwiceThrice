using System;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class RedGolemMob : MobBase, IMob
	{
		public RedGolemMob(GameModel model, int X, int Y): base(model, "RedGolem/", X, Y)
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