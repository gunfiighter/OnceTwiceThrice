using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
    public class FlowItem : ItemBase, IItem
    {
        public Keys Direction;
        private IMovable currentMob;
        public FlowItem(GameModel model, int x, int y, Keys direction, string imageFile) : base(model, x, y, 1, imageFile)
        {
            Direction = direction;
            OnStep += (mob) =>
            {
                
                if (mob is IHero)
                {
                    currentMob = mob;
                    if (currentMob.AllowToMove(Direction))
                        mob.MakeMove(Direction);
                    else
                    {
                        mob.KeyMap.Enable = false;
                        Model.OnTick += OnTick;
                    }
                }
            };
        }

        private void OnTick()
        {
            if (currentMob.AllowToMove(Direction))
            {
                currentMob.MakeMove(Direction);
                currentMob.KeyMap.Enable = true;
                Model.OnTick -= OnTick;
            }
        }
        

        public bool CanStep(MovableBase mob) => true;

        public bool CanStop(MovableBase mob) => true;
    }
}
