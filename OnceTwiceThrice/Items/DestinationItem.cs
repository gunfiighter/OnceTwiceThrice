
namespace OnceTwiceThrice
{
	public class DestinationItem : ItemBase, IItem
	{
		public DestinationItem(GameModel model, int x, int y) : base(model, x, y, 4, "Destination")
		{
            model.OnTick += onTick;
        }

        public override void onTick()
        {
            if (Model.TickCount % 18 == 0)
                this.ChangeSlide();
        }

        public bool CanStep(MovableBase mob) => true;
		public bool CanStop(MovableBase mob) => true;
	}
}