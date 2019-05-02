using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public class MyForm : Form
	{
		public KeyMap CurrentKeyMap;
		public const int DrawingScope = 80;
		public MovableBase CurrentHero;

		public MyForm()
		{
			DoubleBuffered = true;
			CurrentKeyMap = new KeyMap();
			var map = new GameMap(this, LavelsList.Levels[0]);
			Width = map.Width * DrawingScope + 50;
			Height = map.Height * DrawingScope + 50;
			
			var heroEnumerator = map.Heroes.GetEnumerator();
			CurrentHero = Helpful.GetNextHero(heroEnumerator);
			
			KeyDown += (sender, args) =>
			{
				var keyCode = args.KeyCode;
				if (Helpful.KeyIsMove(keyCode))
				{
					CurrentKeyMap.TurnOn(keyCode);
					CurrentHero.MakeMove(keyCode);
				}
				else
				{
					switch (keyCode)
					{
						case Keys.Tab:
							if (heroEnumerator.MoveNext())
							{
								CurrentHero = heroEnumerator.Current;
								break;
							}

							heroEnumerator = map.Heroes.GetEnumerator();
							heroEnumerator.MoveNext();
							CurrentHero = heroEnumerator.Current;
							break;
					}
				}

				Invalidate();
			};

			KeyUp += (sender, args) =>
			{
				CurrentKeyMap.TurnOff(args.KeyCode);
			};

			Paint += (sender, args) =>
			{
				var g = args.Graphics;
				//Отрисовка фона
				map.DrawBackground(g);
				
				//Отрисовка предметов
				map.DrawItems(g);
				
				//Обводка
				g.DrawRectangle(new Pen(Color.Gold, 3), 
					CurrentHero.X * DrawingScope, 
					CurrentHero.Y * DrawingScope, 
					DrawingScope, 
					DrawingScope);
				
				//Отрисовка героев
				foreach (var hero in map.Heroes)
				{
					g.DrawImage(hero.Image,
						new Point((int) ((hero.X + hero.xf) * DrawingScope),
							(int) ((hero.Y + hero.yf) * DrawingScope)));
				}
			};

			var timer = new Timer();
			timer.Interval = 10;
			timer.Tick += (sender, args) =>
			{
				foreach (var hero in map.Heroes)
					hero.MakeAnimation();
				Invalidate();
			};
			timer.Start();

			Invalidate();
		}
	}
}
