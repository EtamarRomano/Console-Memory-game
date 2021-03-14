using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using B20_Ex02_MemoryGame;

namespace B20_Ex02_MemoryGame
{
    internal class GamePlay
    {
        private string m_mode;
        private Board m_board;
        private Player m_player1;
        private Player m_player2;

        internal GamePlay(string i_mode, Board i_board, Player i_player1, Player i_player2)
        {
            this.m_mode = i_mode;
            this.m_board = i_board;
            this.m_player1 = i_player1;
            this.m_player2 = i_player2;
        }

        internal void StartGame()
        {
            if (m_mode == "c")
            {
                CompVsPlayerGamePlay();
            }
            else
            {
                PlayerVsPlayerGameplay();
            }

            m_board.ShowBoard();
            Console.WriteLine("{0}'s score is: " + m_player1.Score, m_player1.Name);
            Console.WriteLine("{0}'s score is: " + m_player2.Score, m_player2.Name);
            if (m_player2.Score > m_player1.Score)
            {
                Console.WriteLine("{0} won the Game, Congratulations!", m_player2.Name);
            }

            if (m_player2.Score < m_player1.Score)
            {
                Console.WriteLine("{0} won the Game, Congratulations!", m_player1.Name);
            }

            if (m_player2.Score == m_player1.Score)
            {
                Console.WriteLine("its a draw!!!");
            }
        }

