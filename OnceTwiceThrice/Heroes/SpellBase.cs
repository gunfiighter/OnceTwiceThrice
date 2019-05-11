using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnceTwiceThrice
{
	public abstract class SpellBase
	{
		public Image Picture { get; }
        public int X { get; }
        public int Y { get; }
		public readonly GameModel Model;
        public readonly IHero Hero;
        public event Action OnDestroy;

        protected int Interval;
        protected int StartTime;

        protected void Destroy()
        {
            OnDestroy?.Invoke();
        }

		public SpellBase(IHero hero, int X, int Y, string ImageFile)
		{
            Interval = 100;
			this.Model = hero.Model;
            this.Hero = hero;
			this.X = X;
			this.Y = Y;
			Picture = Useful.GetImageByName(ImageFile);

            StartTime = Model.TickCount;
            Hero.LockKeyMap();

            OnDestroy += () => Model.Spells.Remove(this as ISpell);

            Model.OnTick += onTick;
		}

        private void onTick()
        {
            if (Model.TickCount - StartTime > Interval * 6 / 10)
                Hero.UnlockKeyMap();
            if (Model.TickCount - StartTime > Interval)
            {
                Destroy();
                Model.OnTick -= onTick;
            }
        }
	}
}
