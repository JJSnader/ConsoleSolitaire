using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleSolitaire
{
    internal class Deck
    {
        public const int Card_Width = 13;
        public const int Card_Height = 11;

        public const int Display_Height = 46;
        public const int Display_Width = 99;

        private List<Card> _allCards = [];
        private List<string> _helpText = [
            "Flip - Show next card in deck | Move 10S JH - Move 10 of Spades to Jack of Hearts | Help ",
            "Pack AH - Move Ace of Hearts to Hearts pile | Exit - Exit game | New - Start new game"
        ];
        public string UserMessage = string.Empty;

        public List<string> ColorMap = [];

        public List<CardStack> Stacks { get; set; }

        public List<Card> CardDeck { get; set; }

        public List<Card> DiscardedCardDeck { get; set; }

        public Card? RevealedDeckCard { get; set; }

        public Card? TopSpade 
        {
            get => _topSpade;
            set => _topSpade = value; 
        }
        public Card? TopHeart 
        { 
            get => _topHeart; 
            set => _topHeart = value; 
        }
        public Card? TopDiamond { get; set; }
        public Card? TopClub { get; set; }

        private Card? _topSpade;
        private Card? _topHeart;
        private Card? _topDiamond;
        private Card? _topClub;

        private Card CardBack { get; set; }
        private Card EmptyDeck { get; set; }

        public Deck() 
        {
            _allCards = [];
            Stacks = [];
            CardDeck = [];
            DiscardedCardDeck = [];
            CardBack = new Card([
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
            ], false, "0", 'X');
            EmptyDeck = new Card([
                "┏━━━━━━━━━━━┓",
                "┃           ┃",
                "┃           ┃",
                "┃           ┃",
                "┃           ┃",
                "┃     X     ┃",
                "┃           ┃",
                "┃           ┃",
                "┃           ┃",
                "┃           ┃",
                "┗━━━━━━━━━━━┛"
            ], false, "0", 'X');
            LoadDeck(); 
        }

        public List<string> GetDisplay()
        {
            ColorMap = [];
            for (int i = 0; i < Display_Height; i++)
                ColorMap.Add("".PadLeft(Display_Width, 'G'));
            
            var result = new List<string>(Display_Height);
            for (int i = 0; i < Display_Height; ++i)
                result.Add("".PadLeft(Display_Width));

            if (CardDeck.Count > 0)
                DrawSubDisplay(0, 0, CardBack.CardDisplay, result);
            else
                DrawSubDisplay(0, 0, EmptyDeck.CardDisplay, result);

            if (RevealedDeckCard != null)
                DrawSubDisplay(0, Card_Width + 1, RevealedDeckCard.CardDisplay, result);
            else
                DrawSubDisplay(0, Card_Width + 1, EmptyDeck.CardDisplay, result);

            DrawSubDisplay(0, (Card_Width * 3) + 3, TopSpade == null ? EmptyDeck.CardDisplay : TopSpade.CardDisplay, result);
            DrawSubDisplay(0, (Card_Width * 4) + 4, TopHeart == null ? EmptyDeck.CardDisplay : TopHeart.CardDisplay, result);
            DrawSubDisplay(0, (Card_Width * 5) + 5, TopDiamond == null ? EmptyDeck.CardDisplay : TopDiamond.CardDisplay, result);
            DrawSubDisplay(0, (Card_Width * 6) + 6, TopClub == null ? EmptyDeck.CardDisplay : TopClub.CardDisplay, result);

            for (int i = 0; i < Stacks.Count; ++i)
                DrawSubDisplay(Card_Height + 2, (Card_Width * i) + i, Stacks[i].GetDisplay(), result);

            result[^3] = _helpText[0].PadRight(Display_Width);
            result[^2] = _helpText[1].PadRight(Display_Width);
            result[^1] = UserMessage.PadRight(Display_Width);

            for (int i = 0; i < result.Count; ++i)
                result[i] = result[i].PadRight(Display_Width);

            // Figure out the color map
            for (int s = 0; s < result.Count; s++)
            {
                var inCard = false;
                for (int i = 0; i < ColorMap[s].Length; ++i)
                {
                    if (i >= result[s].Length)
                    {
                        ColorMap[s] = ColorMap[s][..i];
                        break;
                    }
                    if (result[s][i] == '┏' || result[s][i] == '┗')
                        ColorMap[s] = ColorMap[s][..i] + "".PadLeft(Card_Width, 'W') + ColorMap[s][(i + Card_Width)..];
                    else if (result[s][i] == '┃')
                    {
                        inCard = !inCard;
                        if (inCard)
                        {
                            string whites = "".PadLeft(Card_Width, 'W');
                            string cardLine = ColorMap[s].Substring(i, Card_Width);
                            if (cardLine.Contains('R'))
                            {
                                whites = string.Empty;
                                foreach (char c in cardLine)
                                    if (c == 'R')
                                        whites += 'R';
                                    else
                                        whites += 'W';
                                // This is specifically for revealed red suite cards that
                                // are behind another revealed card
                                if (whites == "WWWWWWWWWRRWW" || whites == "WWWWWWWWWWRWW")
                                    whites = "WWRRWWWWWRRWW";
                            }
                            ColorMap[s] = ColorMap[s][..i] + whites + ColorMap[s][(i + Card_Width)..];
                        }
                    }
                    if (result[s][i] == '♥' || result[s][i] == '♦')
                    {
                        ColorMap[s] = ColorMap[s][..i] + 'R' + ColorMap[s][(i + 1)..];
                        List<char> numbers = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'J', 'Q', 'K', 'A'];
                        // Make number red on revealed card below other cards
                        if (i - 8 > 0 && numbers.Contains(result[s][i - 8]))
                            ColorMap[s] = ColorMap[s][..(i - 9)] + "RRR" + ColorMap[s][(i - 6)..];
                        // Do two digits in case the number is 10
                        if (s - 1 >= 0 && numbers.Contains(result[s - 1][i]))
                            ColorMap[s - 1] = ColorMap[s - 1][..i] + "RR" + ColorMap[s - 1][(i + 2)..];
                        if (s + 1 < ColorMap.Count && numbers.Contains(result[s + 1][i]))
                            ColorMap[s + 1] = ColorMap[s + 1][..(i - 1)] + "RR" + ColorMap[s + 1][(i + 1)..];
                        // Special case for royalty
                        if (i + 2 < result[s].Length && numbers.Contains(result[s][i + 2]))
                            ColorMap[s] = ColorMap[s][..(i + 2)] + "R" + ColorMap[s][(i + 3)..];

                    }                    
                }
            }

            return result;
        }

        static void DrawSubDisplay(int topX, int topY, List<string> subDisplay, List<string> display)
        {
            for (int x = topX; x < (topX + subDisplay.Count); ++x)
            {
                if (x >= display.Count)
                    break;
                display[x] = display[x].Insert(topY, subDisplay[x - topX]);
                display[x] = display[x].Remove(topY + Card_Width, Card_Width);
            }
        }

        private void LoadDeck()
        {
            List<Suite> suites = [];
            suites.Add(new Suite('♥'));
            suites.Add(new Suite('♦'));
            suites.Add(new Suite('♣'));
            suites.Add(new Suite('♠'));

            foreach (var s in suites)
            {
                if (s.SuiteChar == '♥')
                    TopHeart = new Card(s.Blank, false, "B", '♥');
                else if (s.SuiteChar == '♦')
                    TopDiamond = new Card(s.Blank, false, "B", '♦');
                else if (s.SuiteChar == '♣')
                    TopClub = new Card(s.Blank, false, "B", '♣');
                else if (s.SuiteChar == '♠')
                    TopSpade = new Card(s.Blank, false, "B", '♠');

                _allCards.Add(new Card(s.Ace, false, "A", s.SuiteChar));
                _allCards.Add(new Card(s.Two, false, "2", s.SuiteChar));
                _allCards.Add(new Card(s.Three, false, "3", s.SuiteChar));
                _allCards.Add(new Card(s.Four, false, "4", s.SuiteChar));
                _allCards.Add(new Card(s.Five, false, "5", s.SuiteChar));
                _allCards.Add(new Card(s.Six, false, "6", s.SuiteChar));
                _allCards.Add(new Card(s.Seven, false, "7", s.SuiteChar));
                _allCards.Add(new Card(s.Eight, false, "8", s.SuiteChar));
                _allCards.Add(new Card(s.Nine, false, "9", s.SuiteChar));
                _allCards.Add(new Card(s.Ten, false, "10", s.SuiteChar));
                _allCards.Add(new Card(s.Jack, false, "J", s.SuiteChar));
                _allCards.Add(new Card(s.Queen, false, "Q", s.SuiteChar));
                _allCards.Add(new Card(s.King, false, "K", s.SuiteChar));
            }

            RandomizeCardList(_allCards);

            var unusedCards = new List<Card>(_allCards);

            for (int i = 1; i < 8; i++)
            {
                var stack = new CardStack();
                for (int j = 0; j < i; j++)
                {
                    stack.Add(unusedCards[^1]);
                    unusedCards.RemoveAt(unusedCards.Count - 1);
                }
                stack[^1].Revealed = true;
                Stacks.Add(stack);
            }

            CardDeck = unusedCards;
        }

        static void RandomizeCardList(List<Card> list)
        {
            var random = new Random();
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }

        public void FlipNextDeckCard()
        {
            UserMessage = string.Empty;
            if (CardDeck.Count > 0)
            {
                if (RevealedDeckCard != null)
                {
                    DiscardedCardDeck.Add(RevealedDeckCard);
                    RevealedDeckCard.Revealed = false;
                }

                RevealedDeckCard = CardDeck[^1];
                CardDeck.RemoveAt(CardDeck.Count - 1);
            }
            else
            {
                if (RevealedDeckCard != null)
                {
                    CardDeck.Add(RevealedDeckCard);
                    RevealedDeckCard = null;
                }

                for (int i = DiscardedCardDeck.Count - 1; i >= 0; i--)
                {
                    CardDeck.Add(DiscardedCardDeck[i]);
                    DiscardedCardDeck.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="firstCardNumber"></param>
        /// <param name="firstCardSuite">Should be ♥, ♦, ♣, ♠, or X.</param>
        /// <param name="secondCardNumber"></param>
        /// <param name="secondCardSuite"></param>
        public void MoveCard(string firstCardNumber, char firstCardSuite, string secondCardNumber, char secondCardSuite)
        {
            firstCardNumber = firstCardNumber.ToUpper();
            secondCardNumber = secondCardNumber.ToUpper();
            UserMessage = IsMoveLegal(firstCardNumber, firstCardSuite, secondCardNumber, secondCardSuite);
            if (!string.IsNullOrEmpty(UserMessage))
                return;

            // Find second card
            CardStack? destCardStack = null;

            foreach (var s in Stacks)
            {
                if (s.Count == 0 && secondCardSuite == 'X')
                {
                    destCardStack = s;
                    break;
                }
                else if (s.Count > 0)
                {
                    if (s[^1].Number == secondCardNumber && s[^1].Suite == secondCardSuite)
                    {
                        destCardStack = s; 
                        break;
                    }
                }
            }

            if (destCardStack == null)
            {
                UserMessage = "MOVE FAILED - The destination card is not available.";
                return;
            }


            // Find first card

            if (RevealedDeckCard != null && RevealedDeckCard.Suite == firstCardSuite && RevealedDeckCard.Number == firstCardNumber)
            {
                RevealedDeckCard.Revealed = true;
                destCardStack.Add(RevealedDeckCard);
                if (DiscardedCardDeck.Count > 0)
                {
                    RevealedDeckCard = DiscardedCardDeck[^1];
                    DiscardedCardDeck.RemoveAt(DiscardedCardDeck.Count - 1);
                }
                else
                    RevealedDeckCard = null;
            }
            else
            {
                bool foundCard = false;
                foreach (var s in Stacks)
                {
                    if (s.Count > 0)
                    {
                        for (int i = 0; i < s.Count; i++)
                        {
                            if (s[i].Revealed && s[i].Number == firstCardNumber && s[i].Suite == firstCardSuite)
                            {
                                if (i > 0)
                                    s[i - 1].Revealed = true;
                                for (int j = i; j < s.Count; j++)
                                    destCardStack.Add(s[j]);
                                while (s.Count > 0 
                                    && (s[^1].Number != firstCardNumber || s[^1].Suite != firstCardSuite))
                                    s.RemoveAt(s.Count - 1);
                                if (s.Count > 0)
                                    s.RemoveAt(s.Count - 1);
                                foundCard = true;
                                break;
                            }
                        }
                        if (foundCard)
                            break;
                    }
                }
                if (!foundCard)
                    UserMessage = "MOVE FAILED - Couldn't find first card.";
            }

        }

        private string IsMoveLegal(string firstCardNumber, char firstCardSuite, string secondCardNumber, char secondCardSuite)
        {
            var firstCardRed = firstCardSuite == '♥' || firstCardSuite == '♦';
            var secondCardRed = secondCardSuite == '♥' || secondCardSuite == '♦';

            if (((firstCardRed && secondCardRed) 
                || (!firstCardRed && !secondCardRed))
                && (secondCardSuite != 'X'))
                return "MOVE FAILED - Both cards are of the same color.";

            var firstCardNum = GetCardNum(firstCardNumber);
            var secondCardNum = GetCardNum(secondCardNumber);

            if (firstCardNum != secondCardNum - 1 && (firstCardNum != 13 || secondCardNum != -1))
                return "MOVE FAILED - The specified card can't be moved onto the destination.";

            return string.Empty;
        }

        private int GetCardNum(string cardNumber)
        {
            var cardNum = 0;
            switch (cardNumber.ToUpper())
            {
                case "A":
                    cardNum = 1;
                    break;
                case "J":
                    cardNum = 11;
                    break;
                case "Q":
                    cardNum = 12;
                    break;
                case "K":
                    cardNum = 13;
                    break;
                case "B": // B is the blank
                    cardNum = 0;
                    break;
                default:
                    if (!int.TryParse(cardNumber, out cardNum))
                        cardNum = -1;
                    break;
            }

            return cardNum;
        }

        public void Pack(string cardNum, char cardSuite)
        {
            UserMessage = string.Empty;
            cardNum = cardNum.ToUpper();
            if (RevealedDeckCard != null && RevealedDeckCard.Number == cardNum && RevealedDeckCard.Suite == cardSuite)
            {
                // We're packing the revealed deck card
                switch(cardSuite)
                {
                    case '♥':
                        Card? topHeart = TopHeart;
                        AssignIfRevealedDeckCard(ref topHeart, cardNum);
                        TopHeart = topHeart;
                        break;
                    case '♦':
                        Card? topDiamond = TopDiamond;
                        AssignIfRevealedDeckCard(ref topDiamond, cardNum);
                        TopDiamond = topDiamond;
                        break;
                    case '♣':
                        Card? topClub = TopClub;
                        AssignIfRevealedDeckCard(ref topClub, cardNum);
                        TopClub = topClub;
                        break;
                    case '♠':
                        Card? topSpade = TopSpade;
                        AssignIfRevealedDeckCard(ref topSpade, cardNum);
                        TopSpade = topSpade;
                        break;
                }
            }
            else
            {
                var foundCard = false;
                foreach (var s in Stacks)
                {
                    if (s.Count == 0) continue;

                    if (s[^1].Number == cardNum && s[^1].Suite == cardSuite)
                    {
                        foundCard = true;
                        switch (s[^1].Suite)
                        {
                            case '♥':
                                if ((TopHeart == null && GetCardNum(cardNum) != 1) 
                                    || (TopHeart != null && GetCardNum(cardNum) != GetCardNum(TopHeart.Number) + 1))
                                {
                                    UserMessage = "PACK FAILED - Card given is not next in sequence.";
                                    break;
                                }
                                TopHeart = s[^1];
                                break;
                            case '♦':
                                if ((TopDiamond == null  && GetCardNum(cardNum) != 1)
                                    || (TopDiamond != null && GetCardNum(cardNum) != GetCardNum(TopDiamond.Number) + 1))
                                {
                                    UserMessage = "PACK FAILED - Card given is not next in sequence.";
                                    break;
                                }
                                TopDiamond = s[^1];
                                break;
                            case '♣':
                                if ((TopClub == null && GetCardNum(cardNum) != 1)
                                    || (TopClub != null && GetCardNum(cardNum) != GetCardNum(TopClub.Number) + 1))
                                {
                                    UserMessage = "PACK FAILED - Card given is not next in sequence.";
                                    break;
                                }
                                TopClub = s[^1];
                                break;
                            case '♠':
                                if ((TopSpade == null && GetCardNum(cardNum) != 1)
                                    || (TopSpade != null && GetCardNum(cardNum) != GetCardNum(TopSpade.Number) + 1))
                                {
                                    UserMessage = "PACK FAILED - Card given is not next in sequence.";
                                    break;
                                }
                                TopSpade = s[^1];
                                break;
                        }
                        if (string.IsNullOrEmpty(UserMessage))
                            s.RemoveAt(s.Count - 1);
                        if (s.Count > 0)
                            s[^1].Revealed = true;
                    }
                    if (foundCard)
                        break;
                }
                if (!foundCard)
                    UserMessage = "PACK FAILED - Given card is not available.";
            }
            CheckForCompletion();
        }

        private void AssignIfRevealedDeckCard(ref Card? TopSuiteCard, string cardNum)
        {
            if ((TopSuiteCard == null && GetCardNum(cardNum) != 1) 
                || (TopSuiteCard != null && GetCardNum(TopSuiteCard.Number) != GetCardNum(cardNum) - 1))
            {
                UserMessage = "PACK FAILED - Packed card is not the next card in sequence.";
                return;
            }
            else
            {
                TopSuiteCard = RevealedDeckCard;
                if (DiscardedCardDeck.Count > 0)
                {
                    RevealedDeckCard = DiscardedCardDeck[^1];
                    DiscardedCardDeck.RemoveAt(DiscardedCardDeck.Count - 1);
                }
                else
                    RevealedDeckCard = null;
            }
        }

        private void CheckForCompletion()
        {
            if (TopHeart != null && TopHeart.Number == "K"
                && TopDiamond != null && TopDiamond.Number == "K"
                && TopClub != null && TopClub.Number == "K"
                && TopSpade != null && TopSpade.Number == "K"
                && DiscardedCardDeck.Count == 0
                && CardDeck.Count == 0)
                UserMessage = "CONGRATULATIONS! You have completed this solitaire game.";
        }
    }
}
