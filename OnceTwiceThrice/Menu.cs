using System.Drawing;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
	partial class Menu : Form
	{
		public Menu()
		{
			StartPosition = FormStartPosition.CenterScreen;
			var windowSize = new Size(MyForm.DrawingScope * 16 + 15, MyForm.DrawingScope * 12 + 38);
			MinimumSize = MaximumSize = windowSize;
			DoubleBuffered = true;
			Text = "OnceTwiceThrice!";
			Levels = new LevelsList();

			CreateMenu();
		}

		private Button[] LevelButtons;

		public void CreateMenu()
		{
			BackgroundImage = Image.FromFile("../../images/Menu/fon.png");
			MaximizeBox = false;
			LevelButtons = new Button[10];
			var buttonLocation = new Point(30, 160);
			for (int i = 0; i < 10; i++)
			{
				if (i == 5)
				{
					buttonLocation.Y = 300;
					buttonLocation.X = 30;
				}
				buttonLocation.X += 130;
				LevelButtons[i] = CreateLevelButton(buttonLocation, i);
				Controls.Add(LevelButtons[i]);
			}
		}

		public LevelsList Levels;

		public Button CreateLevelButton(Point buttonLocation, int number)
		{
			var buttonLevelSize = new Size(100, 100);
			var button = new Button();
			button.Size = buttonLevelSize;
			button.Location = buttonLocation;
			button.BackgroundImage = Image.FromFile("../../images/Menu/LevelIcon/" + number + ".png");
			button.BackgroundImageLayout = ImageLayout.Stretch;
			button.MouseClick += (sender, args) =>
			{
				Hide();
				var myForm = new MyForm(this, Levels.Levels[number]);
				myForm.ShowDialog();

			};
			return button;
		}
	}
}