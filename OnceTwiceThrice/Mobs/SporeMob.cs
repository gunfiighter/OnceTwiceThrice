namespace OnceTwiceThrice
{
	public class SporeMob : MobBase, IMob
	{
		public override bool SkinIgnoreDirection => true; 
		public SporeMob(GameModel model, int X, int Y): base(model, "Spore/0", X, Y)
		{
			OnCantMove += (key) =>
			{
				this.Destroy();
			};
		}

		public override bool CanStep(IBackground background) => true;
	}
}