namespace OnceTwiceThrice
{
	public class MobBase : MovableBase
	{
		public MobBase(GameModel model, string ImageFile, int X, int Y) : base(model, ImageFile, X, Y)
		{
			;
			OnMoveStart += MoveStart;
		}

		public virtual void MoveStart()
		{
			foreach (var hero in Model.Heroes)
			{
				if ((hero.MX == MX && hero.MY == MY) || (hero.X == MX && hero.Y == MY))
				{
					hero.Destroy();
					return;
				}
			}
		}
		
		
		public override double Speed
		{
			get { return 0.04; }
		}
	}
}