
namespace ConsoleSolitaire
{
    internal class Suite
    {
        private char _suiteChar { get; set; }

        public char SuiteChar
        {
            get => _suiteChar;
        }

        public readonly List<string> Blank = [];

        public readonly List<string> Ace = [];

        public readonly List<string> Two = [];

        public readonly List<string> Three = [];

        public readonly List<string> Four = [];

        public readonly List<string> Five = [];

        public readonly List<string> Six = [];

        public readonly List<string> Seven = [];

        public readonly List<string> Eight = [];

        public readonly List<string> Nine = [];

        public readonly List<string> Ten = [];

        public readonly List<string> Jack = [];

        public readonly List<string> Queen = [];

        public readonly List<string> King = [];

        public Suite(char suiteChar)
        { 
            _suiteChar = suiteChar;

            Blank = [
                $"┏━━━━━━━━━━━┓",
                $"┃           ┃",
                $"┃           ┃",
                $"┃           ┃",
                $"┃           ┃",
                $"┃     {_suiteChar}     ┃",
                $"┃           ┃",
                $"┃           ┃",
                $"┃           ┃",
                $"┃           ┃",
                $"┗━━━━━━━━━━━┛"
            ];

            Ace = [
                $"┏━━━━━━━━━━━┓",
                $"┃ A         ┃",
                $"┃ {_suiteChar}         ┃",
                $"┃           ┃",
                $"┃           ┃",
                $"┃     {_suiteChar}     ┃",
                $"┃           ┃",
                $"┃           ┃",
                $"┃         {_suiteChar} ┃",
                $"┃         A ┃",
                $"┗━━━━━━━━━━━┛"
            ];

            Two = [
                $"┏━━━━━━━━━━━┓",  
                $"┃ 2         ┃",  
                $"┃ {_suiteChar}         ┃", 
                $"┃       {_suiteChar}   ┃", 
                $"┃           ┃",  
                $"┃           ┃",  
                $"┃           ┃",  
                $"┃   {_suiteChar}       ┃", 
                $"┃         {_suiteChar} ┃",  
                $"┃         2 ┃", 
                $"┗━━━━━━━━━━━┛"
            ];

            Three = [
                $"┏━━━━━━━━━━━┓",  
                $"┃ 3         ┃",  
                $"┃ {_suiteChar}         ┃", 
                $"┃       {_suiteChar}   ┃", 
                $"┃           ┃",  
                $"┃     {_suiteChar}     ┃", 
                $"┃           ┃",  
                $"┃   {_suiteChar}       ┃", 
                $"┃         {_suiteChar} ┃",  
                $"┃         3 ┃", 
                $"┗━━━━━━━━━━━┛"
            ];

            Four = [
                $"┏━━━━━━━━━━━┓",
                $"┃ 4         ┃",
                $"┃ {_suiteChar}         ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃           ┃",
                $"┃           ┃",
                $"┃           ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃         {_suiteChar} ┃",
                $"┃         4 ┃",
                $"┗━━━━━━━━━━━┛"
            ];

            Five = [
                $"┏━━━━━━━━━━━┓",
                $"┃ 5         ┃",
                $"┃ {_suiteChar}         ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃           ┃",
                $"┃     {_suiteChar}     ┃",
                $"┃           ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃         {_suiteChar} ┃",
                $"┃         5 ┃",
                $"┗━━━━━━━━━━━┛"
            ];

            Six = [
                $"┏━━━━━━━━━━━┓",
                $"┃ 6         ┃",
                $"┃ {_suiteChar}         ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃           ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃           ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃         {_suiteChar} ┃",
                $"┃         6 ┃",
                $"┗━━━━━━━━━━━┛"
            ];

            Seven = [
                $"┏━━━━━━━━━━━┓",
                $"┃ 7         ┃",
                $"┃ {_suiteChar}         ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃           ┃",
                $"┃   {_suiteChar} {_suiteChar} {_suiteChar}   ┃",
                $"┃           ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃         {_suiteChar} ┃",
                $"┃         7 ┃",
                $"┗━━━━━━━━━━━┛"
            ];

            Eight = [
                $"┏━━━━━━━━━━━┓",
                $"┃ 8         ┃",
                $"┃ {_suiteChar}         ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃           ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃         {_suiteChar} ┃",
                $"┃         8 ┃",
                $"┗━━━━━━━━━━━┛"
            ];

            Nine = [
                $"┏━━━━━━━━━━━┓",
                $"┃ 9         ┃",
                $"┃ {_suiteChar}         ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃     {_suiteChar}     ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃         {_suiteChar} ┃",
                $"┃         9 ┃",
                $"┗━━━━━━━━━━━┛"
            ];

            Ten = [
                $"┏━━━━━━━━━━━┓",
                $"┃ 10        ┃",
                $"┃ {_suiteChar}         ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃   {_suiteChar} {_suiteChar} {_suiteChar}   ┃",
                $"┃           ┃",
                $"┃   {_suiteChar} {_suiteChar} {_suiteChar}   ┃",
                $"┃   {_suiteChar}   {_suiteChar}   ┃",
                $"┃         {_suiteChar} ┃",
                $"┃        10 ┃",
                $"┗━━━━━━━━━━━┛"
            ];

            Jack = [
                $"┏━━━━━━━━━━━┓",
                $"┃ J         ┃",
                $"┃ {_suiteChar}         ┃",
                $"┃           ┃",
                $"┃           ┃",
                $"┃   {_suiteChar} J {_suiteChar}   ┃",
                $"┃           ┃",
                $"┃           ┃",
                $"┃         {_suiteChar} ┃",
                $"┃         J ┃",
                $"┗━━━━━━━━━━━┛"
            ];

            Queen = [
                $"┏━━━━━━━━━━━┓",
                $"┃ Q         ┃",
                $"┃ {_suiteChar}         ┃",
                $"┃           ┃",
                $"┃     {_suiteChar}     ┃",
                $"┃   {_suiteChar} Q {_suiteChar}   ┃",
                $"┃     {_suiteChar}     ┃",
                $"┃           ┃",
                $"┃         {_suiteChar} ┃",
                $"┃         Q ┃",
                $"┗━━━━━━━━━━━┛"
            ];

            King = [
                $"┏━━━━━━━━━━━┓",
                $"┃ K         ┃",
                $"┃ {_suiteChar}         ┃",
                $"┃     {_suiteChar}     ┃",
                $"┃   {_suiteChar} {_suiteChar} {_suiteChar}   ┃",
                $"┃ {_suiteChar} {_suiteChar} K {_suiteChar} {_suiteChar} ┃",
                $"┃   {_suiteChar} {_suiteChar} {_suiteChar}   ┃",
                $"┃     {_suiteChar}     ┃",
                $"┃         {_suiteChar} ┃",
                $"┃         K ┃",
                $"┗━━━━━━━━━━━┛"
            ];
        }
    }
}
