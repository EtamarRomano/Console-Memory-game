using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B20_Ex02_MemoryGame
{
    internal class Program
    {
        public static void Main()
        {
            byte parsedBoardWidth = 0;
            byte parsedBoardHight = 0;
            Player player2 = null;
            string playerName = string.Empty;
            string mode;
            string PlayAgainFlag = "Y";
            while (PlayAgainFlag.Equals("Y"))
            {
                Ex02.ConsoleUtils.Screen.Clear();
                while(playerName == string.Empty)
                {
                    Console.WriteLine("Hello player, please enter your name and press enter");
                    playerName = Console.ReadLine();
                }
                Player player1 = new Player(playerName);
                Console.WriteLine("Hello {0} please press 'c' if you want to play against the computer or 'p' to play with another player", player1.Name);
                mode = Console.ReadLine();
                
                /// checking argument validation
                while (mode != "c" && mode != "p")
                {
                    Console.WriteLine("that game mode is not valid, please try again");
                    mode = Console.ReadLine();
                }

                /// end of validation checking
                playerName = string.Empty;
                if (mode == "p")
                {
                    while (playerName == string.Empty)
                    {
                        Console.WriteLine("please enter the name of player number 2");
                        playerName = Console.ReadLine();
                    }
                    player2 = new Player(playerName);
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("Welcome {0}", player2.Name);
                }

                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("please enter the width of the playing board (such that it is 6 or 4)");
                string boardWidth = Console.ReadLine();

                /// checking argument validation
                while (!Board.isValidBoardSize(boardWidth, out parsedBoardWidth))
                {
                    if (parsedBoardWidth == 0)
                    {
                        Console.WriteLine("your board width is not valid, please enter new board width such that it is 6 or 4");
                        boardWidth = Console.ReadLine();
                    }
                }

                Console.WriteLine("please enter the hight of the playing board (6 or 4)");
                string boardHight = Console.ReadLine();
                while (!Board.isValidBoardSize(boardHight, out parsedBoardHight))
                {
                    if (parsedBoardHight == 0)
                    {
                        Console.WriteLine("your board hight is not valid, please enter new board hight that 6 or 4");
                        boardHight = Console.ReadLine();
                    }
                }

                /// end of validation checking
                Board board = new Board(parsedBoardWidth, parsedBoardHight);

                GamePlay game = new GamePlay(mode, board, player1, player2);
                System.Threading.Thread.Sleep(1000);
                Ex02.ConsoleUtils.Screen.Clear();
                System.Threading.Thread.Sleep(500);

                game.StartGame();

                Console.WriteLine("Do you wanna play another game? enter Y to start new one or any other key to quit");
                System.Threading.Thread.Sleep(1000);
                PlayAgainFlag = Console.ReadLine();
                playerName = string.Empty;
            }
        }
    }
}