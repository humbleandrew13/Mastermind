using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mastermind
{
    class Program
    {
        static void Main(string[] args)
        {
            bool gameActive = true;

            while (gameActive)
            {
                int[] randomAnswer = GenerateAnswer();
                List<Tuple<int, int[], int[], string, string>> MasterRecord = new List<Tuple<int, int[], int[], string, string>>();

                Console.WriteLine("Welcome to Mastermind!\n");
                Console.WriteLine("Begin by entering a four digit sequence where each digit is an integer from 1 to 6.\n");
                Console.WriteLine("For each guess, a '+' indicates a correct digit in the correct position,");
                Console.WriteLine("and a '-' indicates a correct digit in an incorrect position.\n");

                for (int n = 0; n < 10; n++)
                {
                    Tuple<string, int[]> playerGuess = GetPlayerGuess();

                    MasterRecord = AssessGuess(n, playerGuess.Item2, randomAnswer, playerGuess.Item1, MasterRecord);
                    if(MasterRecord.Last().Item5 == "++++")
                    {
                        Console.WriteLine($"\n {playerGuess.Item1} is the correct sequence! You win!\n\b");
                        break;
                    }
                    else if (n == 9)
                    {
                        string answerString = "";
                        for (int q = 0; q < 4; q++)
                        {
                            answerString += randomAnswer[q].ToString();
                        }
                        Console.WriteLine($"You have exhausted your ten guesses.\nThe correct sequence was {answerString}. Better luck next time.\n");
                    }
                    else
                    {
                        Console.WriteLine($"\n {playerGuess.Item1} ==> {MasterRecord.Last().Item5}\n\n");
                    }
                }

                Console.WriteLine("Would you like to play again? (Y/N)\n");
                string playAgain = Console.ReadLine().Substring(0, 1).ToUpper();

                if (playAgain == "Y")
                {
                    Console.Clear();
                    continue;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\nThank you for trying Mastermind!");
                    Console.ReadKey();
                    gameActive = false;
                    break;
                }
            }


            int[] GenerateAnswer()
            {
                Random randomNumber = new Random();
                int[] intArray = new int[4];

                for (int x = 0; x < 4; x++)
                {
                    intArray[x] = randomNumber.Next(1, 7);
                }

                return intArray;
            }

            Tuple<string, int[]> GetPlayerGuess()
            {
                bool tryAgain = true;
                string guess;
                do
                {
                    guess = Console.ReadLine();

                    if (guess.Length == 4 && int.TryParse(guess, out int guessInt))
                    {
                        bool numberFail = false;
                        foreach (char c in guess)
                        {
                            if (int.Parse(c.ToString()) > 6 || int.Parse(c.ToString()) < 1)
                            {
                                Console.WriteLine("This is not an appropriate guess. Please try again.");
                                numberFail = true;
                                break;
                            }
                        }
                        if (!numberFail) tryAgain = false;
                    }
                    else
                    {
                        Console.WriteLine("This is not an appropriate guess. Please try again.");
                    }
                } while (tryAgain);

                return new Tuple<string, int[]>(guess, ParseGuess(guess));
            }

            int[] ParseGuess(string playerGuess)
            {
                int[] guessArray = new int[4];
                for (int z = 0; z < 4; z++)
                {
                    guessArray[z] = int.Parse(playerGuess.Substring(z, 1));
                }
                
                return guessArray;
            }

            List<Tuple<int, int[], int[], string, string>> AssessGuess(int guessSequence, int[] playerGuessInts, int[] answerInts, string playerGuess, 
                List<Tuple<int, int[], int[], string, string>> MasterRecord)
            {
                string result = "";
                int[] answerIntsCopy = new int[4];
                answerInts.CopyTo(answerIntsCopy, 0);
                List<Tuple<int, int>> matches = new List<Tuple<int, int>>();

                for (int x = 0; x < 4; x++)
                {
                    if (playerGuessInts[x] == answerInts[x])
                    {
                        result += "+";
                        matches.Add(new Tuple<int, int>(x, playerGuessInts[x]));
                    }
                }

                if (result != "++++")
                {
                    for (int y = 0; y < 4; y++)
                    {
                        if (answerInts.Contains(playerGuessInts[y]))
                        {
                            List<Tuple<int, int>> funnel = matches.Where(m => m.Item2 == playerGuessInts[y]).ToList();
                            foreach (var a in funnel)
                            {
                                answerIntsCopy[a.Item1] = 0;
                            }
                        }

                        if (answerIntsCopy.Contains(playerGuessInts[y]))
                        {
                            if (playerGuessInts[y] != answerInts[y])
                            {
                                result += "-";
                                answerIntsCopy[Array.IndexOf(answerIntsCopy, playerGuessInts[y])] = 0;

                            }
                        }
                    }
                }

                MasterRecord.Add(new Tuple<int, int[], int[], string, string>(guessSequence, playerGuessInts, answerInts, playerGuess, result));

                return MasterRecord;
            }
        }
    }
}
