using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSolitaire
{
    internal class Constants
    {
        public const char RedSuiteOne = '♥';
        public const char RedSuiteTwo = '♦';

        public const char BlackSuiteOne = '♠';
        public const char BlackSuiteTwo = '♣';

        public static bool IsRedSuite(char suite)
        {
            return suite == RedSuiteOne || suite == RedSuiteTwo;
        }
    }
}
