using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSolitaire
{
    internal class SuiteChars
    {
        public char SpadeChar {  get; set; }
        public char ClubChar { get; set; }
        public char DiamondChar { get; set; }
        public char HeartChar { get; set; }

        public SuiteChars(char spadeChar, char clubChar, char diamondChar, char heartChar)
        {
            SpadeChar = spadeChar;
            ClubChar = clubChar;
            DiamondChar = diamondChar;
            HeartChar = heartChar;
        }
    }
}
