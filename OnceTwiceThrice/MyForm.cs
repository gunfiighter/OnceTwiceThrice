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
		private GameModel model;
		
		private Func<IMovable, int> GetPaintX = (mob) =>
			(int) Math.Round((mob.X + mob.DX) * DrawingScope);
		private Func<IMovable, int> GetPaintY = (mob) =>
			(int) Math.Round((mob.Y + mob.DY) * DrawingScope);

		private Timer timer;

		private void PaintGameState(object sender, PaintEventArgs args)
		{
			var g = args.Graphics;
			//Отрисовка фона
			model.DrawBackground(g);
				
			//Отрисовка предметов
			model.DrawItems(g);
				
			//Обводка героя
			g.DrawRectangle(new Pen(Color.Gold, 2), 
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
		}

		private void KeyDownInGame(object sender, KeyEventArgs args)
		{
			var keyCode = args.KeyCode;
			if (Helpful.KeyIsMove(keyCode))
			{
				model.CurrentHero.KeyMap.TurnOn(keyCode);
				model.CurrentHero.MakeMove(keyCode);
			}
			else
			{
				switch (keyCode)
				{
					case Keys.Tab:
						model.CurrentHero.KeyMap.TurnOff();
						model.SwitchHero();
						break;
				}
			}
		}

		private void KeyUpInGame(object sender, KeyEventArgs args)
		{
			model.CurrentHero.KeyMap.TurnOff(args.KeyCode);
		}

		private void TickInGame(object sender, EventArgs args)
		{
//			foreach (var hero in model.Heroes)
//				hero.MakeAnimation();
//			foreach (var mob in model.Mobs)
//				mob.MakeAnimation();
			model.Tick();
			Invalidate();
		}

		private void GameOver()
		{
			MessageBox.Show("GameOver");
			RemoveEvents();
			Close();
		}

		private void Win()
		{
			MessageBox.Show("You WIN");
			RemoveEvents();
			Close();
		}

		private void RemoveEvents()
		{
			KeyDown -= KeyDownInGame;
			KeyUp -= KeyUpInGame;
			Paint -= PaintGameState;
			timer.Dispose();
		}

		private void PlayTheGame(Lavel lavel)
		{
			timer = new Timer(){Interval = 10};
			
			model = new GameModel(lavel);
			Width = model.Width * DrawingScope + 50;
			Height = model.Height * DrawingScope + 50;
			
			KeyDown += KeyDownInGame;
			KeyUp += KeyUpInGame;
			model.OnGameOver += GameOver;
			model.OnWin += Win;
			Paint += PaintGameState;
			timer.Tick += TickInGame;
			
			timer.Start();
			
			Invalidate();
		}

		public MyForm()
		{
			DoubleBuffered = true;
			var lavels = new LavelsList();
			PlayTheGame(lavels.Levels[1]);
		}
	}
}
