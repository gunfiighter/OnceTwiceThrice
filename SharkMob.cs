using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class SharkMob : MobBase, IMob
	{
		public static string ImagePath = "Shark/";
		public SharkMob(GameModel model, int X, int Y) : base(model, ImagePath, X, Y)
		{
            ;
        }

		public override bool SkinIgnoreDirection => true;

		public override sbyte SlidesCount => 4;
		public override int SlideLatency => 15;
		public override bool DestroyByMatthiusSpell => false;
	}
}