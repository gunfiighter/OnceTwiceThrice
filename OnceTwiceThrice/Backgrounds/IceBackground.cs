using System.Drawing;

namespace OnceTwiceThrice
{
    public class IceBackground : BackgroundBase, IBackground
    {
        public IceBackground(GameModel model, int x, int y) : base(x, y, "Ice", 1)
        {
            ;
        }

        public bool CanStep(MovableBase mob) => true;
        public bool CanStop(MovableBase mob) => true;
    }
}