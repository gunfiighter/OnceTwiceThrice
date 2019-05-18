using System;
using System.Linq;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
    public class FrogMob : MobBase, IMob
    {
        public static string ImagePath = "Frog/";

        public FrogMob(GameModel model, int x, int y) : base(model, ImagePath, x, y)
        {
            Action IceToWater = () =>
            {
                if (Model.BackMap[X, Y] is IceBackground)
                    Model.BackMap[X, Y] = new WaterBackground(Model, X, Y);
            };
            OnMoveStart += () =>
            {
                if (Model.BackMap[X, Y] is WaterBackground ||
                    Model.BackMap[MX, MY] is WaterBackground)
                {
                    Destroy();
                    return;
                }

                IceToWater();
            };

            OnCantMove += (key) =>
            {
                IceToWater();

                var newDirection = Useful.ReverseDirection(key);
                GoTo(newDirection);
            };

            OnStop += base.ForStop;
        }

        public override void ForStop()
        {
            foreach (var spell in Model.Spells.Where(spell => spell is SkimletSpell && Math.Abs(spell.X - X) <= 1 && Math.Abs(spell.Y - Y) <= 1))
            {
                var direction = SpellBase.GetOppositeDirection(spell, this);
                if (direction != Keys.None)
                    GoTo(direction);
            }
        }

        public override bool CanStep(IBackground background) => true;
        public override bool CanStep(IItem item)
        {
            if (item is FireItem)
                return true;
            return base.CanStep(item);
        }

        public override double Speed => 0.03;
        public override sbyte SlidesCount => 4;
        public override bool DestroyByMatthiusSpell => false;
        public override bool DestroyBySkimletSpell => true;

    }
}