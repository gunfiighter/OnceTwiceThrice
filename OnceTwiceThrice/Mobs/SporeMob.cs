namespace OnceTwiceThrice
{
	public class SporeMob : MobBase, IMob
	{
		public static string ImagePath = "Spore/";
		
		public SporeMob(GameModel model, int X, int Y): base(model, ImagePath, X, Y)
		{
			OnCantMove += (key) =>
			{
				this.Destroy();
			};
		}

		public override bool SkinIgnoreDirection => true;
		public override bool CanStep(IBackground background) => true;

		public override double Speed => 0.03;
        public override bool Explodable => false;
    }
}