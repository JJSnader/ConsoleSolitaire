using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSolitaire
{
    public enum CardBack
    {
        Standard = 1,
        Clean = 2,
        Framed = 3,
        Diamond = 4
    }

    internal class CardBacks
    {
        public readonly List<string> Standard = [];

        public readonly List<string> Clean = [];

        public readonly List<string> Framed = [];

        public readonly List<string> Diamond = [];


        public CardBacks() 
        {
            Standard = [
                "┏━━━━━━━━━━━┓",
                "┃ ♠ ♦ ♠ ♦ ♠ ┃",
                "┃           ┃",
                "┃ ♥ ♣ ♥ ♣ ♥ ┃",
                "┃           ┃",
                "┃ ♠ ♦ ♠ ♦ ♠ ┃",
                "┃           ┃",
                "┃ ♥ ♣ ♥ ♣ ♥ ┃",
                "┃           ┃",
                "┃ ♠ ♦ ♠ ♦ ♠ ┃",
                "┗━━━━━━━━━━━┛"
            ];

            Clean = [
                "┏━━━━━━━━━━━┓",
                "┃ ♠       ♦ ┃",
                "┃           ┃",
                "┃           ┃",
                "┃           ┃",
                "┃           ┃",
                "┃           ┃",
                "┃           ┃",
                "┃           ┃",
                "┃ ♥       ♣ ┃",
                "┗━━━━━━━━━━━┛"
            ];

            Framed = [
                "┏━━━━━━━━━━━┓",
                "┃ ┏━━━━━━━┓ ┃",
                "┃ ┃   ♠   ┃ ┃",
                "┃ ┃       ┃ ┃",
                "┃ ┃   ♦   ┃ ┃",
                "┃ ┃       ┃ ┃",
                "┃ ┃   ♥   ┃ ┃",
                "┃ ┃       ┃ ┃",
                "┃ ┃   ♣   ┃ ┃",
                "┃ ┗━━━━━━━┛ ┃",
                "┗━━━━━━━━━━━┛"
            ];

            Diamond = [
                "┏━━━━━━━━━━━┓",
                "┃ ♦♦♦♦♦♦♦♦♦ ┃",
                "┃ ♦♦♦♦♦♦♦♦♦ ┃",
                "┃ ♦♦♦♦♦♦♦♦♦ ┃",
                "┃ ♦♦♦♦♦♦♦♦♦ ┃",
                "┃ ♦♦♦♦♦♦♦♦♦ ┃",
                "┃ ♦♦♦♦♦♦♦♦♦ ┃",
                "┃ ♦♦♦♦♦♦♦♦♦ ┃",
                "┃ ♦♦♦♦♦♦♦♦♦ ┃",
                "┃ ♦♦♦♦♦♦♦♦♦ ┃",
                "┗━━━━━━━━━━━┛"
            ];
        
        }

        public List<string> GetCardBack(CardBack back)
        {
            return back switch
            {
                CardBack.Standard => Standard,
                CardBack.Clean => Clean,
                CardBack.Framed => Framed,
                CardBack.Diamond => Diamond,
                _ => Standard,
            };
        }

    }
}
