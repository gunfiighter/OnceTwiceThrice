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
            var dict = new Dictionary<IMob, Action>();
            foreach (var mob in Model.Mobs)
            {
                dict.Add(mob, () =>
                {
                    if (mob.MX == X && mob.MY == Y)
                        mob.Destroy();
                });
                mob.OnMoveStart += dict[mob];
            }

            OnDestroy += () =>
            {
                foreach (var act in dict)
                    act.Key.OnMoveStart -= act.Value;
            };
        }

        public override bool SkinIgnoreDirection => true;

        public override sbyte SlidesCount => 4;
        public override int SlideLatency => 15;
    }
}