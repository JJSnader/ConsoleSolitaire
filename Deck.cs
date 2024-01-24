
namespace ConsoleSolitaire
{
    internal class Deck
    {
        public const int Card_Width = 13;
        public const int Card_Height = 11;

        public const int Display_Height = 46;
        public const int Display_Width = 99;

        private readonly List<Card> _allCards = [];
        private readonly List<string> _helpText = [
            "Flip - Show next card in deck | Move 10S JH - Move 10 of Spades to Jack of Hearts | Help ",
            "Pack AH - Move Ace of Hearts to Hearts pile | Exit - Exit game | New - Start new game    "
        ];

        public string UserMessage = string.Empty;

        public Mode GameMode = Mode.Single;

        public List<string> ColorMap = [];

        public List<CardStack> Stacks { get; set; }

        public List<Card> CardDeck { get; set; }

        public List<Card> DiscardedCardDeck { get; set; }

        public RevealedCardStack RevealedCardDeck { get; set; }

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
        public CardBack CurrentCardBack { get; set; }

        private Card? _topSpade;
        private Card? _topHeart;
        private Card? _topDiamond;
        private Card? _topClub;

        private CardBacks _cb;

        private Card CardBack { get; set; }
        private Card EmptyDeck { get; set; }

        public Deck() 
        {
            _allCards = [];
            Stacks = [];
            CardDeck = [];
            DiscardedCardDeck = [];
            _cb = new();
            RevealedCardDeck = new();
            CurrentCardBack = ConsoleSolitaire.CardBack.Standard;
            CardBack = new Card(_cb.Standard, false, "0", 'X');
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
            ColorMap = new List<string>(Display_Height);
            for (int i = 0; i < Display_Height; i++)
                ColorMap.Add("".PadLeft(Display_Width, 'G'));
            
            var result = new List<string>(Display_Height);
            for (int i = 0; i < Display_Height; ++i)
                result.Add("".PadLeft(Display_Width));

            if (CardDeck.Count > 0)
                DrawSubDisplay(0, 0, 
                    CardBack.CardDisplay, result, 
                    CardBack.CardColorMap, ColorMap);
            else
                DrawSubDisplay(0, 0, 
                    EmptyDeck.CardDisplay, result, 
                    EmptyDeck.CardColorMap, ColorMap);

            if (RevealedCardDeck != null && RevealedCardDeck.Count > 0)
                DrawSubDisplay(0, Card_Width + 1, 
                    RevealedCardDeck.GetDisplay(), result, 
                    RevealedCardDeck.GetColorMap(), ColorMap);
            else
                DrawSubDisplay(0, Card_Width + 1, 
                    EmptyDeck.CardDisplay, result,
                    EmptyDeck.CardColorMap, ColorMap);

            DrawSubDisplay(0, (Card_Width * 3) + 3, 
                TopSpade == null ? EmptyDeck.CardDisplay : TopSpade.CardDisplay, result,
                TopSpade == null ? EmptyDeck.CardColorMap : TopSpade.CardColorMap, ColorMap);
            DrawSubDisplay(0, (Card_Width * 4) + 4,
                TopHeart == null ? EmptyDeck.CardDisplay : TopHeart.CardDisplay, result,
                TopHeart == null ? EmptyDeck.CardColorMap : TopHeart.CardColorMap, ColorMap);
            DrawSubDisplay(0, (Card_Width * 5) + 5,
                TopDiamond == null ? EmptyDeck.CardDisplay : TopDiamond.CardDisplay, result,
                TopDiamond == null ? EmptyDeck.CardColorMap : TopDiamond.CardColorMap, ColorMap);
            DrawSubDisplay(0, (Card_Width * 6) + 6, 
                TopClub == null ? EmptyDeck.CardDisplay : TopClub.CardDisplay, result,
                TopClub == null ? EmptyDeck.CardColorMap : TopClub.CardColorMap, ColorMap);

            for (int i = 0; i < Stacks.Count; ++i)
                DrawSubDisplay(Card_Height + 2, (Card_Width * i) + i, 
                    Stacks[i].GetDisplay(), result,
                    Stacks[i].GetColorMap(), ColorMap);

            result[^3] = _helpText[0].PadRight(Display_Width);
            result[^2] = _helpText[1].PadRight(Display_Width);
            result[^1] = UserMessage.PadRight(Display_Width);

            for (int i = 0; i < result.Count; ++i)
                result[i] = result[i].PadRight(Display_Width);

            return result;
        }

