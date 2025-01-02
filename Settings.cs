using Newtonsoft.Json;

namespace ConsoleSolitaire
{
    internal class Settings
    {
        public Mode GameMode 
        {
            get 
            {
                if (gameMode != null)
                    return (Mode)gameMode;

                switch (_json.GameMode)
                {
                    case "TRIPLE":
                        gameMode = Mode.Triple;
                        break;
                    case "DOUBLE":
                        gameMode = Mode.Double;
                        break;
                    case "SINGLE":
                    default:
                        gameMode = Mode.Single;
                        break;
                }
                return (Mode)gameMode;
            } 
            set
            {
                gameMode = value;

                _json.GameMode = gameMode?.ToString().ToUpper();
                SaveSettings();
            }
        }

        public CardBack CardBack
        {
            get
            {
                if (cardBack != null)
                    return (CardBack)cardBack;

                switch (_json.CardBack)
                {
                    case "4":
                        cardBack = CardBack.Diamond;
                        break;
                    case "3":
                        cardBack = CardBack.Framed;
                        break;
                    case "2":
                        cardBack = CardBack.Clean;
                        break;
                    case "1":
                        cardBack = CardBack.Standard;
                        break;
                }

                return cardBack ?? CardBack.Standard;
            }
            set
            {
                cardBack = value;

                _json.CardBack = ((int)(cardBack ?? CardBack.Standard)).ToString().ToUpper();
                SaveSettings();
            }
        }

        public ConsoleColor BackColor 
        { 
            get
            {
                if (backColor != null)
                    return (ConsoleColor)backColor;

                switch (_json.BackColor)
                {
                    case "BLUE":
                        backColor = ConsoleColor.DarkBlue;
                        break;
                    case "RED":
                        backColor = ConsoleColor.DarkRed;
                        break;
                    case "GRAY":
                        backColor = ConsoleColor.DarkGray;
                        break;
                    case "YELLOW":
                        backColor = ConsoleColor.DarkYellow;
                        break;
                    case "BLACK":
                        backColor = ConsoleColor.Black;
                        break;
                    case "GREEN":
                    default:
                        backColor = ConsoleColor.DarkGreen;
                        break;
                }

                return backColor ?? ConsoleColor.DarkGreen;
            }
            set
            {
                backColor = value;

                switch (backColor)
                {
                    case ConsoleColor.DarkBlue:
                        _json.BackColor = "BLUE";
                        break;
                    case ConsoleColor.DarkRed:
                        _json.BackColor = "RED";
                        break;
                    case ConsoleColor.DarkGray:
                        _json.BackColor = "GRAY";
                        break;
                    case ConsoleColor.DarkYellow:
                        _json.BackColor = "YELLOW";
                        break;
                    case ConsoleColor.Black:
                        _json.BackColor = "BLACK";
                        break;
                    case ConsoleColor.DarkGreen:
                    default:
                        _json.BackColor = "GREEN";
                        break;
                }
                SaveSettings();
            }
        }

        public List<double> CompletionTimes
        {
            get => _json.CompletionTimes ??= [];
        }

        private Mode? gameMode;
        private CardBack? cardBack;
        private ConsoleColor? backColor;
        private SettingsJson _json;
        private string filepath;

        public Settings()
        {
            filepath = System.AppDomain.CurrentDomain.BaseDirectory;
            if (!filepath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                filepath += Path.DirectorySeparatorChar;
            filepath += "settings.json";
            _json = new SettingsJson();

            var validSettings = false;

            if (File.Exists(filepath))
            {
                string raw = File.ReadAllText(filepath);
                if (!string.IsNullOrEmpty(raw))
                {
                    var json = JsonConvert.DeserializeObject<SettingsJson>(raw);
                    if (json != null)
                    {
                        _json = json;
                        validSettings = true;
                    }
                }
            }

            if (!validSettings)
            {
                _json = new SettingsJson()
                {
                    BackColor = "GREEN",
                    GameMode = "SINGLE",
                    CardBack = "1",
                    CompletionTimes = [],
                };

                SaveSettings();
            }

        }

        public void SaveSettings()
        {
            File.WriteAllText(filepath, JsonConvert.SerializeObject(_json));
        }
    }

    internal class SettingsJson
    {
        public string? CardBack { get; set; }

        public string? BackColor { get; set; }

        public string? GameMode { get; set; }

        public List<double>? CompletionTimes { get; set; }
    }
}
