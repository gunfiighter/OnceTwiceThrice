using System;
using System.Drawing;

namespace OnceTwiceThrice
{
    public class ItemBase
    {
        public int X { get; }
        public int Y { get; }

        private Image _picture;
        public Image Picture
        {
            get => _picture;
            protected set
            {
                NeedInvalidate = true;
                _picture = value;
            }
        }

        public GameModel Model { get; }
		protected int animationCounter { get; set; }
		private sbyte animationMove = 1;
		private readonly Image[] slides;
		private readonly int slidesCount;
        public bool NeedInvalidate { get; set; }
        public event Action OnDestroy;
        public event Action<IMovable> OnStep;
        public void Step(IMovable mob)
        {
            OnStep?.Invoke(mob);
        }


        public ItemBase(GameModel model, int x, int y, int SlidesCount, string mobName)
		{
            this.Model = model;
			this.X = x;
			this.Y = y;
			slides = new Image[SlidesCount];
			for (int i = 0; i < SlidesCount; i++)
				slides[i] = Useful.GetImageByName(mobName + "/" + i);
			animationMove = 1;
			animationCounter = 0;
			Picture = slides[animationCounter];
			slidesCount = SlidesCount;

            OnDestroy += () =>
            {
                Model.Map[X, Y].Items.Remove(Model.Map[X, Y].Items.Peek());
                Model.NeedInvalidate = true;
                Model.OnTick -= onTick;
            };
		}

        public void Destroy()
        {
            OnDestroy?.Invoke();
        }

        public virtual void onTick() { }

		protected void ChangeSlide()
		{
			animationCounter += animationMove;
			if (animationCounter == slidesCount - 1)
				animationMove = -1;
			if (animationCounter == 0)
				animationMove = 1;			
			Picture = slides[animationCounter];
		}
	}
}