namespace OnceTwiceThrice
{
	public class SporesMob : MobBase, IMob
	{
		public override bool SkinIgnoreDirection => true; 
		public SporesMob(GameModel model, int X, int Y): base(model, "Spores", X, Y)
		{
			OnCantMove += (key) =>
			{
				this.Destroy();
			};
		}

		public override bool CanStep(IBackground background) => true;
	}
}