using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            (int)Math.Round((mob.X + mob.DX) * DrawingScope);
        private Func<IMovable, int> GetPaintY = (mob) =>
            (int)Math.Round((mob.Y + mob.DY) * DrawingScope);

        private Rectangle[,] regions;
        private Rectangle FPSRectangle = new Rectangle(0, 0, 80, 40);

        private Timer timer;

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

            g.DrawString(perfomance, new Font("Arial", 10, FontStyle.Regular), Brushes.Black, FPSRectangle);

            //g.DrawImage(model.CurrentHero.Image, new Rectangle(new Point(200, 200), new Size(50, 50)));
            //g.DrawImage(model.CurrentHero.Image, 100, 200, 100, 200);
        }

        private string perfomance;

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
            //if (model.TickCount % 100 == 0)
                Invalidate(GetDrawArea(), false);

            if (model.TickCount % 100 == 0)
            {
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                perfomance = String.Format("FPS:{0:00.00}", 1f / ((ts.Seconds * 1000 + ts.Milliseconds) * 1f / 100000));

                stopWatch = new Stopwatch();
                stopWatch.Start();
            }
        }

        private Stopwatch stopWatch;

        private Region GetDrawArea()
        {
            if (model.NeedInvalidate)
            {
                model.NeedInvalidate = false;
                return new Region();
            }

            var result = new Region(new Rectangle(0, 0, 0, 0));
            model.BackMap.Foreach((x, y) =>
            {
                if (model.BackMap[x, y].NeedInvalidate)
                {
                    result.Union(regions[x, y]);
                    model.BackMap[x, y].NeedInvalidate = false;
                }
            });
            model.ItemsMap.Foreach((x, y) =>
            {
                foreach (var item in model.ItemsMap[x, y])
                    if (item.NeedInvalidate) {
                        result.Union(regions[x, y]);
                        item.NeedInvalidate = false;
                    }
            });

            foreach (var mob in model.Mobs)
                if (mob.NeedInvalidate)
                {
                    result.Union(new Rectangle(
                        GetPaintX(mob) - DrawingScope * 1,
                        GetPaintY(mob) - DrawingScope * 1,
                        DrawingScope * 3, DrawingScope * 3));
                    mob.NeedInvalidate = false;
                }

            foreach (var hero in model.Heroes)
            {
                var borderWidth = 10;
                result.Union(new Rectangle(
                GetPaintX(hero) - borderWidth,
                GetPaintY(hero) - borderWidth,
                DrawingScope + borderWidth * 2, DrawingScope + borderWidth * 2));
            }
            foreach (var spell in model.Spells)
                if (spell.NeedInvalidate)
                    result.Union(regions[spell.X, spell.Y]);

            result.Union(regions[model.CurrentHero.X, model.CurrentHero.Y]);
            result.Union(FPSRectangle);
            return result;
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
            timer = new Timer() { Interval = 15 };
            model = new GameModel(lavel);

            regions = new Rectangle[model.Width, model.Height];
            for (var x = 0; x < lavel.Background[0].Length; x++)
                for (var y = 0; y < lavel.Background.Length; y++)
                    regions[x, y] =
                        new Rectangle(
                            x * DrawingScope,
                            y * DrawingScope,
                            DrawingScope,
                            DrawingScope);


            Width = model.Width * DrawingScope + 50;
            Height = model.Height * DrawingScope + 50;

            KeyDown += KeyDownInGame;
            KeyUp += KeyUpInGame;
            model.OnGameOver += GameOver;
            model.OnWin += Win;
            Paint += PaintGameState;
            timer.Tick += TickInGame;

            stopWatch = new Stopwatch();
            stopWatch.Start();
            timer.Start();

            Invalidate();
        }

        public MyForm()
        {
            DoubleBuffered = true;
            var levels = new LevelsList();
            CreateMenu();
            PlayTheGame(levels.Levels[0]);
        }

        public void CreateMenu()
        {
            MaximizeBox = false;
            MinimumSize = new Size(DrawingScope * 16 + 15, DrawingScope * 12 + 38);
            MaximumSize = new Size(DrawingScope * 16 + 15, DrawingScope * 12 + 38);
        }
    }
}
