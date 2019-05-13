using System.Drawing;

namespace OnceTwiceThrice
{
    public class ThreeItem : ItemBase, IItem
    {
        public ThreeItem(GameModel model, int x, int y) : base(model, x, y, 1, "Three")
        {
            ;
        }

        public bool CanStep(MovableBase mob) => false;
        public bool CanStop(MovableBase mob) => true;
    }
}