
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;

namespace GameTest
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
            Console.WriteLine(GetLocalIPAddress());
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
                    //ComputerTurn();
                    OpponentTurn();
                    
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
        private async void OpponentTurn()
        {
            Server server = new Server(5000);

            // Receive messages here
            int row = -1;
            int col = -1;
            server.Start();
            server.OnMessageReceived += (msg) =>
            {
                Console.WriteLine("Client says: " + msg);

                
                if (row == -1)
                {
                    row = int.Parse(msg);//Make try parse later
                }
                else
                {
                    col = int.Parse(msg);
                    Console.WriteLine($"Opponent attacks position ({row}, {col})");
                    row = -1;
                    col = -1;
                }
            }
            ;



            Console.WriteLine("Server running... press ENTER to stop");
            Console.ReadLine();
            

            server.Stop();
        
            //Console.WriteLine($"Opponent attacks position ({ row}, { col})"); TBD
            
            //catch { //Remember to wrap above in exception handler
            //    OpponentTurn();
            //    Console.Clear();
            //    Console.WriteLine("Player did not enter valid number");
                
            //}
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
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

    }
}
