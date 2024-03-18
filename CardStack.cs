
namespace ConsoleSolitaire
{
    internal class CardStack
    {
        private readonly List<Card> cards;

        public CardStack() 
        {
            cards = new();
        }

        public CardStack(List<Card> cards)
        {
            this.cards = cards;
        }

        public Card this[int index]
        { 
            get => cards[index]; 
            set => cards[index] = value; 
        }

        public int Count
        {
            get => cards.Count;
        }

        public int BottomRevealedCardIndex
        {
            get
            {
                if (cards.Count == 0) return -1;
                var index = 0;
                while (index < cards.Count && !cards[index].Revealed)
                    index++;
                if (index == cards.Count) return -1;
                return index;
            }
        }


        public void Add(Card card)
        {
            cards.Add(card);
        }

        public void RemoveAt(int index)
        {
            cards.RemoveAt(index);
        }

        public List<string> GetDisplay()
        {
            var result = new List<string>();

            if (cards == null || cards.Count == 0)
                return result;

            for (int i = 0; i < cards.Count; i++)
            {
                if (!cards[i].Revealed)
                {
                    result.Add("┏━━━━━━━━━━━┓");
                }
                else if (i !=  cards.Count - 1)
                {
                    result.Add($"┏━━━━━━━━━━━┓");
                    result.Add($"┃ {cards[i].Number,-8}{cards[i].Suite} ┃");
                }
                else
                    foreach (var row in cards[i].CardDisplay)
                        result.Add(row);                
            }

            return result;
        }

        public List<string> GetColorMap()
        {
            var result = new List<string>();

            if (cards == null || cards.Count == 0)
                return result;

            for (int i = 0; i < cards.Count; i++)
            {
                if (!cards[i].Revealed)
                {
                    result.Add("".PadLeft(Deck.Card_Width, 'W'));
                }
                else if (i != cards.Count - 1)
                {
                    result.Add("".PadLeft(Deck.Card_Width, 'W'));

                    var redSuites = new List<char>(){ '♦', '♥' };
                    if (redSuites.Contains(cards[i].Suite))
                        result.Add("WW" + "".PadLeft(Deck.Card_Width - 4, 'R') + "WW");
                    else 
                        result.Add("".PadLeft(Deck.Card_Width, 'W'));
                }
                else
                    foreach (var row in cards[i].CardColorMap)
                        result.Add(row);
            }

            return result;
        }
    }
}
