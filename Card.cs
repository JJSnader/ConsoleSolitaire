using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSolitaire
{
    internal class Card
    {
        private List<string> _cardDisplay = new();
        private bool _revealed;
        string _number;
        char _suite;

        public List<string> CardDisplay 
        {
            get => _cardDisplay;
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
