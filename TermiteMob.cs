using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
    public class TermiteMob : MobBase, IMob
    {
        private int startTime;
        private bool _tickEnable;
        public bool TickEnable
        {
            get => _tickEnable;
            set
            {
                if (value == _tickEnable)
                    return;
                if (value)
                {
                    startTime = Model.TickCount;
                    Model.OnTick += onTick;
                }
                else
                    Model.OnTick -= onTick;
                _tickEnable = value;
            }
        }
        public TermiteMob(GameModel model, int x, int y) : base(model, "Termite/", x, y)
        {
            TickEnable = true;
        }

        public override void ForStop()
        {
            var itemsStack = Model.Map[X, Y].Items;
            if (itemsStack.Count > 0 &&
                itemsStack.Peek() is ThreeItem)
            {                
                itemsStack.Peek().Destroy();
                KeyMap.Enable = false;

                startTime = Model.TickCount;
            }
            base.ForStop();
        }

        public void onTick()
        {
            if ((Model.TickCount - startTime) % 100 == 0 && Model.TickCount - startTime >= 100)
            {
                var direction = Keys.None;
                if (checkThree(X, Y, Keys.Up))
                    direction = Keys.Up;
                if (checkThree(X, Y, Keys.Right))
                    direction = Keys.Right;
                if (checkThree(X, Y, Keys.Down))
                    direction = Keys.Down;
                if (checkThree(X, Y, Keys.Left))
                    direction = Keys.Left;

                if (direction == Keys.None)
                    TickEnable = false;
                else
                {
                    KeyMap.Enable = true;
                    GoTo(direction);
                }
            }
        }

        public void CheckerTurnOn()
        {
            TickEnable = true;
        }

        public override void ForMoveStart()
        {
            var willDie = Model.Map[MX, MY].Mobs.ToArray();
            for (var i = 0; i < willDie.Length; i++)
                willDie[i].Destroy();
            base.ForMoveStart();
        }

        private bool checkThree(int x, int y, Keys direction)
        {
            Useful.XyPlusKeys(x, y, direction, ref x, ref y);
            if (!Model.IsInsideMap(x, y))
                return false;
            if (Model.Map[x, y].Items.Count > 0 &&
                Model.Map[x, y].Items.Peek() is ThreeItem)
                return true;

            Useful.XyPlusKeys(x, y, direction, ref x, ref y);
            if (!Model.IsInsideMap(x, y))
                return false;
            return 
                Model.Map[x, y].Items.Count > 0 &&
                Model.Map[x, y].Items.Peek() is ThreeItem;
        }

        public override bool CanStep(IItem item)
        {
            if (item is ThreeItem)
                return true;
            return base.CanStep(item);
        }

        public override bool StandingAnimation => true;
        public override sbyte SlidesCount => 4;
        public override int SlideLatency => 10;
        public override double Speed => 0.02;
    }
}
