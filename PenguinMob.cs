using System;
using System.Linq;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
    public class PenguinMob : MobBase, IMob
    {
        public static string ImagePath = "Penguin/";

        public PenguinMob(GameModel model, int x, int y) : base(model, ImagePath, x, y)
        {
            Action WaterToIce = () =>
            {
                if (Model.Map[X, Y].Back is WaterBackground)
                    Model.Map[X, Y].Back = new IceBackground(Model, X, Y);
            };
            OnMoveStart += () =>
            {
                if (Model.Map[X, Y].Back is LavaBackground ||
                    Model.Map[MX, MY].Back is LavaBackground)
                {
                    Destroy();
                    return;
                }

                WaterToIce();
            };

            OnCantMove += (key) =>
            {
                WaterToIce();

                var newDirection = Useful.ReverseDirection(key);
                GoTo(newDirection);
            };
        }
        public override bool CanStep(IBackground background) => true;

        public override void ForStop()
        {
            foreach (var spell in Model.Spells.Where(spell => spell is MatthiusSpell && Math.Abs(spell.X - X) <= 1 && Math.Abs(spell.Y - Y) <= 1))
            {
                var direction = SpellBase.GetOppositeDirection(spell, this);
                if (direction != Keys.None)
                    GoTo(direction);
            }
            base.ForStop();
        }

        public override double Speed => 0.03;
        public override sbyte SlidesCount => 4;

    }
}