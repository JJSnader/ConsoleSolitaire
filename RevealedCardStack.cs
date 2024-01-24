using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSolitaire
{
    internal class RevealedCardStack
    {
        private readonly List<Card> cards;

        public Card this[int index]
        {
            get => cards[index];
        }
        public void Add(Card card)
        {
            cards.Add(card);
        }

        public void Clear()
        {
            cards.Clear();
        }

        public List<Card> Cards => cards;

        public int Count => cards.Count;

        public Card Top
        {
            get => cards.LastOrDefault();
        }

        public RevealedCardStack()
        {
            cards = [];
        }

        private const int Back_Card_Width = 4;

        public List<string> GetDisplay()
        {
            var result = new List<string>();

            for (int i = 0; i < cards.Count; i++)
            {
                var cardDisplay = cards[i].CardDisplay;
                for (int j = 0; j < cardDisplay.Count; j++)
                {
                    if (j >= result.Count)
                    {
                        result.Add(cardDisplay[j][..Back_Card_Width]);
                        if (i == cards.Count - 1)
                            result[j] += cardDisplay[j][Back_Card_Width..];
                    }
                    else
                    {
                        result[j] += cardDisplay[j][..Back_Card_Width];
                        if (i == cards.Count - 1)
                            result[j] += cardDisplay[j][Back_Card_Width..];
                    }
                }
            }

            return result;
        }

        public List<string> GetColorMap()
        {
            var result = new List<string>();
            List<char> numbers = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'J', 'Q', 'K', 'A'];
            for (int i = 0; i < cards.Count; i++)
            {
                var cardDisplay = cards[i].CardDisplay;
                for (int j = 0; j < cardDisplay.Count; j++)
                {
                    var displayChars = string.Empty;
                    var cutoff = Back_Card_Width;
                    if (i == cards.Count - 1)
                        cutoff = cardDisplay[j].Length;

                    for (int k = 0; k < cutoff; k++)
                    {
                        if ((numbers.Contains(cardDisplay[j][k]) && Constants.IsRedSuite(cards[i].Suite))
                            || Constants.IsRedSuite(cardDisplay[j][k]))
                        {
                            displayChars += 'R';
                        }
                        else
                            displayChars += 'W';
                    }

                    if (j >= result.Count)
                        result.Add(displayChars);
                    else
                        result[j] += displayChars;
                }
            }

            return result;
        }
    }
}
