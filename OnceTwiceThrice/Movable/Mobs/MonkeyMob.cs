using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
    public class MonkeyMob : MobBase, IMob
    {
        public static string ImagePath = "Monkey/";
        public MonkeyMob(GameModel model, int X, int Y) : base(model, ImagePath, X, Y)
        {
            Model.Map[X, Y].OnMobAdd += TryKill;
            OnDestroy += () => Model.Map[X, Y].OnMobAdd -= TryKill;
        }
        public override bool SkinIgnoreDirection => true;

        public override sbyte SlidesCount => 4;
        public override int SlideLatency => 15;
    }
}