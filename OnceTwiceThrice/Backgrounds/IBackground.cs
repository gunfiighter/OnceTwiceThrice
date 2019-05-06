using System.Drawing;

namespace OnceTwiceThrice
{
	public interface IBackground
	{
		Image Picture { get; }
		bool CanStep(MovableBase mob); //Можно ли наступить на объект
		bool CanStop(MovableBase mob); //Блокирует ли фон команды на движения мобов, находящемся на нем
	}
}