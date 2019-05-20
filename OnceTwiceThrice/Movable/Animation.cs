using System.Windows.Forms;

namespace OnceTwiceThrice
{
    public class Animation
    {
        public bool IsMoving { get; set; }
        public Keys Direction { get; set; }
        public Animation()
        {
            IsMoving = false;
            Direction = Keys.None;
        }
    }
}
