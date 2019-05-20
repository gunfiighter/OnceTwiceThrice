using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace OnceTwiceThrice
{
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

        public IMob iMob { get; protected set; }
        public IHero iHero { get; protected set; }
		
		public virtual bool SkinIgnoreDirection => false;
		public virtual int SlideLatency { get => 10;  }

		public MovableBase(GameModel model, string imageFile, int x, int y)
		{
			this.Model = model;
			KeyMap = new KeyMap();
            iMob = null;
            iHero = null;
            
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

            model.Map[X, Y].Mobs.Add(this);

            OnMoveStart += ForMoveStart;

            OnStop += ForStop;

			model.OnTick += MakeAnimation;
			OnDestroy += () =>
			{

				if (this is IMob)
				{

					model.OnTick -= MakeAnimation;
					model.Mobs.Remove(this as IMob);
					model.Map[MX, MY].Mobs.Remove(this);
				}

				if (this is IHero)
				{
					model.OnTick -= MakeAnimation;
					model.Heroes.Remove(this as IHero);
					model.Map[MX, MY].Mobs.Remove(this);
				}
				model.Deaths.Add(new Death(model, X, Y));
				model.NeedInvalidate = true;
			};

		}

		public void MakeMove(Keys key)
		{

            if (this is IMob)
            {
                if (!CurrentAnimation.IsMoving)
                    GazeDirection = key;
            }
            else
                GazeDirection = key;

            if (CurrentAnimation.IsMoving)// && this is IMob)
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
            {
                if (StandingAnimation && Model.TickCount % SlideLatency == 0)
                    AnimatedMove();
                return;
            }

            NeedInvalidate = true;

            int newX = 0;
			int newY = 0;
			Useful.XyPlusKeys(0, 0, CurrentAnimation.Direction, ref newX, ref newY);
			DX += newX * Speed;
			DY += newY * Speed;

            CheckAllIntersection();

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

        public void CheckAllIntersection()
        {
            foreach (var mob in Model.Mobs)
            {
                if (mob != this)
                    Model.Conflict(this, mob);
            }
            foreach (var hero in Model.Heroes)
            {
                if (hero != this)
                    Model.Conflict(this, hero);
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
                foreach (var mob in Model.Map[newX, newY].Mobs)
                    if (mob is IHero)
                        return false;

			if (Model.Map[newX, newY].Items.Count > 0)
				return CanMoveOn(Model.Map[newX, newY].Items.Peek());
			return CanMoveOn(Model.Map[newX, newY].Back);
		}
        public void GoTo(Keys direction)
        {
            KeyMap.TurnOff();
            KeyMap.TurnOn(direction);
            MakeMove(direction);
        }

        public bool CanMoveOn(IItem item) => item.CanStep(this) || CanStep(item);
		public bool CanMoveOn(IBackground background) => background.CanStep(this) || CanStep(background);

        public virtual bool CanKill(IMob mob) => Strong.CanKill(this as IMob, mob);
        public virtual void ForStop()
        {
            var cell = Model.Map[X, Y];
            cell.Back.Step(this);
            if (cell.Items.Count > 0)
                cell.Items.Peek().Step(this);
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

        public virtual void ForMoveStart()
        {
            Model.Map[X, Y].Mobs.Remove(this);
            Model.Map[MX, MY].Mobs.Add(this);
            Model.MobMapChange(this);
        }

        public bool NeedInvalidate { get; set; }

        public virtual bool StandingAnimation => false;


        public virtual bool CanStep(IItem item) => false;
		public virtual bool CanStep(IBackground background) => false;
        public virtual bool IceSlip => false;
        public virtual bool DestroyByMatthiusSpell => true;
        public virtual bool DestroyBySkimletSpell => false;
    }
}
