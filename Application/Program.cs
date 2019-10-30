using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Players;

namespace Application
{
    class Program
    {

        private static Dictionary<short, ConsoleColor> IdToColor = new Dictionary<short, ConsoleColor>();

        static void Main(string[] args)
        {
            try
            {
                IdToColor.Add(0, ConsoleColor.White);
                IdToColor.Add(1, ConsoleColor.Cyan);
                IdToColor.Add(2, ConsoleColor.Yellow);

                string winner = "Draw";

                var winningSequence = new List<int>();

                var turnCount = 0;

                Game.Game SzemerediGame = new Game.Game();

                var firstPlayerType = PlayerType.Human;

                var secondPlayerType = GetPlayerType();


                GetInitialParametrs(SzemerediGame);

                bool initialized = SzemerediGame.Initialize();

                if (!initialized)
                {
                    Console.WriteLine("\nThere are no arithmetic sequences of given length in generated set!\n\n" +
                        "Press any key to exit...\n");
                    Console.ReadKey();
                    Environment.Exit(0);
                }

                Console.Clear();

                IPlayer firstPlayer = PlayerFactory.GetPlayer(firstPlayerType);
                firstPlayer.Id = 1;
                if (firstPlayer is HumanPlayer)
                {
                    (firstPlayer as HumanPlayer).GetValue = () =>
                    {
                        int selectedNumber;
                        bool proceed = false;

                        do
                        {
                            proceed = int.TryParse(Console.ReadLine(), out selectedNumber);
                            if (!SzemerediGame.GeneratedNumbers.Any(x => x.Value.Value == selectedNumber))
                            {
                                Console.WriteLine("Choose the number that is not colored yet!");
                                proceed = false;
                            }
                        } while (!proceed);

                        return selectedNumber;
                    };
                }
                IPlayer secondPlayer = PlayerFactory.GetPlayer(secondPlayerType);
                secondPlayer.Id = 2;

                DisplayNumbers(SzemerediGame.GeneratedNumbers);
                //Display game state

                while (!SzemerediGame.GameEnded)
                {
                    turnCount++;
                    Console.WriteLine("\nYour turn!\n");
                    firstPlayer.Play(SzemerediGame);
                    Console.Clear();
                    DisplayNumbers(SzemerediGame.GeneratedNumbers);
                    if (SzemerediGame.Check(firstPlayer.PlayerNumbers))
                    {
                        winner = firstPlayer.Name;
                        winningSequence = ArithmeticSequences.ArithmeticSequences.Find(firstPlayer.PlayerNumbers, SzemerediGame.SequenceLength).First();
                        break;
                    }

                    if (SzemerediGame.GameEnded)
                    {
                        break;
                    }

                    secondPlayer.Play(SzemerediGame);
                    Console.Clear();
                    DisplayNumbers(SzemerediGame.GeneratedNumbers);
                    if (SzemerediGame.Check(secondPlayer.PlayerNumbers))
                    {
                        winner = secondPlayer.Name;
                        winningSequence = ArithmeticSequences.ArithmeticSequences.Find(secondPlayer.PlayerNumbers, SzemerediGame.SequenceLength).First();
                        break;
                    }
                }

                Console.Clear();
                DisplayNumbers(SzemerediGame.GeneratedNumbers);

                if (!winner.Equals("Draw"))
                {
                    Console.WriteLine("\nWinner: " + winner);
                    Console.WriteLine();
                    DisplaySequence(winningSequence);
                    Console.WriteLine();
                    Console.WriteLine("Number of turns: " + turnCount);
                }
                else
                {
                    Console.WriteLine(winner + "!");
                    Console.WriteLine();
                    Console.WriteLine("Numbser of turns: " + turnCount);
                }

                Console.WriteLine("\nPress any key to exit...");

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.ReadKey();
            }
        }

        private static void GetInitialParametrsFromXML(Game.Game szemerediGame, Parameters parameters)
        {
            szemerediGame.Lower = parameters.Lower;

            szemerediGame.Upper = parameters.Upper;

            szemerediGame.SetCount = parameters.SetCount;

            szemerediGame.SequenceLength = parameters.SequenceLength;
        }

        private static void GetInitialParametrs(Game.Game SzemerediGame)
        {
            Console.WriteLine("Initialize game\n");

            bool proceed = false;

            do
            {
                Console.Write("Lower limit: ");
                proceed = int.TryParse(Console.ReadLine(), out SzemerediGame.Lower);
                if (!proceed || SzemerediGame.Lower < 0)
                {
                    Console.WriteLine("Lower limit must be a  non-negative integer!");
                    proceed = false;
                }
            } while (!proceed);

            do
            {
                Console.Write("Upper limit: ");
                proceed = int.TryParse(Console.ReadLine(), out SzemerediGame.Upper);
                if (!proceed || SzemerediGame.Upper < SzemerediGame.Lower)
                {
                    Console.WriteLine("Upper limit must be a  non-negative integer greater than lower limit!");
                    proceed = false;
                }
            } while (!proceed);


            do
            {
                Console.Write("Cardinality of set: ");
                proceed = int.TryParse(Console.ReadLine(), out SzemerediGame.SetCount);
                if (!proceed || SzemerediGame.SetCount > SzemerediGame.Upper - SzemerediGame.Lower + 1 || SzemerediGame.SetCount <=1)
                {
                    Console.WriteLine("Cardinality of set must be greater than  1 and smaller than (upper limit) - (lower limit) + 1!");
                    proceed = false;
                }
            } while (!proceed);

            do
            {
                Console.Write("Length of arithmetic sequence: ");
                proceed = int.TryParse(Console.ReadLine(), out SzemerediGame.SequenceLength);
                if (!proceed || SzemerediGame.SetCount < SzemerediGame.SequenceLength || SzemerediGame.SequenceLength <= 1)
                {
                    Console.WriteLine("Length of arithmetic sequence must be greater than 1 i smaller than generated set cardinality");
                    proceed = false;
                }
            } while (!proceed);
        }

        private static void DisplayNumbers(Dictionary<int, Game.Number> numbers)
        {
            Console.WriteLine("Game state:");
            foreach (var n in numbers.Values.ToList())
            {
                Console.ForegroundColor = IdToColor[n.Owner];
                Console.Write(n.Value + " ");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void DisplaySequence(List<int> sequence)
        {
            foreach (var number in sequence)
            {
                Console.Write(number + " ");
            }
        }

        private static PlayerType GetPlayerType()
        {
            int position = 0;

            PlayerType[] types = { PlayerType.Random, PlayerType.Aggressive, PlayerType.Defensive };

            PlayerType type = PlayerType.Random;

            ConsoleKey key;

            do
            {
                type = types[position];

                Console.WriteLine("Choose opponent level:\n");

                for (int i = 0; i < types.Length; i++)
                {
                    if (i == position)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.WriteLine(types[i].ToString());
                }
                Console.BackgroundColor = ConsoleColor.Black;
                key = Console.ReadKey().Key;

                if (key.Equals(ConsoleKey.DownArrow))
                {
                    position = (position + 1) % 3;
                }
                else if (key.Equals(ConsoleKey.UpArrow))
                {
                    position = (position + 2) % 3;
                }
                Console.Clear();
            }
            while (!key.Equals(ConsoleKey.Enter));


            return type;
        }
    }
}
