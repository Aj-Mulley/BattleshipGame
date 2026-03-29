using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipGame
{
    public class Game
    {
        public void Start()
        {
            ShowWelcomeMessage();
            RunGameLoop();
            ShowGameOverMessage();
        }

        private void ShowWelcomeMessage()
        {
            Console.WriteLine("=================================");
            Console.WriteLine("         BATTLESHIP GAME         ");
            Console.WriteLine("=================================");
            Console.WriteLine("Sink all enemy ships to win!");
            Console.WriteLine();
            Console.WriteLine("Press any key to start...");
            Console.ReadKey();
            Console.Clear();
        }

        private void RunGameLoop()
        {
            bool gameOver = false;
            bool playerTurn = true;
            int turnCounter = 1;

            while (!gameOver)
            {
                Console.WriteLine("=================================");
                Console.WriteLine($"Turn #{turnCounter}");
                Console.WriteLine("=================================");

                if (playerTurn)
                {
                    PlayerTurn();
                }
                else
                {
                    ComputerTurn();
                }

                // Temporary win condition (for testing)
                if (turnCounter == 10)
                {
                    gameOver = true;
                }

                playerTurn = !playerTurn;
                turnCounter++;

                Console.WriteLine();
                Console.WriteLine("Press any key for next turn...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void PlayerTurn()
        {
            Console.WriteLine("PLAYER TURN");

            int row = GetValidNumber("Enter attack row (0-7): ");
            int col = GetValidNumber("Enter attack column (0-7): ");

            Console.WriteLine($"Player attacks position ({row}, {col})");

            
        }

        private void ComputerTurn()
        {
            Console.WriteLine("COMPUTER TURN");

            Random rand = new Random();
            int row = rand.Next(0, 8);
            int col = rand.Next(0, 8);

            Console.WriteLine($"Computer attacks position ({row}, {col})");

            
        }

        private int GetValidNumber(string prompt)
        {
            int number;
            bool valid;

            do
            {
                Console.Write(prompt);
                valid = int.TryParse(Console.ReadLine(), out number);

                if (!valid || number < 0 || number > 7)
                {
                    Console.WriteLine("Invalid input. Enter a number between 0 and 7.");
                }

            } while (!valid || number < 0 || number > 7);

            return number;
        }

        private void ShowGameOverMessage()
        {
            Console.WriteLine("=================================");
            Console.WriteLine("            GAME OVER            ");
            Console.WriteLine("=================================");
        }
    }
}
