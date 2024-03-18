using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSolitaire
{
    internal class AutoSolve
    {
        private Deck _d;

        private string filepath;

        public bool Solved { get; set; }

        public AutoSolve(Deck d)
        {
            _d = d;
            filepath = System.AppDomain.CurrentDomain.BaseDirectory;
            if (!filepath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                filepath += Path.DirectorySeparatorChar;
            filepath += "log.txt";
            Solved = false;
        }

        public List<string> Solve()
        {

            var noMovement = 0;
            var lastDeckCount = _d.CardDeck.Count + _d.RevealedCardDeck.Count + _d.DiscardedCardDeck.Count;
            var deckCountUnchanged = 0;
            while (noMovement < 10 && deckCountUnchanged < 24 && !_d.UserMessage.Contains("CONGRATULATIONS"))
            {
                var movedCard = false;

                foreach (var s in _d.Stacks)
                {
                    if (s.Count == 0) continue;

                    var bottom = s[s.BottomRevealedCardIndex];
                    if (_d.GetCardNum(bottom.Number) == 1)
                    {
                        Log($"Pack {bottom.Number}{bottom.Suite}");
                        _d.Pack(bottom.Number, bottom.Suite);
                        movedCard = true;
                    }
                    else if (_d.GetCardNum(bottom.Number) == 13)
                    {
                        foreach (var s1 in _d.Stacks)
                        {
                            if (s == s1) continue;
                            if (s1.Count > 0) continue;

                            Log($"Move {bottom.Number}{bottom.Suite} X");
                            _d.MoveCard(bottom.Number, bottom.Suite, "X", 'X');
                            movedCard = true;
                            break;
                        }
                    }
                    else
                    {
                        foreach (var s1 in _d.Stacks)
                        {
                            if (s1 == s) continue;
                            if (s1.Count == 0) continue;

                            var s1Bottom = s1[s1.BottomRevealedCardIndex];

                            if (_d.IsOppositeSuite(s1Bottom.Suite, bottom.Suite) && _d.GetCardNum(s1Bottom.Number) == _d.GetCardNum(bottom.Number) - 1)
                            {
                                Log($"Move {s1Bottom.Number}{s1Bottom.Suite} {bottom.Number}{bottom.Suite}");
                                _d.MoveCard(s1Bottom.Number, s1Bottom.Suite, bottom.Number, bottom.Suite);
                                if (string.IsNullOrEmpty(_d.UserMessage))
                                {
                                    movedCard = true;
                                    break;
                                }
                            }

                            if (!movedCard)
                            {
                                if (_d.IsOppositeSuite(s1[^1].Suite, bottom.Suite) 
                                    && _d.GetCardNum(s1[^1].Number) - 1 == _d.GetCardNum(bottom.Number))
                                {
                                    Log($"Move {bottom.Number}{bottom.Suite} {s1[^1].Number}{s1[^1].Suite}");
                                    _d.MoveCard(bottom.Number, bottom.Suite, s1[^1].Number, s1[^1].Suite);
                                    if (string.IsNullOrEmpty(_d.UserMessage))
                                    {
                                        movedCard = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (!movedCard)
                        {
                            Log("Flip");
                            _d.FlipNextDeckCard();
                            if (_d.RevealedCardDeck.Count == 0)
                            {
                                Log("Flip");
                                _d.FlipNextDeckCard();
                            }
                            if (_d.RevealedCardDeck.Count == 0)
                            {
                                Log("Flip");
                                _d.FlipNextDeckCard(); 
                            }
                            if (_d.RevealedCardDeck.Count > 0)
                            {
                                var deckCard = _d.RevealedCardDeck[0];

                                if (_d.IsOppositeSuite(bottom.Suite, deckCard.Suite) && _d.GetCardNum(bottom.Number) - 1 == _d.GetCardNum(deckCard.Number))
                                {
                                    Log($"Move {deckCard.Number}{deckCard.Suite} {bottom.Number}{bottom.Suite}");
                                    _d.MoveCard(deckCard.Number, deckCard.Suite, bottom.Number, bottom.Suite);
                                    if (string.IsNullOrEmpty(_d.UserMessage))
                                    {
                                        movedCard = true;
                                    }
                                }
                                else if (_d.IsCardNextOnStack(deckCard))
                                {
                                    Log($"Pack {deckCard.Number}{deckCard.Suite}");
                                    _d.Pack(deckCard.Number, deckCard.Suite);
                                    if (string.IsNullOrEmpty(_d.UserMessage))
                                        movedCard = true;
                                }
                            }
                        }
                    }
                }

                if (!movedCard)
                {
                    noMovement++;
                    if (lastDeckCount != _d.CardDeck.Count + _d.RevealedCardDeck.Count + _d.DiscardedCardDeck.Count)
                    {
                        lastDeckCount = _d.CardDeck.Count + _d.RevealedCardDeck.Count + _d.DiscardedCardDeck.Count;
                    }
                    else
                    {
                        deckCountUnchanged++;
                    }
                }
                else
                {
                    noMovement = 0;
                    deckCountUnchanged = 0;
                }
            }

            return _d.GetDisplay();
        }

        private void Log(string message)
        {
            File.AppendAllText(filepath, message + "\r\n");
        }
    }
}
