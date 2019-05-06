using System.Drawing;

namespace OnceTwiceThrice
{
	public interface IItems
	{
		Image Picture { get; }
		bool CanStep(MovableBase mob);//Можно ли наступить на объект
		bool CanStop(MovableBase mob); //Блокирует ли объект команды на движения мобов, находящемся на нем
		int X { get; }
		int Y { get; }
	}
}