        static void DrawSubDisplay(int topX, int topY, List<string> subDisplay, List<string> display, List<string> subColorMap, List<string> colorMap)
        {
            for (int x = topX; x < (topX + subDisplay.Count); ++x)
            {
                if (x >= display.Count)
                    break;
                display[x] = display[x].Insert(topY, subDisplay[x - topX]);
                display[x] = display[x].Remove(topY + subDisplay[x - topX].Length, subDisplay[x - topX].Length);

                colorMap[x] = colorMap[x].Insert(topY, subColorMap[x - topX]);
                colorMap[x] = colorMap[x].Remove(topY + subColorMap[x - topX].Length, subColorMap[x - topX].Length);
            }
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
            RevealedCardDeck ??= new();
            if (CardDeck.Count > 0)
            {
                if (RevealedCardDeck != null && RevealedCardDeck.Count > 0)
                {
                    foreach (var card in RevealedCardDeck.Cards)
                    {
                        card.Revealed = false;
                        DiscardedCardDeck.Add(card);
                    }
                    RevealedCardDeck.Clear();
                }

                int revealCount = (int)GameMode;
                for (int i = 0; i < revealCount; i++)
                {
                    if (CardDeck.Count > 0)
                    {
                        RevealedCardDeck.Add(CardDeck[^1]);
                        CardDeck.RemoveAt(CardDeck.Count - 1);
                    }
                }
                
            }
            else
            {
                if (RevealedCardDeck.Count > 0)
                {
                    for (int i = RevealedCardDeck.Count - 1; i >= 0; i--)
                    {
                        CardDeck.Add(RevealedCardDeck[i]);
                    }
                    RevealedCardDeck.Clear();
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

            if (RevealedCardDeck != null && RevealedCardDeck.Count > 0 && RevealedCardDeck.Top.Suite == firstCardSuite && RevealedCardDeck.Top.Number == firstCardNumber)
            {
                RevealedCardDeck.Top.Revealed = true;
                destCardStack.Add(RevealedCardDeck.Top);
                RevealedCardDeck.Cards.Remove(RevealedCardDeck.Top);
                if (DiscardedCardDeck.Count > 0 && GameMode == Mode.Single)
                {
                    RevealedCardDeck.Add(DiscardedCardDeck[^1]);
                    DiscardedCardDeck.RemoveAt(DiscardedCardDeck.Count - 1);
                }
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

        public void Pack(string cardNum, char cardSuite)
        {
            UserMessage = string.Empty;
            cardNum = cardNum.ToUpper();
            if (RevealedCardDeck != null && RevealedCardDeck.Count > 0 && RevealedCardDeck.Top.Number == cardNum && RevealedCardDeck.Top.Suite == cardSuite)
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

        public void AutoPack()
        {
            var packedCard = true;
            while (packedCard)
            {
                packedCard = false;
                if (RevealedCardDeck.Count > 0 && IsCardNextOnStack(RevealedCardDeck.Top))
                {
                    Pack(RevealedCardDeck.Top.Number, RevealedCardDeck.Top.Suite);
                    packedCard = true;
                }
                else
                {
                    foreach (var s in Stacks)
                    {
                        if (s.Count == 0) continue;

                        if (IsCardNextOnStack(s[^1]))
                        {
                            Pack(s[^1].Number, s[^1].Suite);
                            packedCard = true;
                        }
                    }
                }
            }
        }

        public void ChangeMode(Mode newMode)
        {
            GameMode = newMode;
            if (RevealedCardDeck.Count > 0)
            {
                for (int i = RevealedCardDeck.Count - 1; i >= 0; i--)
                {
                    CardDeck.Add(RevealedCardDeck[i]);
                    RevealedCardDeck.Cards.RemoveAt(i);
                }
                for (int i = 0; i < (int)GameMode; i++)
                {
                    if (CardDeck.Count > 0)
                    {
                        RevealedCardDeck.Add(CardDeck[^1]);
                        CardDeck.RemoveAt(CardDeck.Count - 1);
                    }
                }
            }
        }

        public void ChangeCardBack(CardBack newBack)
        {
            CurrentCardBack = newBack;
            CardBack = new Card(_cb.GetCardBack(newBack), false, "0", 'X');
        }

        #region Private

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

        private bool IsCardNextOnStack(Card card)
        {
            switch (card.Suite)
            {
                case '♥':
                    return IsCardNextOnStack(TopHeart, card.Number);
                case '♦':
                    return IsCardNextOnStack(TopDiamond, card.Number);
                case '♣':
                    return IsCardNextOnStack(TopClub, card.Number);
                case '♠':
                    return IsCardNextOnStack(TopSpade, card.Number);
            }
            return false;
        }

        private bool IsCardNextOnStack(Card? TopSuiteCard, string cardNum)
        {
            return (TopSuiteCard == null && GetCardNum(cardNum) == 1)
                || (TopSuiteCard != null && GetCardNum(TopSuiteCard.Number) == GetCardNum(cardNum) - 1);
        }

        private void AssignIfRevealedDeckCard(ref Card? TopSuiteCard, string cardNum)
        {
            if (!IsCardNextOnStack(TopSuiteCard, cardNum))
            {
                UserMessage = "PACK FAILED - Packed card is not the next card in sequence.";
                return;
            }
            else
            {
                TopSuiteCard = RevealedCardDeck.Top;
                RevealedCardDeck.Cards.Remove(RevealedCardDeck.Top);
                if (DiscardedCardDeck.Count > 0 && RevealedCardDeck.Count == 0)
                {
                    RevealedCardDeck.Add(DiscardedCardDeck[^1]);
                    DiscardedCardDeck.RemoveAt(DiscardedCardDeck.Count - 1);
                }
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

        #endregion
    }

    internal enum Mode
    {
        Single = 1,
        Double = 2,
        Triple = 3
    }
}
