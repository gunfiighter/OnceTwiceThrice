using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnceTwiceThrice
{
	public interface ISpell
	{
		Image Picture { get; }
        int X { get; }
        int Y { get; }
        bool NeedInvalidate { get; set; }
    }
}
