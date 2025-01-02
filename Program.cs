using System.Text;

namespace ConsoleSolitaire
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.WindowWidth = Deck.Display_Width;
            Console.WindowHeight = Deck.Display_Height;
            Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight + 1);

            var s = new Settings();
            var backColor = s.BackColor;
            var foreColor = ConsoleColor.Black;
            if (backColor == ConsoleColor.Black)
                foreColor = ConsoleColor.Gray;

            Console.ForegroundColor = foreColor;
            Console.BackgroundColor = backColor;

            var opposite = false;
            var cursed = false;

            var deck = new Deck(s);
            PrintDisplay(deck, backColor, foreColor, opposite, cursed);

            var selectingCardBack = false;

            string? input;
            do
            {
                input = Console.ReadLine();
                while (string.IsNullOrEmpty(input))
                    input = Console.ReadLine();

                var inputArray = input.Split(' ');
                var isHelp = false;
                var printStats = false;
                switch (inputArray[0].ToUpper())
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                        if (selectingCardBack)
                        {
                            selectingCardBack = false;
                            s.CardBack = (CardBack)int.Parse(inputArray[0]);
                            deck.ChangeCardBack(s.CardBack);
                        }
                        break;
                    case "A":
                    case "AUTOPACK":
                        deck.AutoPack();
                        break;
                    case "AUTOSOLVE":
                        var a = new AutoSolve(deck);
                        a.Solve();
                        break;
                    case "BC":
                    case "BACKCOLOR":
                        if (inputArray.Length < 2)
                        {
                            deck.UserMessage = "You must provide a color to switch to.";
                            break;
                        }
                        switch(inputArray[1].ToUpper())
                        {
                            case "BLUE":
                                foreColor = ConsoleColor.Black;
                                backColor = ConsoleColor.DarkBlue;
                                break;
                            case "GREEN":
                                foreColor = ConsoleColor.Black;
                                backColor = ConsoleColor.DarkGreen;
                                break;
                            case "RED":
                                foreColor = ConsoleColor.Black;
                                backColor = ConsoleColor.DarkRed;
                                break;
                            case "GRAY":
                                foreColor = ConsoleColor.Black;
                                backColor = ConsoleColor.DarkGray;
                                break;
                            case "YELLOW":
                                foreColor = ConsoleColor.Black;
                                backColor = ConsoleColor.DarkYellow;
                                break;
                            case "BLACK":
                                foreColor = ConsoleColor.Gray;
                                backColor = ConsoleColor.Black;
                                break;
                        }
                        Console.BackgroundColor = backColor;
                        s.BackColor = backColor;
                        break;
                    case "CARDBACK":
                        Console.Clear();
                        selectingCardBack = true;
                        break;
                    case "CURED":
                        cursed = false;
                        break ;
                    case "CURSED":
                        cursed = true;
                        break;
                    case "F":
                    case "FLIP":
                        deck.FlipNextDeckCard();
                        break;
                    case "N":
                    case "NEW":
                        deck = new Deck(s);
                        deck.ChangeMode(s.GameMode);
                        deck.ChangeCardBack(s.CardBack);
                        break;
                    case "H":
                    case "HELP":
                        Console.Clear();
                        isHelp = true;
                        break;
                    case "M":
                    case "MOVE":
                        if (inputArray.Length < 3)
                        {
                            deck.UserMessage = "Move must have a source card and a destination card.";
                            break;
                        }
                        
                        if (inputArray[1].Length > 3)
                            inputArray[1] = inputArray[1][0..3];
                        var firstCardNum = inputArray[1].Substring(0, inputArray[1].Length - 1);
                        var firstCardSuiteRaw = inputArray[1][^1];
                        var firstCardSuite = GetSuiteChar(firstCardSuiteRaw.ToString());
                        var secondCardNum = inputArray[2].Substring(0, inputArray[2].Length - 1);
                        var secondCardSuiteRaw = inputArray[2][^1];
                        var secondCardSuite = GetSuiteChar(secondCardSuiteRaw.ToString());

                        deck.MoveCard(firstCardNum, firstCardSuite, secondCardNum, secondCardSuite);
                        
                        break;
                    case "OPPOSITE":
                        opposite = !opposite;
                        break;
                    case "P":
                    case "PACK":
                        if (inputArray.Length < 2)
                        {
                            deck.UserMessage = "You must specify a card to pack.";
                            break;
                        }

                        var cardNum = inputArray[1].Substring(0, inputArray[1].Length - 1);
                        var cardSuiteRaw = inputArray[1][^1];
                        var cardSuite = GetSuiteChar(cardSuiteRaw.ToString());
                        deck.Pack(cardNum, cardSuite);
                        break;
                    case "MODE":
                        if (inputArray.Length < 2)
                        {
                            deck.UserMessage = "You must specify a new mode.";
                            break;
                        }

                        switch (inputArray[1].ToUpper())
                        {
                            case "S":
                            case "SINGLE":
                                s.GameMode = Mode.Single;
                                break;
                            case "D":
                            case "DOUBLE":
                                s.GameMode = Mode.Double;
                                break;
                            case "T":
                            case "TRIPLE":
                                s.GameMode = Mode.Triple;
                                break;
                        }
                        deck.UserMessage = "New mode will be applied on the next new game.";
                        break;
                    case "STATS":
                        printStats = true;
                        break;
                }

                if (isHelp)
                    PrintHelpText();
                else if (selectingCardBack)
                    PrintCardBacks();
                else if (printStats)
                    PrintStats(s);
                else
                    PrintDisplay(deck, backColor, foreColor, opposite, cursed);

            } while (input.ToUpper() != "EXIT"
                && input.ToUpper() != "E"
                && input.ToUpper() != "QUIT"
                && input.ToUpper() != "Q");
        }

        static char GetSuiteChar(string input)
        {
            if (string.IsNullOrEmpty(input))
                return 'X';

            switch (input.ToUpper()[0])
            {
                case 'H':
                    return '♥';
                case 'D':
                    return '♦';
                case 'C':
                    return '♣';
                case 'S':
                    return '♠';
            }
            return 'X';
        }

        static void PrintStats(Settings s)
        {
            Console.Clear();

            if (s.CompletionTimes.Count == 0)
            {
                Console.WriteLine("No stats to load!");
                return;
            }

            var avg = s.CompletionTimes.Average();
            var min = s.CompletionTimes.Min();

            Console.WriteLine($"Fastest completion time: {TimeSpan.FromMilliseconds(min):hh\\:mm\\:ss}");
            Console.WriteLine($"Average completion time: {TimeSpan.FromMilliseconds(avg):hh\\:mm\\:ss}");
            Console.WriteLine();
            Console.WriteLine("All completion times");
            Console.WriteLine("--------------------");
            foreach (var time in s.CompletionTimes)
            {
                Console.WriteLine($"{TimeSpan.FromMilliseconds(time):hh\\:mm\\:ss}");
            }
            Console.WriteLine();
            Console.WriteLine("Type 'return' to go back to the game.");
        }

        static void PrintDisplay(Deck d, ConsoleColor backColor, ConsoleColor foreColor, bool opposite, bool cursed)
        {
            PrintDisplayWithColorMap(d.GetDisplay(), d.ColorMap, backColor, foreColor, opposite, cursed);
        }

        static void PrintDisplayWithColorMap(List<string> display, List<string> colorMap, ConsoleColor backColor, ConsoleColor foreColor, bool opposite, bool cursed)
        {
            Console.Clear();
            var cardBackColor = ConsoleColor.White;
            var cardForeColorPrimary = ConsoleColor.Black;
            var cardForeColorSecondary = ConsoleColor.Red;
            if (cursed)
            {
                cardBackColor = ConsoleColor.Blue;
                cardForeColorPrimary = ConsoleColor.Red;
                cardForeColorSecondary = ConsoleColor.Green;
            }
            if (opposite)
                (cardForeColorPrimary, cardForeColorSecondary) = (cardForeColorSecondary, cardForeColorPrimary);

            for (int i = 0; i < display.Count; i++)
            {
                var currentColor = 'z';
                var text = "";
                for (int j = 0; j < display[i].Length; j++)
                {
                    if (colorMap[i][j] != currentColor)
                    {
                        if (!string.IsNullOrEmpty(text))
                        {
                            Console.Write(text);
                            text = string.Empty;
                        }

                        currentColor = colorMap[i][j];
                        switch (currentColor)
                        {
                            case 'G':
                                Console.BackgroundColor = backColor;
                                Console.ForegroundColor = foreColor;
                                break;
                            case 'W':
                                Console.BackgroundColor = cardBackColor;
                                Console.ForegroundColor = cardForeColorPrimary;
                                break;
                            case 'R':
                                Console.BackgroundColor = cardBackColor;
                                Console.ForegroundColor = cardForeColorSecondary;
                                break;
                        }
                    }
                    text += display[i][j].ToString();
                }
                if (!string.IsNullOrEmpty(text))
                    Console.WriteLine(text);
                else
                    Console.WriteLine();
            }
        }

        static void PrintHelpText()
        {
            List<string> helps = [
                "COMMANDS",
                "--------",
                "You can type just the capital letters of the command as a shortcut.",
                " ",
                "Autopack  : Automatically pack all cards that are able to be packed. ",
                "BackColor : Change the background color. Valid options: BLUE, GREEN, RED, GRAY, YELLOW, BLACK.",
                "CARDBACK  : Change the style of the card back. ",
                "Exit      : Exit the game. ",
                "Flip      : Show the next card in the deck, or restack the discarded cards.",
                "Help      : Print this prompt. ",
                "MODE      : How many cards are flipped at once. Valid options are Single, Double, and Triple. ",
                "Move      : Move an available card to a different card. ",
                "            To move a King to an empty spot, use \"move KS X\".",
                "            Example: \"move 10S JH\" would move the 10 of Spades to the Jack of Hearts.",
                "New       : Start a new game.",
                "Pack      : Moves the specified card to its suite's final pile. ",
                "Quit      : Same as Exit. ",
                " ",
                "Type Return to return to your game."
            ];

            foreach (var h in helps)
                Console.WriteLine(h);
        }

        static void PrintCardBacks()
        {
            List<string> display = [
                "",
                "Choose your card back by entering the number next to it.",
                "",
            ];
            List<string> colorMap = [
                "".PadLeft(Deck.Display_Width, 'G'),
                "".PadLeft(Deck.Display_Width, 'G'),
                "".PadLeft(Deck.Display_Width, 'G')
            ];

            var cb = new CardBacks();
            for (int i = 0; i < cb.Standard.Count; i++)
            {
                display.Add("".PadLeft(4));
                colorMap.Add("".PadLeft(4, 'G'));
            }

            for (int i = 0; i < cb.Standard.Count; i++)
            {
                display[i + 3] += cb.Standard[i] + "".PadLeft(4);
                colorMap[i + 3] += "".PadLeft(cb.Standard[i].Length, 'W') + "".PadLeft(4, 'G');
                display[i + 3] += cb.Clean[i] + "".PadLeft(4);
                colorMap[i + 3] += "".PadLeft(cb.Clean[i].Length, 'W') + "".PadLeft(4, 'G');
                display[i + 3] += cb.Framed[i] + "".PadLeft(4);
                colorMap[i + 3] += "".PadLeft(cb.Framed[i].Length, 'W') + "".PadLeft(4, 'G');
                display[i + 3] += cb.Diamond[i] + "".PadLeft(4);
                colorMap[i + 3] += "".PadLeft(cb.Diamond[i].Length, 'W') + "".PadLeft(4, 'G');

                for (int j = 0; j < display[i + 3].Length; j++)
                {
                    if (display[i + 3][j] == '♥' || display[i + 3][j] == '♦')
                    {
                        colorMap[i + 3] = colorMap[i + 3][..j] + 'R' + colorMap[i + 3][(j + 1)..];
                    }
                }

            }
            var cardNames = "".PadLeft(4)
                + $"{nameof(cb.Standard)} (1)".PadRight(Deck.Card_Width + 4)
                + $"{nameof(cb.Clean)} (2)".PadRight(Deck.Card_Width + 4)
                + $"{nameof(cb.Framed)} (3)".PadRight(Deck.Card_Width + 4)
                + $"{nameof(cb.Diamond)} (4)".PadRight(Deck.Card_Width + 4);

            display.Add(cardNames);
            colorMap.Add("".PadLeft(cardNames.Length, 'G'));

            PrintDisplayWithColorMap(display, colorMap, ConsoleColor.White, ConsoleColor.Black, false, false);
        }
    }
}
