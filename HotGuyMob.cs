using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
    public class HotGuyMob : MobBase, IMob
    {
        private int startTime;
        public HotGuyMob(GameModel model, int x, int y) : base(model, "HotGuy/", x, y)
        {
            
            KeyMap.Enable = false;

            OnStop += () =>
            {
                var itemsStack = Model.Map[X, Y].Items;
                if (itemsStack.Count > 0 &&
                    itemsStack.Peek() is ThreeItem)
                {
                    startTime = Model.TickCount;
                    itemsStack.Peek().Destroy();
                    itemsStack.Add(new FireItem(Model, X, Y));
                    Model.OnTick += onTick;
                }
            };

            startTime = Model.TickCount;
            Model.OnTick += onTick;
        }

        public void onTick()
        {
            if (Model.TickCount - startTime == 100)
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
                    Model.OnTick -= onTick;
                else
                    GoTo(direction);
            }
        }

        public void TrySeachThree()
        {
            startTime = Model.TickCount - 100;
            onTick();
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

        public override bool SkinIgnoreDirection => true;
        public override sbyte SlidesCount => 4;
        public override int SlideLatency => 10;
        public override double Speed => 0.02;
        public override bool DestroyByMatthiusSpell => false;
        public override bool DestroyBySkimletSpell => true;
    }
}
