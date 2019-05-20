
namespace OnceTwiceThrice
{
    public class CactusMob : MobBase, IMob
    {
        public CactusMob(GameModel model, int x, int y) : base(model, "Cactus/", x, y)
        {
            KeyMap.Enable = false;
        }

        public override bool SkinIgnoreDirection => true;
        public override sbyte SlidesCount => 4;
        public override int SlideLatency => 10;
        public override double Speed => 0.02;
    }
}
