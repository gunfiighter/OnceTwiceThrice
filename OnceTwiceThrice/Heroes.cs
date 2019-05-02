namespace OnceTwiceThrice
{
	public class RedWizard : MovableBase
	{
		public RedWizard(GameMap map, string ImageFile, int X, int Y): base(map, ImageFile, X, Y)
		{
			;
		}

		public override bool CanStep() => true;
	}
}