        private void CompVsPlayerGamePlay()
        {
            m_player2 = new Player("Comp");
            Random rand = new Random();
            byte turn_Flag = (byte)rand.Next(2); // determines who starts the game
            string chosen_Card_1 = string.Empty;
            string chosen_Card_2 = string.Empty;

            Console.WriteLine("Hello {0}, we are ready to start", m_player1.Name);
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine(@"Game Instructions:
in each turn any player needs to choose a two slots on the game board to reveal,
if they are the same you'll get 1 point otherwise the slots will return to hidden state and the i get the turn.
pay attention to pick a proper slot in the board game like A1/E3 etc... (use capital letters)");
            Console.WriteLine("Press enter if you are ready to Start!");
            Console.ReadLine();
            Ex02.ConsoleUtils.Screen.Clear();
            while (!m_board.IsGameFinished(m_player1.Score, m_player2.Score))
            {
                if (turn_Flag == 1)
                {
                    Console.WriteLine("{0} it your turn to play!", m_player1.Name);
                    m_board.ShowBoard();
                    Console.WriteLine("please enter a slot to reveal");
                    chosen_Card_1 = Console.ReadLine();
                    /// checking argument validation
                    while (!m_board.IsValidSlot(chosen_Card_1, 'p'))
                    {
                        chosen_Card_1 = Console.ReadLine();
                    }

                    /// end of validation checking
                    m_board.IsGameTerminated(chosen_Card_1); /// system shuts down in case of "Q"

                    Ex02.ConsoleUtils.Screen.Clear();
                    m_board.ShowBoard(chosen_Card_1);
                    Console.WriteLine("please enter another slot to reveal");
                    chosen_Card_2 = Console.ReadLine();
                    /// checking argument validation
                    while (!m_board.IsValidSlot(chosen_Card_2, 'p'))
                    {
                        chosen_Card_2 = Console.ReadLine();
                    }

                    /// end of validation checking                    
                    m_board.IsGameTerminated(chosen_Card_1); /// system shuts down in case of "Q"

                    Ex02.ConsoleUtils.Screen.Clear();
                    m_board.ShowBoard(chosen_Card_1, chosen_Card_2);
                    System.Threading.Thread.Sleep(2000);
                    Ex02.ConsoleUtils.Screen.Clear();
                    if( m_board.IsGussedCorrectly(chosen_Card_1, chosen_Card_2))
                    {
                        m_player1.Score++;
                    }
                    else
                    {
                        turn_Flag = (byte)(1 - turn_Flag); // making the turn to switch to the other player
                    }
                    
                }
                else
                {
                    Console.WriteLine("It's my turn now");
                    System.Threading.Thread.Sleep(1000);
                    m_board.ShowBoard();
                    System.Threading.Thread.Sleep(1000);
                    Ex02.ConsoleUtils.Screen.Clear();
                    //// Choosing first random slot
                    chosen_Card_1 = m_player2.ComputerFirstMove(m_board);
                    while (!m_board.IsValidSlot(chosen_Card_1, 'c'))
                    {
                        chosen_Card_1 = m_player2.ComputerFirstMove(m_board);
                    }
                    //// Slot has been chosen

                    m_board.ShowBoard(chosen_Card_1);
                    //// Choosing second random slot
                    chosen_Card_2 = m_player2.ComputerSecondMove(m_board, chosen_Card_1);
                    while (!m_board.IsValidSlot(chosen_Card_2, 'c'))
                    {
                        chosen_Card_2 = m_player2.ComputerSecondMove(m_board, chosen_Card_1);
                    }
                    //// Slot has been chosen

                    Ex02.ConsoleUtils.Screen.Clear();
                    m_board.ShowBoard(chosen_Card_1, chosen_Card_2);
                    System.Threading.Thread.Sleep(2000);
                    Ex02.ConsoleUtils.Screen.Clear();
                    if(m_board.IsGussedCorrectly(chosen_Card_1, chosen_Card_2))
                    {
                        m_player2.Score++;
                    }
                    else
                    {
                        turn_Flag = (byte)(1 - turn_Flag); /// making the turn to switch to the other player
                    }

                }

                chosen_Card_1 = string.Empty;
                chosen_Card_2 = string.Empty;
            }
        }

        private void PlayerVsPlayerGameplay()
        {
            Random rand = new Random();
            byte turn_Flag = (byte)rand.Next(2); // determines who starts the game
            string chosen_Card_1 = null;
            string chosen_Card_2 = null;

            Console.WriteLine("Hello {0} and {1}, we are ready to start", m_player1.Name, m_player2.Name);
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine(@"Game Instructions:
in each turn any player needs to choose a two slots on the game board to reveal,
if they are the same you'll get 1 point otherwise the slots will return to hidden state and the other player gets the turn.
pay attention to pick a proper slot in the board game like A1/E3 etc... (use capital letters)");
            Console.WriteLine("Press enter if you are ready to Start!");
            Console.ReadLine();
            Ex02.ConsoleUtils.Screen.Clear();
            while (!m_board.IsGameFinished(m_player1.Score, m_player2.Score))
            {
                Console.WriteLine("{0} it your turn to play!", turn_Flag == 1 ? m_player1.Name : m_player2.Name);
                m_board.ShowBoard();
                Console.WriteLine("please enter a slot to reveal");
                chosen_Card_1 = Console.ReadLine();
                /// checking argument validation
                while (!m_board.IsValidSlot(chosen_Card_1, 'p'))
                {
                    chosen_Card_1 = Console.ReadLine();
                }

                m_board.IsGameTerminated(chosen_Card_1); /// system shuts down in case of "Q"
                                                         /// end of validation checking
                Ex02.ConsoleUtils.Screen.Clear();
                m_board.ShowBoard(chosen_Card_1);
                Console.WriteLine("please enter another slot to reveal");
                chosen_Card_2 = Console.ReadLine();
                /// checking argument validation
                while (!m_board.IsValidSlot(chosen_Card_2, 'p'))
                {
                    chosen_Card_2 = Console.ReadLine();
                }

                m_board.IsGameTerminated(chosen_Card_1); /// system shuts down in case of "Q"
                /// end of validation checking
                Ex02.ConsoleUtils.Screen.Clear();
                m_board.ShowBoard(chosen_Card_1, chosen_Card_2);
                System.Threading.Thread.Sleep(2000);
                Ex02.ConsoleUtils.Screen.Clear();
                if(m_board.IsGussedCorrectly(chosen_Card_1, chosen_Card_2) && turn_Flag == 1)
                {
                    m_player1.Score++;
                }
                else if (m_board.IsGussedCorrectly(chosen_Card_1, chosen_Card_2) && turn_Flag == 0)
                {
                    m_player2.Score++;
                }
                else
                {
                    turn_Flag = (byte)(1 - turn_Flag); // making the turn to switch to the other player
                }
            }
        }
    }
}