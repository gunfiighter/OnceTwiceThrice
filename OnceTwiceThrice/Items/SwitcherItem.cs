using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnceTwiceThrice
{
    public class SwitcherItem : ItemBase, IItem
    {
        private bool _enable;
        public bool Enable
        {
            get => _enable;
            private set
            {
                _enable = value;
                if (!value)
                {
                    Model.ItemsMap[_tx, _ty].Push(new StoneItem(Model, _tx, _ty));
                    Picture = pictureOn;
                }
                else
                {
                    Picture = pictureOff;
                    if (Model.ItemsMap[_tx, _ty].Count > 0)
                        Model.ItemsMap[_tx, _ty].Peek().Destroy();
                }
            }
        }

        private int _tx;
        private int _ty;
        private Image pictureOn;
        private Image pictureOff;
        private int startTime;

        public SwitcherItem(GameModel model, int x, int y, int tx, int ty) : base(model, x, y, 1, "Switcher")
        {
            _tx = tx;
            _ty = ty;
            pictureOff = Useful.GetImageByName("Switcher/1");
            pictureOn = Useful.GetImageByName("Switcher/0");

            OnStep += (mob) =>
            {
                TurnOn();
            };
        }

        public override void onTick()
        {
            if ((Model.TickCount - startTime) % 10 == 0)
                TurnOff();
        }

        private bool CheckSwitch()
        {
            return
                Model.ItemsMap[X, Y].Count > 1 ||
                Model.MobMap[X, Y].Count > 0;
        }

        private void TurnOn()
        {
            if (Enable)
                return;
            if (CheckSwitch())
            {
                startTime = Model.TickCount;
                Enable = true;
                Model.OnTick += onTick;
            }
        }

        private void TurnOff()
        {
            if (!Enable)
                return;
            if (!CheckSwitch())
            {
                Enable = false;
                Model.OnTick -= onTick;
            }
        }
        public bool CanStep(MovableBase mob) => true;

        public bool CanStop(MovableBase mob) => true;
    }
}
