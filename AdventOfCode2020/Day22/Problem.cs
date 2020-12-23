using AdventOfCode2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2020.Day22
{
    public class Problem
    {
        public static void Solve()
        {
            var actual = InputParser.GetLines("daytwentytwo.txt");
            var test = InputParser.GetLines("daytwentytwo-test.txt");
            var infinite = InputParser.GetLines("daytwentytwo-infinite.txt");
            var (playerOneDeck, playerTwoDeck) = DealDecks(actual);
            bool didPlayerOneWin = PlayNewGame(playerOneDeck, playerTwoDeck);
            if (didPlayerOneWin)
            {
                Console.WriteLine($"Player One won! Score: {playerOneDeck.CalculateScore()}");
            }
            else
            {
                Console.WriteLine($"Player Two won! Score: {playerTwoDeck.CalculateScore()}");
            }
        }

        static int GameNumber = 0;

        class Card
        {
            public int Value { get; set; }
        }

        class Deck
        {
            private List<Card> _cards = new List<Card>();
            public IEnumerable<Card> Cards => _cards;

            public Deck(List<Card> cards)
            {
                _cards = cards;
            }

            public Card Draw()
            {
                Card card = Cards.First();
                _cards.Remove(Cards.First());
                return card;
            }

            public void AddToBottom(Card card)
            {
                _cards.Add(card);
            }

            public long CalculateScore()
            {
                List<Card> finalCards = _cards;
                finalCards.Reverse();
                long result = 0;
                int multiplier = 1;

                foreach (Card card in finalCards)
                {
                    result += card.Value * multiplier;
                    multiplier++;
                }

                return result;
            }

            public Deck CopyOf()
            {
                return new Deck(Cards.ToList());
            }

            public string Print()
            {
                return string.Join(',', Cards.Select(c => c.Value.ToString()).ToList());
            }

            public override bool Equals(object obj)
            {
                var deck = obj as Deck;

                if (deck is null)
                    return false;

                if (deck.Cards.Count() != Cards.Count())
                    return false;

                for (int i = 0; i < Cards.Count(); i++)
                {
                    Card thisCard = Cards.ElementAt(i);
                    Card otherCard = deck.Cards.ElementAt(i);
                    if (otherCard.Value != thisCard.Value)
                        return false;
                }

                return true;
            }

            public override int GetHashCode()
            {
                return Cards.Aggregate(0, (prev, next) => prev.GetHashCode() + next.GetHashCode());
            }
        }

        private static bool PlayNewGame(Deck playerOneDeck, Deck playerTwoDeck)
        {
            GameNumber++;
            var gameNum = GameNumber;
            List<(Deck, Deck)> roundTracker = new List<(Deck, Deck)>();
            List<string> previousRounds = new List<string>();
            while (playerOneDeck.Cards.Any() && playerTwoDeck.Cards.Any())
            {
                string deckConfiguration = $"{playerOneDeck.Print()} || {playerTwoDeck.Print()}";

                if (previousRounds.Any(r => r.Equals(deckConfiguration)))
                {
                    return true;
                }

                // I thought I was being clever with this, but this is just not as fast as using strings
                //if (roundTracker.Any(r => r.Item1.Equals(playerOneDeck) && r.Item2.Equals(playerTwoDeck)))
                //{
                //    return true;    // This game has entered an infinite loop; player one wins be default
                //}

                //roundTracker.Add((playerOneDeck.CopyOf(), playerTwoDeck.CopyOf()));
                previousRounds.Add($"{playerOneDeck.Print()} || {playerTwoDeck.Print()}");

                Card playerOneCard = playerOneDeck.Draw();
                Card playerTwoCard = playerTwoDeck.Draw();

                if (playerOneDeck.Cards.Count() >= playerOneCard.Value && playerTwoDeck.Cards.Count() >= playerTwoCard.Value)
                {
                    List<Card> playerOneSubCards = new List<Card>();
                    for (int i = 0; i < playerOneCard.Value; i++)
                        playerOneSubCards.Add(playerOneDeck.Cards.ElementAt(i));

                    List<Card> playerTwoSubCards = new List<Card>();
                    for (int i = 0; i < playerTwoCard.Value; i++)
                        playerTwoSubCards.Add(playerTwoDeck.Cards.ElementAt(i));

                    Deck playerOneSubDeck = new Deck(playerOneSubCards);
                    Deck playerTwoSubDeck = new Deck(playerTwoSubCards);

                    var didPlayerOneWin = PlayNewGame(playerOneSubDeck, playerTwoSubDeck);

                    if (didPlayerOneWin)
                    {
                        playerOneDeck.AddToBottom(playerOneCard);
                        playerOneDeck.AddToBottom(playerTwoCard);
                    }
                    else
                    {
                        playerTwoDeck.AddToBottom(playerTwoCard);
                        playerTwoDeck.AddToBottom(playerOneCard);
                    }
                }
                else if (playerOneCard.Value > playerTwoCard.Value)
                {
                    playerOneDeck.AddToBottom(playerOneCard);
                    playerOneDeck.AddToBottom(playerTwoCard);
                }
                else if (playerTwoCard.Value > playerOneCard.Value)
                {
                    playerTwoDeck.AddToBottom(playerTwoCard);
                    playerTwoDeck.AddToBottom(playerOneCard);
                }
                else
                {
                    Console.WriteLine("Combat was a tie");
                }
            }

            return playerOneDeck.Cards.Any();
        }

        private static (Deck, Deck) DealDecks(string[] input)
        {
            List<Card> playerOneCards = new List<Card>();
            List<Card> playerTwoCards = new List<Card>();
            bool isPlayer1 = true;

            foreach (string line in input)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue; 

                if (line.Contains("Player 1"))
                    continue;

                if (line.Contains("Player 2"))
                {
                    isPlayer1 = false;
                    continue;
                }

                if (isPlayer1)
                {
                    playerOneCards.Add(new Card { Value = int.Parse(line) });
                }
                else
                {
                    playerTwoCards.Add(new Card { Value = int.Parse(line) });
                }
            }

            Deck playerOneDeck = new Deck(playerOneCards);
            Deck playerTwoDeck = new Deck(playerTwoCards);

            return (playerOneDeck, playerTwoDeck);
        }
    }
}
