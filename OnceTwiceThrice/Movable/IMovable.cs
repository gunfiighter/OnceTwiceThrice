using System;
using System.Drawing;
using System.Windows.Forms;

namespace OnceTwiceThrice
{
    public interface IMovable
    {
        GameModel Model { get; }
        Image Image { get; set; }
        KeyMap KeyMap { get; set; }
        void MakeMove(Keys key);
        void MakeAnimation();
        void Destroy();
        int X { get; }
        int Y { get; }
        int MX { get; }
        int MY { get; }
        double DX { get; }
        double DY { get; }

        event Action OnMoveStart;
        bool DestroyByMatthiusSpell { get; }
        bool DestroyBySkimletSpell { get; }
        Animation CurrentAnimation { get; }
        Keys GazeDirection { get; set; }
        void LockKeyMap();
        void UnlockKeyMap();
        void UpdateImage();
        bool NeedInvalidate { get; set; }
        void GoTo(Keys direction);
        bool IceSlip { get; }
        bool AllowToMove(Keys key);
        bool CanKill(IMob mob);
        IHero iHero { get; }
        IMob iMob { get; }
        double Speed { get; }
    }
}
