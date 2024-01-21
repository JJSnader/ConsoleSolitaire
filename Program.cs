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

            var foreColor = ConsoleColor.Black;
            var backColor = ConsoleColor.DarkGreen;

            Console.ForegroundColor = foreColor;
            Console.BackgroundColor = backColor;

            var deck = new Deck();
            PrintDisplay(deck, backColor, foreColor);

            string? input;
            do
            {
                input = Console.ReadLine();
                while (string.IsNullOrEmpty(input))
                    input = Console.ReadLine();

                var inputArray = input.Split(' ');

                var isHelp = false;
                switch (inputArray[0].ToUpper())
                {
                    case "F":
                    case "FLIP":
                        deck.FlipNextDeckCard();
                        break;
                    case "N":
                    case "NEW":
                        deck = new Deck();
                        break;
                    case "H":
                    case "HELP":
                        Console.Clear();
                        isHelp = true;
                        break;
                    case "BC":
                    case "BACKCOLOR":
                        if (inputArray.Length < 2)
                            deck.UserMessage = "You must provide a color to switch to.";
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
                        break;
                    case "M":
                    case "MOVE":
                        if (inputArray.Length < 3)
                            deck.UserMessage = "Move must have a source card and a destination card.";
                        
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
                    case "P":
                    case "PACK":
                        if (inputArray.Length < 2)
                            deck.UserMessage = "You must specify a card to pack.";

                        var cardNum = inputArray[1].Substring(0, inputArray[1].Length - 1);
                        var cardSuiteRaw = inputArray[1][^1];
                        var cardSuite = GetSuiteChar(cardSuiteRaw.ToString());
                        deck.Pack(cardNum, cardSuite);
                        break;

                }

                if (isHelp)
                    PrintHelpText();
                else
                    PrintDisplay(deck, backColor, foreColor);

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

        static void PrintDisplay(Deck d, ConsoleColor backColor, ConsoleColor foreColor)
        {
            Console.Clear();
            var lines = d.GetDisplay();
            for (int i = 0; i < lines.Count; i++)
            {
                var currentColor = 'z';
                var text = "";
                for (int j = 0; j < lines[i].Length; j++)
                {
                    if (d.ColorMap[i][j] != currentColor)
                    {
                        if (!string.IsNullOrEmpty(text))
                        {
                            Console.Write(text);
                            text = string.Empty;
                        }

                        currentColor = d.ColorMap[i][j];
                        switch (currentColor)
                        {
                            case 'G':
                                Console.BackgroundColor = backColor;
                                Console.ForegroundColor = foreColor;
                                break;
                            case 'W':
                                Console.BackgroundColor = ConsoleColor.White;
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;
                            case 'R':
                                Console.BackgroundColor = ConsoleColor.White;
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                        }
                    }
                    text += lines[i][j].ToString();
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
                "New       : Start a new game.",
                "Flip      : Show the next card in the deck, or restack the discarded cards.",
                "Move      : Move an available card to a different card. ",
                "            To move a King to an empty spot, use \"move KS X\".",
                "            Example: \"move 10S JH\" would move the 10 of Spades to the Jack of Hearts.",
                "Pack      : Moves the specified card to its suite's final pile. ",
                "BackColor : Change the background color. Valid options: blue, green, red, gray, yellow, black.",
                "Exit      : Exit the game. ",
                "Quit      : Same as Exit. ",
                "Help      : Print this prompt. ",
                " ",
                "Type Return to return to your game."
            ];

            foreach (var h in helps)
                Console.WriteLine(h);
        }
    }
}
