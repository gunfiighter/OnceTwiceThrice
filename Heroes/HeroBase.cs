namespace OnceTwiceThrice
{
	public class HeroBase : MovableBase
	{
		public HeroBase(GameModel model, string ImageFile, int X, int Y) : base(model, ImageFile, X, Y)
		{
			OnDestroy += () => {
				model.GameOver(this);
			};
			OnStop += () =>
			{
				var itemsStack = model.ItemsMap[this.X, this.Y];
				if (itemsStack.Count > 0 && itemsStack.Peek() is DestinationItem)
					model.Win();
			};
			OnMoveStart += MoveStart;
		}
		
		public virtual void MoveStart()
		{
			foreach (var mob in Model.Mobs)
			{
				if ((mob.MX == MX && mob.MY == MY) || (mob.X == MX && mob.Y == MY))
				{
					this.Destroy();
					return;
				}
			}
		}

		public override double Speed
		{
			get { return 0.05; }
		}
	}
}