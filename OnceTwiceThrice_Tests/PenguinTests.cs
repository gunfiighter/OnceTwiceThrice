using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnceTwiceThrice;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Tests
{
    [TestClass]
    public class PenguinTests
    {
        [TestMethod]
        public void FireAndIce()
        {
            var level = new Level(
                new[]
                {
                    "WGW",
                    "WWW",
                    "GWW"
                },
                new List<string[]>(
                    new[] {
                        new[] {
                            "...",
                            "...",
                            "..."
                        }
                    }),
                new[]
                {
                    "...",
                    ".p.",
                    "M.."
                },
                new string[0]);
            var game = new PlayTestGame(level);
            var mob = game.Model.Map[1, 1].Mobs.Peek();
            game.Turn(Keys.Up);
            game.CreateSpell();
            Assert.IsTrue(mob.CurrentAnimation.IsMoving);
            Assert.AreEqual(Keys.Right, mob.CurrentAnimation.Direction);
            Assert.IsInstanceOfType(game.Model.Map[mob.X, mob.Y].Back, typeof(IceBackground));

            game.DoTicksWhileNotStop(mob);
            Assert.AreEqual(Keys.Left, mob.CurrentAnimation.Direction);
            Assert.IsInstanceOfType(game.Model.Map[mob.X, mob.Y].Back, typeof(IceBackground));

            game.DoTicksWhileNotStop(mob);
            Assert.AreEqual(Keys.Right, mob.CurrentAnimation.Direction);
            Assert.IsInstanceOfType(game.Model.Map[mob.X, mob.Y].Back, typeof(IceBackground));

            game.DoTicksWhileNotStop(mob);
            Assert.AreEqual(Keys.Left, mob.CurrentAnimation.Direction);
            Assert.IsInstanceOfType(game.Model.Map[mob.X, mob.Y].Back, typeof(IceBackground));

            game.Turn(Keys.Right);
            game.CreateSpell();

            game.DoTicksWhileNotStop(mob);
            Assert.AreEqual(Keys.Up, mob.CurrentAnimation.Direction);
            Assert.IsInstanceOfType(game.Model.Map[mob.X, mob.Y].Back, typeof(IceBackground));

            game.DoTicksWhileNotStop(mob);
            Assert.AreEqual(Keys.Down, mob.CurrentAnimation.Direction);

            game.DoTicksWhileNotStop(mob);
            Assert.AreEqual(Keys.Up, mob.CurrentAnimation.Direction);
            Assert.IsInstanceOfType(game.Model.Map[mob.X, mob.Y].Back, typeof(IceBackground));

            game.DoTicksWhileNotStop(mob);
            Assert.AreEqual(Keys.Down, mob.CurrentAnimation.Direction);
            game.DoTicksWhileNotStop(mob);

            game.DoTicks(1);
            game.CreateSpell();
            Assert.AreEqual(0, game.Model.Mobs.Count);
        }
    }
}
