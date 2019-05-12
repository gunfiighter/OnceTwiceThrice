using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OnceTwiceThrice
{
	public class Animation
	{
		public bool IsMoving { get; set; }
		public Keys Direction { get; set; }
		public Animation()
		{
			IsMoving = false;
			Direction = Keys.None;
		}
	}

	public interface IMovable
	{
        GameModel Model { get; }
		Image Image { get; set; }
		KeyMap KeyMap { get; set; }
		void MakeMove(Keys key);
		void MakeAnimation();
		void Destroy();
		int X { get; }
		int Y { get; }
		int MX { get; }
		int MY { get; }
		double DX { get; }
		double DY { get; }

        event Action OnMoveStart;
        bool DestroyByMatthiusSpell { get; }
        bool DestroyBySkimletSpell { get; }
        Animation CurrentAnimation { get; }
        Keys GazeDirection { get; set; }
        void LockKeyMap();
        void UnlockKeyMap();
		void UpdateImage();
        bool NeedInvalidate { get; set; }
	}

	public class MovableBase : IMovable
	{
        private Image _image;
        public Image Image
        {
            get => _image;
            set
            {
                NeedInvalidate = true;
                _image = value;
            }
        }

		private Keys gazeDirection;
		public Keys GazeDirection {
			get => gazeDirection;
			set
			{
				gazeDirection = value;
				UpdateImage();
			}
		}
		
		private List<Image> goUp;
		private List<Image> goDown;
		private List<Image> goRight;
		private List<Image> goLeft;
		
		public GameModel Model { get; }

		public event Action OnStop; //Вызывается при завершени анимации шага
		public event Action<Keys> OnCantMove; //Вызывается при невозможности двигаться в заданном направлении
		public event Action OnDestroy; //Вызывается при уничтожении объекта
		public event Action OnMoveStart; //Вызывается когда принято решение двигаться
		
		public KeyMap KeyMap { get; set; }

        public void LockKeyMap()
        {
            KeyMap.Enable = false;
        }

        public void UnlockKeyMap()
        {
            KeyMap.Enable = true;
        }

		public int X { get; private set; }
		public int Y { get; private set; }
		
		public int MX { get; private set; }
		public int MY { get; private set; }

		public double DX { get; private set; }
		public double DY { get; private set; }

		public Animation CurrentAnimation { get; private set; }
		public virtual sbyte SlidesCount { get => 1; }

		public virtual double Speed
		{
			get { return 0.05; }
		}
		
		public virtual bool SkinIgnoreDirection => false;
		public virtual int SlideLatency { get => 10;  }

		public MovableBase(GameModel model, string imageFile, int x, int y)
		{
			this.Model = model;
			KeyMap = new KeyMap();
			goDown = new List<Image>();
			goUp = new List<Image>();
			goRight = new List<Image>();
			goLeft = new List<Image>();
			if (!SkinIgnoreDirection)
			{
				
				for(int i = 0; i < this.SlidesCount; i++)
				{
					goUp.Add(Useful.GetImageByName(imageFile + "Up/" + i));
					goDown.Add(Useful.GetImageByName(imageFile + "Down/" + i));
					goRight.Add(Useful.GetImageByName(imageFile + "Right/" + i));
					goLeft.Add(Useful.GetImageByName(imageFile + "Left/" + i));
				}
			}
			else
				for (var i = 0; i < SlidesCount; i++)
					goDown.Add(Useful.GetImageByName(imageFile + i));
			
			GazeDirection = Keys.Down;
			
			this.X = x;
			this.Y = y;
			MX = x;
			MY = y;
			CurrentAnimation = new Animation();
			CurrentAnimation.Direction = Keys.Down;

            model.MobMap[X, Y].AddLast(this);

            OnMoveStart += () =>
            {
                model.MobMap[X, Y].Remove(this);
                model.MobMap[MX, MY].AddLast(this);
            };

            OnStop += ForStop;

			model.OnTick += MakeAnimation;
			OnDestroy += () =>
			{

				if (this is IMob)
				{
					model.OnTick -= MakeAnimation;
					model.Mobs.Remove(this as IMob);
				}

				if (this is IHero)
				{
					model.OnTick -= MakeAnimation;
					model.Heroes.Remove(this as IHero);
				}
                model.NeedInvalidate = true;
			};
		}

		public void MakeMove(Keys key)
		{
			GazeDirection = key;

			if (CurrentAnimation.IsMoving)
				return;

			if (AllowToMove(key))
			{
				DX = DY = 0;
				var newMoveToX = X;
				var newMoveToY = Y;
				Useful.XyPlusKeys(X, Y, key, ref newMoveToX, ref newMoveToY);

				MX = newMoveToX;
				MY = newMoveToY;
				
				CurrentAnimation.IsMoving = true;
				CurrentAnimation.Direction = key;

				OnMoveStart?.Invoke();
			}
			else
			{
				OnCantMove?.Invoke(key);
			}
		}

		public void MakeAnimation()
		{
			if (SkinIgnoreDirection && Model.TickCount % SlideLatency == 0)
				AnimatedMove();
			if (!CurrentAnimation.IsMoving)
				return;

            NeedInvalidate = true;

            int newX = 0;
			int newY = 0;
			Useful.XyPlusKeys(0, 0, CurrentAnimation.Direction, ref newX, ref newY);
			DX += newX * Speed;
			DY += newY * Speed;
			if (Model.TickCount % SlideLatency == 0)
				AnimatedMove();
			if (1 - Math.Abs(DX) < 0.01 || 1 - Math.Abs(DY) < 0.01)
			{
				if (1 - Math.Abs(DX) < -0.01 || 1 - Math.Abs(DY) < -0.01)
					throw new Exception("DX или DY > 1");
				X = (int)Math.Round(X + DX, 0);
				Y = (int)Math.Round(Y + DY, 0);
				MX = X;
				MY = Y;
				DX = DY = 0;
				CurrentAnimation.IsMoving = false;
				OnStop?.Invoke();
			}
		}

		public void AnimatedMove()
		{
			slideCounter++;
			if (slideCounter == this.SlidesCount) slideCounter = 0;
			UpdateImage();
		}

		public void UpdateImage()
		{
			if (!SkinIgnoreDirection)
			{
				switch (GazeDirection)
				{
					case Keys.Down: Image = goDown[slideCounter]; break;
					case Keys.Up: Image = goUp[slideCounter]; break;
					case Keys.Right: Image = goRight[slideCounter]; break;
					case Keys.Left: Image = goLeft[slideCounter]; break;
				}
			} else
				Image = goDown[slideCounter];
		}

		private int slideCounter = 0;
		

		public void Destroy()
		{
			OnDestroy?.Invoke();
		}

		public bool AllowToMove(Keys key)
		{
			int newX = 0;
			int newY = 0;
			Useful.XyPlusKeys(X, Y, key, ref newX, ref newY);
			if (!Model.IsInsideMap(newX, newY))
				return false;

            if (this is IHero)
                foreach (var mob in Model.MobMap[newX, newY])
                    if (mob is IHero)
                        return false;

			if (Model.ItemsMap[newX, newY].Count > 0)
				return CanMoveOn(Model.ItemsMap[newX, newY].Peek());
			return CanMoveOn(Model.BackMap[newX, newY]);
		}

		public bool CanMoveOn(IItems item) => item.CanStep(this) || CanStep(item);
		public bool CanMoveOn(IBackground background) => background.CanStep(this) || CanStep(background);

        public virtual void ForStop()
        {
            if (!KeyMap.Enable)
                return;
            if (KeyMap[CurrentAnimation.Direction] &&
                Model.IsInsideMap(X, X, CurrentAnimation.Direction))
            {
                MakeMove(CurrentAnimation.Direction);
                return;
            }

            var nextDirection = KeyMap.GetAnyOnDirection();
            if (nextDirection != Keys.None)
                MakeMove(nextDirection);
        }

        public bool NeedInvalidate { get; set; }

		public virtual bool CanStep(IItems item) => false;
		public virtual bool CanStep(IBackground background) => false;
        public virtual bool DestroyByMatthiusSpell => true;
        public virtual bool DestroyBySkimletSpell => false;
    }
}
