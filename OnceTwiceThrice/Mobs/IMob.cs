using System;

namespace OnceTwiceThrice
{
	public interface IMob : IMovable
	{
		event Action OnMoveStart;
	}
}