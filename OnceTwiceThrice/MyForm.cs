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
		public const int DrawingScope = 80;

		public MyForm()
		{
			DoubleBuffered = true;
			var model = new GameModel(LavelsList.Levels[0]);
			Width = model.Width * DrawingScope + 50;
			Height = model.Height * DrawingScope + 50;
			var KeyMap = model.CurrentHero.KeyMap;
			
			KeyDown += (sender, args) =>
			{
				var keyCode = args.KeyCode;
				if (Helpful.KeyIsMove(keyCode))
				{
					KeyMap.TurnOn(keyCode);
					model.CurrentHero.MakeMove(keyCode);
				}
				else
				{
					switch (keyCode)
					{
						case Keys.Tab:
							KeyMap.TurnOff();
							model.SwitchHero();
							KeyMap = model.CurrentHero.KeyMap;
							break;
					}
				}
			};

			KeyUp += (sender, args) =>
			{
				KeyMap.TurnOff(args.KeyCode);
			};

			Func<IMovable, int> GetPaintX = (mob) =>
				(int) Math.Round((mob.X + mob.DX) * DrawingScope);
			Func<IMovable, int> GetPaintY = (mob) =>
				(int) Math.Round((mob.Y + mob.DY) * DrawingScope);

			Paint += (sender, args) =>
			{
				var g = args.Graphics;
				//Отрисовка фона
				model.DrawBackground(g);
				
				//Отрисовка предметов
				model.DrawItems(g);
				
				//Обводка героя
				g.DrawRectangle(new Pen(Color.Gold, 3), 
					GetPaintX(model.CurrentHero), 
					GetPaintY(model.CurrentHero), 
					DrawingScope, 
					DrawingScope);
				
				//Отрисовка героев
				foreach (var hero in model.Heroes)
				{
					g.DrawImage(hero.Image,
						new Point(GetPaintX(hero), GetPaintY(hero)));
				}
				
				//Отрисовка мобов
				foreach (var mob in model.Mobs)
				{
					g.DrawImage(mob.Image,
						new Point(GetPaintX(mob), GetPaintY(mob)));
				}
			};

			var timer = new Timer();
			timer.Interval = 10;
			
//			KeyMap.TurnOn(Keys.Right);
//			model.CurrentHero.MakeMove(Keys.Right);
			
			timer.Tick += (sender, args) =>
			{
				foreach (var hero in model.Heroes)
					hero.MakeAnimation();
				foreach (var mob in model.Mobs)
					mob.MakeAnimation();
				Invalidate();
			};
			timer.Start();

			Invalidate();
		}
	}
}
