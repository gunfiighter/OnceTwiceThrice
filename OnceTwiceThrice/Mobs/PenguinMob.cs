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
            OnMoveStart += () =>
            {
                if (Model.BackMap[X, Y] is LavaBackground ||
                    Model.BackMap[MX, MY] is LavaBackground)
                {
                    Destroy();
                    return;
                }

                if (Model.BackMap[X, Y] is WaterBackground)
                    Model.BackMap[X, Y] = new IceBackground(Model, X, Y);
            };

            OnCantMove += (key) =>
            {
                var newDirection = key;
                switch (key)
                {
                    case Keys.Up: newDirection = Keys.Down; break;
                    case Keys.Down: newDirection = Keys.Up; break;
                    case Keys.Right: newDirection = Keys.Left; break;
                    case Keys.Left: newDirection = Keys.Right; break;
                }
                GoTo(newDirection);
            };

            OnStop += base.ForStop;
        }
        public override bool CanStep(IBackground background) => true;

        public override void ForStop()
        {
            foreach (var spell in Model.Spells.Where(spell => spell is MatthiusSpell && Math.Abs(spell.X - X) <= 1 && Math.Abs(spell.Y - Y) <= 1))
            {
                SpellBase.MoveInOppositeDirection(spell, this);
            }
        }

        public override double Speed => 0.03;
        public override sbyte SlidesCount => 4;

    }
}