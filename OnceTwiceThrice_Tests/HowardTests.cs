using OnceTwiceThrice;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace Tests
{
    [TestClass]
    public class HowardTests
    {
        [TestMethod]
        public void SimpleMove()
        {
            var level = new Level(
                new[]
                {
                    "GGG",
                    "GGG",
                    "GGG"
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
                    ".H.",
                    "..."
                },
                new string[0]);
            var game = new PlayTestGame(level);
            game.GoWhileNotStop(Keys.Up, Tuple.Create(1, 0));
            game.GoWhileNotStop(Keys.Down, Tuple.Create(1, 1));
            game.GoWhileNotStop(Keys.Left, Tuple.Create(0, 1));
            game.GoWhileNotStop(Keys.Right, Tuple.Create(1, 1));
            game.GoWhileNotStop(Keys.Right, Tuple.Create(2, 1));
            game.GoWhileNotStop(Keys.Left, Tuple.Create(1, 1));
            game.GoWhileNotStop(Keys.Down, Tuple.Create(1, 2));
            game.GoWhileNotStop(Keys.Up, Tuple.Create(1, 1));
        }

        [TestMethod]
        public void MoveWithBarrier()
        {
            var level = new Level(
                new[]
                {
                    "GLG",
                    "GGG",
                    "GGW"
                },
                new List<string[]>(
                    new[] {
                        new[] {
                            "...",
                            "T.F",
                            "S.."
                        }
                    }),
                new[]
                {
                    "...",
                    ".H.",
                    "..."
                },
                new string[0]);
            var game = new PlayTestGame(level);
            game.GoWhileNotStop(Keys.Up, Tuple.Create(1, 1));
            game.GoWhileNotStop(Keys.Left, Tuple.Create(0, 1));
            game.GoWhileNotStop(Keys.Right, Tuple.Create(1, 1));
            game.GoWhileNotStop(Keys.Right, Tuple.Create(1, 1));
            game.GoWhileNotStop(Keys.Down, Tuple.Create(1, 2));
            game.GoWhileNotStop(Keys.Left, Tuple.Create(1, 2));
            game.GoWhileNotStop(Keys.Right, Tuple.Create(1, 2));
            game.GoWhileNotStop(Keys.Up, Tuple.Create(1, 1));
        }

        [TestMethod]
        public void Spell()
        {
            var level = new Level(
                new[]
                {
                    "GGG",
                    "GGG",
                    "GBW"
                },
                new List<string[]>(
                    new[] {
                        new[] {
                            "...",
                            "T.8",
                            "..."
                        }
                    }),
                new[]
                {
                    ".c.",
                    "mH.",
                    "..."
                },
                new string[0]);
            var game = new PlayTestGame(level);
            game.Turn(Keys.Up);
            game.CreateSpell();
            game.CreateSpell();
            game.CreateSpell();
            Assert.AreEqual(1, game.Model.Spells.Count);
            Assert.AreEqual(1, game.Model.Map[1, 0].Mobs.Count);
            game.DoTicks(100);
            Assert.AreEqual(0, game.Model.Map[1, 0].Items.Count);

            game.Turn(Keys.Left);
            game.CreateSpell();
            Assert.AreEqual(1, game.Model.Map[0, 1].Mobs.Count);
            Assert.AreEqual(1, game.Model.Map[0, 1].Items.Count);
            game.DoTicks(100);
            Assert.AreEqual(1, game.Model.Map[0, 1].Items.Count);

            game.Turn(Keys.Down);
            Assert.AreEqual(0, game.Model.Spells.Count);
            game.CreateSpell();
            Assert.AreEqual(1, game.Model.Spells.Count);
            Assert.AreEqual(0, game.Model.Map[1, 2].Items.Count);

            game.Turn(Keys.Right);
            game.CreateSpell();
            Assert.AreEqual(1, game.Model.Spells.Count);
            game.DoTicks(50);
            game.CreateSpell();
            Assert.AreEqual(1, game.Model.Spells.Count);
            game.DoTicks(10);
            game.CreateSpell();
            Assert.AreEqual(2, game.Model.Spells.Count);
            Assert.AreEqual(1, game.Model.Map[2, 1].Items.Count);
            game.DoTicks(100);
            Assert.AreEqual(2, game.Model.Map[2, 1].Items.Count);
        }
    }
}
