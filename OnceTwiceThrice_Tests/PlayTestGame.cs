using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;
using OnceTwiceThrice;

namespace Tests
{
    public class PlayTestGame
    {
        public GameModel Model;
        public void GoTo(Keys direction)
        {
            Model.CurrentHero.KeyMap.TurnOff(direction);
            Model.CurrentHero.MakeMove(direction);
            Model.CurrentHero.KeyMap.TurnOff(direction);
        }

        public void DoTicks(int count)
        {
            for (var i = 0; i < count; i++)
                Model.Tick();
        }

        public void GoWhileNotStop(Keys direction, Tuple<int, int> expected)
        {
            GoTo(direction);
            DoTicksWhileNotStop(Model.CurrentHero);
            Assert.AreEqual(expected, Tuple.Create(Model.CurrentHero.X, Model.CurrentHero.Y));
        }

        public void DoTicksWhileNotStop(IMovable mob)
        {
            DoTicks((int)Math.Round(1 / mob.Speed));
        }

        public void Turn(Keys direction)
        {
            Model.CurrentHero.GazeDirection = direction;
        }

        public void CreateSpell()
        {
            Model.CurrentHero.CreateSpell();
        }

        public PlayTestGame(Level level)
        {
            Model = new GameModel(level);
        }
    }
}
