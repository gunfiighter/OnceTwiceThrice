using System.Windows.Forms;

namespace OnceTwiceThrice
{
    public class SemiConductorItem : ItemBase, IItem
    {
        public Keys Direction;

        public SemiConductorItem(GameModel model, int x, int y, Keys direction, string imageFile) : base(model, x, y, 1, imageFile)
        {
            Direction = direction;
        }
        
        public bool CanStep(MovableBase mob) => !(mob is IHero && Useful.ReverseDirection(mob.GazeDirection) == Direction);

        public bool CanStop(MovableBase mob) => true;
    }
}
