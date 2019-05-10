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
		Image Image { get; }
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
	}

	public class MovableBase : IMovable
	{
		public Image Image {
			get
			{
				if (!SkinIgnoreDirection)
				{
					switch (lastDirection)
					{
						case Keys.Up: return goUp[0];
						case Keys.Down: return goDown[0];
						case Keys.Right: return goRight[0];
						case Keys.Left: return goLeft[0];
					}
				}

			return goDown[0];
			}
		}

		private Keys lastDirection;
		
		private List<Image> goUp;
		private List<Image> goDown;
		private List<Image> goRight;
		private List<Image> goLeft;
		
	

		public GameModel Model;

		public event Action OnStop; //Вызывается при завершени анимации шага
		public event Action<Keys> OnCantMove; //Вызывается при невозможности двигаться в заданном направлении
		public event Action OnDestroy; //Вызывается при уничтожении объекта
		public event Action OnMoveStart; //Вызывается когда принято решение двигаться
		
		public KeyMap KeyMap { get; set; }

		public int X { get; private set; }
		public int Y { get; private set; }
		
		public int MX { get; private set; }
		public int MY { get; private set; }

		public double DX { get; private set; }
		public double DY { get; private set; }

		public Animation CurrentAnimation;

		public virtual double Speed
		{
			get { return 0.05; }
		}
		
		public virtual bool SkinIgnoreDirection => false;

		public MovableBase(GameModel model, string ImageFile, int X, int Y)
		{
			this.Model = model;
			KeyMap = new KeyMap();

			goDown = new List<Image>();
			if (!SkinIgnoreDirection)
			{
				goUp = new List<Image>();
				goRight = new List<Image>();
				goLeft = new List<Image>();

				goUp.Add(Useful.GetImageByName(ImageFile + "Up/0"));
				goDown.Add(Useful.GetImageByName(ImageFile + "Down/0"));
				goRight.Add(Useful.GetImageByName(ImageFile + "Right/0"));
				goLeft.Add(Useful.GetImageByName(ImageFile + "Left/0"));
			}
			else
			{
				goDown.Add(Useful.GetImageByName(ImageFile));
			}

			lastDirection = Keys.Down;
			
			this.X = X;
			this.Y = Y;
			MX = X;
			MY = Y;
			CurrentAnimation = new Animation();

			OnStop += () =>
			{
				if (KeyMap[CurrentAnimation.Direction] &&
				    Model.IsInsideMap(X, Y, CurrentAnimation.Direction))
				{
					MakeMove(CurrentAnimation.Direction);
					return;
				}

				var nextDirection = KeyMap.GetAnyOnDirection();
				if (nextDirection != Keys.None)
					MakeMove(nextDirection);
			};

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
			};
		}
		public void MakeMove(Keys key)
		{
			lastDirection = key;
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
			if (!CurrentAnimation.IsMoving)
				return;
			int newX = 0;
			int newY = 0;
			Useful.XyPlusKeys(0, 0, CurrentAnimation.Direction, ref newX, ref newY);
			DX += newX * Speed;
			DY += newY * Speed;
			
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

		public void Destroy()
		{
			OnDestroy?.Invoke();
		}

		//public virtual bool IsHero() => false;

		public bool AllowToMove(Keys key)
		{
			int newX = 0;
			int newY = 0;
			Useful.XyPlusKeys(X, Y, key, ref newX, ref newY);
			if (!Model.IsInsideMap(newX, newY))
				return false;
			if (Model.ItemsMap[newX, newY].Count > 0)
				return CanMoveOn(Model.ItemsMap[newX, newY].Peek());
			return CanMoveOn(Model.BackMap[newX, newY]);
		}

		public bool CanMoveOn(IItems item) => item.CanStep(this) || CanStep(item);
		public bool CanMoveOn(IBackground background) => background.CanStep(this) || CanStep(background);

		public virtual bool CanStep(IItems item) => false;
		public virtual bool CanStep(IBackground background) => false;
	}
}
