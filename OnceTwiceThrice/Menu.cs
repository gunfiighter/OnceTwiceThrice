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
        private PictureBox howToPlay;
        private PictureBox howToWin;
        private Button info;
        private Size buttonHelpSize = new Size(230, 100);

        public void CreateMenu()
        {
            var name = new PictureBox()
            {
                Image = Image.FromFile("../../images/Menu/name.png"),
                Location = new Point(180, 40),
                Size = new Size(584, 70)
            };
            howToPlay = new PictureBox()
            {
                Image = Image.FromFile("../../images/Menu/howToPlay.png"),
                Location = new Point(30, 430),
                Size = new Size(380, 270),
                BackColor = Color.Black
            };
            howToWin = new PictureBox()
            {
                Image = Image.FromFile("../../images/Menu/howToWin.png"),
                Location = new Point(420, 430),
                Size = new Size(520, 150),
                BackColor = Color.Black
            };

            var exit = new Button()
            {
                Size = buttonHelpSize,
                Location = new Point(700, 600),
                BackgroundImage = Image.FromFile("../../images/Menu/exit.png"),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            exit.MouseClick += (sender, args) =>
            {
                Close();
            };
            info = new Button()
            {
                Size = buttonHelpSize,
                Location = new Point(430, 600),
                BackgroundImage = Image.FromFile("../../images/Menu/faq.png"),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            info.MouseClick += (sender, args) =>
            {
                CreateInfoWindow();
            };
            Controls.Add(info);
            Controls.Add(exit);
            Controls.Add(name);
            Controls.Add(howToPlay);
            Controls.Add(howToWin);
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

        public void CreateInfoWindow()
        {
            foreach (var button in LevelButtons)
                button.Hide();
            howToPlay.Hide();
            howToWin.Hide();
            info.Hide();
            var toMenu = new Button()
            {
                Size = buttonHelpSize,
                Location = new Point(430, 600),
                BackgroundImage = Image.FromFile("../../images/Menu/toMenu.png"),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            toMenu.MouseClick += (sender, args) =>
            {
                CreateMenuWindow();
            };
            var heroesDescription = new PictureBox()
            {
                Image = Image.FromFile("../../images/Menu/heroesDescription.png"),
                Location = new Point(20, 130),
                Size = new Size(380, 255),
                BackColor = Color.Transparent
            };
            var mobsDescription = new PictureBox()
            {
                Image = Image.FromFile("../../images/Menu/mobsDescription.png"),
                Location = new Point(450, 130),
                Size = new Size(570, 450),
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            var itemsDescription = new PictureBox()
            {
                Image = Image.FromFile("../../images/Menu/itemsDescription.png"),
                Location = new Point(20, 380),
                Size = new Size(470, 350),
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            Controls.Add(toMenu);
            Controls.Add(heroesDescription);
            Controls.Add(mobsDescription);
            Controls.Add(itemsDescription);
        }

        public void CreateMenuWindow()
        {
            foreach (var button in LevelButtons)
                button.Show();
            howToPlay.Show();
            howToWin.Show();
            info.Show();
            Controls.RemoveAt(Controls.Count - 1);
            Controls.RemoveAt(Controls.Count - 1);
            Controls.RemoveAt(Controls.Count - 1);
        }

        public Button CreateLevelButton(Point buttonLocation, int number)
        {
            var buttonLevelSize = new Size(100, 100);
            var button = new Button()
            {
                Size = buttonLevelSize,
                Location = buttonLocation,
                BackgroundImage = Image.FromFile("../../images/Menu/LevelIcon/" + number + ".png"),
                BackgroundImageLayout = ImageLayout.Stretch
            };
            button.MouseClick += (sender, args) =>
            {
                var myForm = new MyForm(this, Levels.Levels[number]);
                myForm.ShowDialog();
            };
            return button;
        }
    }
}