using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	public static class Useful
	{
		public static MovableBase GetNextHero(IEnumerator<MovableBase> enumerator)
		{
			if (enumerator.MoveNext())
				return enumerator.Current;
			enumerator.Reset();
			return GetNextHero(enumerator);
		}
		
		public static bool KeyIsMove(Keys key)
		{
			return
				key == Keys.Up ||
				key == Keys.Down ||
				key == Keys.Left ||
				key == Keys.Right;
		}

		public static Image GetImageByName(string name)
		{
			return Image.FromFile("../../images/" + name + ".png");
		}

		public static void XyPlusKeys(int x, int y, Keys key, ref int newX, ref int newY)
		{
			var dx = 0;
			var dy = 0;
			switch (key)
			{
				case Keys.Up: dy = -1; break;
				case Keys.Down: dy = 1; break;
				case Keys.Left: dx = -1; break;
				case Keys.Right: dx = 1; break;
				default: throw new ArgumentException();
			}
			newX = x + dx;
			newY = y + dy;
		}

		public static string DirectionName(Keys key)
		{
			switch (key)
			{
				case Keys.Up: return "Up";
				case Keys.Down: return "Down";
				case Keys.Left: return "Left";
				case Keys.Right: return "Right";
				default: throw new ArgumentException();
			}
		}

		public static string[] CutArray(string[] array, int begin, int end)
		{
            if (begin > end)
                return new string[0];
			var result = new string[end - begin + 1];
			for (var i = begin; i <= end; i++)
				result[i - begin] = array[i];
			return result;
		}

        public static void ForeachReverse(Cell.MyLinkedList<IItem> stack, Action<IItem> act)
        {
            var list = new List<IItem>();
            foreach(var e in stack)
                list.Add(e);
            for (var i = list.Count - 1; i >= 0; i--)
                act(list[i]);
        }

        public static bool CheckTouch(IMovable mob, int x, int y)
        {
            return (mob.X == x && mob.Y == y) ||
                    (mob.MX == x && mob.MY == y);
        }

        public static Keys ReverseDirection(Keys direction)
        {
            switch (direction)
            {
                case Keys.Up: return Keys.Down;
                case Keys.Right: return Keys.Left;
                case Keys.Down: return Keys.Up;
                case Keys.Left: return Keys.Right;
            }
            throw new ArgumentException();
        }
    }
}
