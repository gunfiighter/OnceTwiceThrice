using System.Drawing;

namespace OnceTwiceThrice
{
    public class IceBackground : BackgroundBase, IBackground
    {
        public IceBackground(GameModel model, int x, int y) : base(x, y, "Ice", 1)
        {
            OnStep += (mob) =>
            {
                if (mob.IceSlip)
                {
                    mob.MakeMove(mob.CurrentAnimation.Direction);
                }
            };
        }

        public bool CanStep(MovableBase mob) => true;
        public bool CanStop(MovableBase mob) => true;
    }
}