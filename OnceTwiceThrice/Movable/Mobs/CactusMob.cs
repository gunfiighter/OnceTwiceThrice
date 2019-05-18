using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnceTwiceThrice
{
    public class CactusMob : MobBase, IMob
    {
        public CactusMob(GameModel model, int x, int y) : base(model, "Cactus/", x, y)
        {
            KeyMap.Enable = false;
            Model.Map[X, Y].OnMobAdd += TryKill;
            OnMoveStart += () =>
            {
                Model.Map[X, Y].OnMobAdd -= TryKill;
            };
            OnDestroy += () => Model.Map[X, Y].OnMobAdd -= TryKill;
        }

        public override bool SkinIgnoreDirection => true;
        public override sbyte SlidesCount => 4;
        public override int SlideLatency => 10;
        public override double Speed => 0.02;
    }
}
