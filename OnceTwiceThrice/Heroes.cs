namespace OnceTwiceThrice
{
	public class RedWizard : MovableBase
	{
		public RedWizard(GameModel model, string ImageFile, int X, int Y): base(model, ImageFile, X, Y)
		{
			;
		}

		public override bool CanStep() => true;
	}
}