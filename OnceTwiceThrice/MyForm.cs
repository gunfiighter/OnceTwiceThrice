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
		public const int DrawingScope = 60;//размер клетки в px
		private GameModel model;
		
		private Func<IMovable, int> GetPaintX = (mob) =>
			(int) Math.Round((mob.X + mob.DX) * DrawingScope);
		private Func<IMovable, int> GetPaintY = (mob) =>
			(int) Math.Round((mob.Y + mob.DY) * DrawingScope);

		private Timer timer;

        private Rectangle[,] RectangleMap;

		private void PaintGameState(object sender, PaintEventArgs args)
		{
			var g = args.Graphics;
            //Отрисовка фона
            model.BackMap.Foreach((x, y) =>
            {
                g.DrawImage(
                    model.BackMap[x, y].Picture,
                    x * DrawingScope,
                    y * DrawingScope,
                    DrawingScope,
                    DrawingScope);
            });

            //Отрисовка предметов
            model.ItemsMap.Foreach((x, y) =>
            {
                Useful.ForeachReverse(model.ItemsMap[x, y], (item) =>
                {
                    g.DrawImage(
                        item.Picture,
                        item.X * DrawingScope,
                        item.Y * DrawingScope,
                        DrawingScope,
                        DrawingScope);
                });
            });

            foreach (var spell in model.Spells)
			{
				g.DrawImage(spell.Picture,
						spell.X * DrawingScope, 
						spell.Y * DrawingScope,
                        DrawingScope,
                        DrawingScope);
			}

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
				    GetPaintX(hero),
                    GetPaintY(hero),
                    DrawingScope,
                    DrawingScope);
			}
				
			//Отрисовка мобов
			foreach (var mob in model.Mobs)
			{
				g.DrawImage(mob.Image,
					GetPaintX(mob),
                    GetPaintY(mob),
                    DrawingScope,
                    DrawingScope);
			}

            //g.DrawImage(model.CurrentHero.Image, new Rectangle(new Point(200, 200), new Size(50, 50)));
            //g.DrawImage(model.CurrentHero.Image, 100, 200, 100, 200);
        }

		private void KeyDownInGame(object sender, KeyEventArgs args)
		{
			var keyCode = args.KeyCode;

			if (!model.CurrentHero.KeyMap.Enable)
				return;
			if (Useful.KeyIsMove(keyCode))
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
					case Keys.Space:
                        if (model.CurrentHero.KeyMap.GetAnyOnDirection() == Keys.None &&
                            !model.CurrentHero.CurrentAnimation.IsMoving)
						    model.CurrentHero.CreateSpell();
						break;
					case Keys.ControlKey:
						switch (model.CurrentHero.GazeDirection)
						{
							case Keys.Up:
								model.CurrentHero.GazeDirection = Keys.Right;

								break;
							case Keys.Down:
								model.CurrentHero.GazeDirection = Keys.Left; break;
							case Keys.Right:
								model.CurrentHero.GazeDirection = Keys.Down; break;
							case Keys.Left:
								model.CurrentHero.GazeDirection = Keys.Up; break;
						}
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
			model.Tick();
            if (model.TickCount % 3 == 0)
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

		private void PlayTheGame(Level lavel)
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
			var levels = new LevelsList();
			PlayTheGame(levels.Levels[0]);
		}
	}
}
