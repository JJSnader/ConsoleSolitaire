using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSolitaire
{
    internal class CardStack
    {
        private List<Card> cards;

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
    }
}
