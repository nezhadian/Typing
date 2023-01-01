using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Typing
{
    public class KeyboardLayout
    {
        public Dictionary<Key, (char, char)> Layout = new Dictionary<Key, (char, char)>()
        {
            //ABC
            {Key.A,('a','A') },
            {Key.B,('b','B') },
            {Key.C,('c','C') },
            {Key.D,('d','D') },
            {Key.E,('e','E') },
            {Key.F,('f','F') },
            {Key.G,('g','G') },
            {Key.H,('h','H') },
            {Key.I,('i','I') },
            {Key.J,('j','J') },
            {Key.K,('k','K') },
            {Key.L,('l','L') },
            {Key.M,('m','M') },
            {Key.N,('n','N') },
            {Key.O,('o','O') },
            {Key.P,('p','P') },
            {Key.Q,('q','Q') },
            {Key.R,('r','R') },
            {Key.S,('s','S') },
            {Key.T,('t','T') },
            {Key.U,('u','U') },
            {Key.V,('v','V') },
            {Key.W,('w','W') },
            {Key.X,('x','X') },
            {Key.Y,('y','Y') },
            {Key.Z,('z','Z') },

            //Space
            {Key.Space,(' ',' ') },
            //OEMs
            {Key.Oem1,(';',':') },
            {Key.Oem2,('/','?') },
            {Key.Oem7,('\'','"') },
            {Key.OemComma,(',','<') },
            {Key.OemPeriod,('.','>') },
            
            //1-9
            {Key.D0,('0',')') },
            {Key.D1,('1','!') },
            {Key.D2,('2','@') },
            {Key.D3,('3','#') },
            {Key.D4,('4','$') },
            {Key.D5,('5','%') },
            {Key.D6,('6','^') },
            {Key.D7,('7','&') },
            {Key.D8,('8','*') },
            {Key.D9,('9','(') },
        };

        public char? Key2Char(Key key, bool isToggled)
        {
            if (Layout.TryGetValue(key, out (char a, char A) keyChar))
            {
                return isToggled ? keyChar.a : keyChar.A;
            }
            else
            {
                return null;
            }
        }
    }
}
