
namespace ConsoleSolitaire
{
    internal class Card
    {
        private readonly List<string> _cardDisplay;
        private readonly List<string> _cardColorMap;
        private readonly string _number;
        private readonly char _suite;
        private bool _revealed;

        public List<string> CardDisplay 
        {
            get => _cardDisplay;
        }

        public List<string> CardColorMap
        {
            get => _cardColorMap;
        }

        public string Number
        {
            get => _number;
        }

        public char Suite
        {
            get => _suite;
        }

        public bool Revealed 
        { 
            get => _revealed; 
            set => _revealed = value; 
        }

        public Card(List<string> display, bool revealed, string number, char suite)
        {
            _cardDisplay = display;
            _cardColorMap = [];
            List<char> numbers = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'J', 'Q', 'K', 'A'];
            if (suite == '♠' || suite == '♣')
                for (int i = 0; i < display.Count; i++)
                    _cardColorMap.Add("".PadLeft(Deck.Card_Width, 'W'));
            else
            {
                for (int i = 0; i < display.Count; i++)
                {
                    var line = string.Empty;
                    for (int j = 0; j < display[i].Length; j++)
                    {
                        if (numbers.Contains(display[i][j]) || display[i][j] == '♦' || display[i][j] == '♥')
                            line += 'R';
                        else
                            line += 'W';
                    }
                    _cardColorMap.Add(line);
                }
            }
            _revealed = revealed;
            _number = number;
            _suite = suite;
        }

        public static bool operator ==(Card? left, Card? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null && right is null) return true;
            if (left is null || right is null) return false;

            return left.Number == right.Number && left.Suite == right.Suite;
        }

        public static bool operator !=(Card? left, Card? right)
        {
            return !(left == right);
        }
    }
}
