using System;
using System.Drawing;

namespace OnceTwiceThrice
{
	public interface IBackground
	{
		Image Picture { get; }
        int X { get; }
        int Y { get; }
        bool NeedInvalidate { get; set; }
        event Action<IMovable> OnStep;
        void Step(IMovable mob);
		bool CanStep(MovableBase mob); //Можно ли наступить на объект
		bool CanStop(MovableBase mob); //Блокирует ли фон команды на движения мобов, находящемся на нем
	}
}
