using System.Drawing;

namespace OnceTwiceThrice
{
    public class ThreeItem : ItemBase, IItems
    {
        public ThreeItem(int x, int y) : base(x, y, 1, "Three")
        {
            ;
        }

        public bool CanStep(MovableBase mob) => false;
        public bool CanStop(MovableBase mob) => true;
    }
}