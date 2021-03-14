using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B20_Ex02_MemoryGame
{
    internal class Board
    {
        private readonly byte m_BoardWidth;
        private readonly byte m_BoardHight;
        private char[,] m_GameBoard;
        private char[,] m_CurrentGameStateBoard;
        private char[,] m_ComputerMemory;

        internal Board(byte i_boardWidth, byte i_boardHight)
        {
            m_BoardWidth = i_boardWidth;
            m_BoardHight = i_boardHight;
            m_GameBoard = new char[m_BoardHight, m_BoardWidth];
            m_CurrentGameStateBoard = new char[m_BoardHight, m_BoardWidth];
            m_ComputerMemory = new char[m_BoardHight, m_BoardWidth];
            InitializeBoards();
        }

        internal byte Hight
        {
            get { return m_BoardHight; }
        }

        internal byte Width
        {
            get { return m_BoardWidth; }
        }

        internal char[,] ComputerMemory
        {
            get { return m_ComputerMemory; }
        }

        internal static bool isValidBoardSize(string i_boardSize, out byte boardSize)
        {
            bool ans = true;
            if (i_boardSize != "4" && i_boardSize != "6")
            {
                boardSize = 0;
                return false;
            }

            bool parsed = byte.TryParse(i_boardSize, out boardSize);
            if (!parsed)
            {
                ans = false;
            }

            return ans;
        }
        private void InitializeBoards()
        {
            for (int i = 0; i < m_BoardHight; i++)
            {
                for (int j = 0; j < m_BoardWidth; j++)
                {
                    m_CurrentGameStateBoard[i, j] = ' ';
                    m_ComputerMemory[i, j] = ' ';
                }
            }

            List<char> cards = new List<char>();
            char cardValue = 'A';
            byte cardIndex = 0;
            while (cardIndex < m_BoardWidth * m_BoardHight)
            {
                cards.Add(cardValue);
                cards.Add(cardValue);
                cardValue++;
                cardIndex += 2;
            }

            Random cardRand = new Random();
            byte random_Location = 0;
            for (int i = 0; i < m_BoardHight; i++)
            {
                for (int j = 0; j < m_BoardWidth; j++)
                {
                    random_Location = (byte)cardRand.Next(cards.Count);
                    m_GameBoard[i, j] = cards[random_Location];
                    cards.RemoveAt(random_Location);
                }
            }
        }

        

        internal void PrintBoard()
        {
            char columnLetter = 'A';
            StringBuilder seperator = new StringBuilder();
            StringBuilder boardToPrint = new StringBuilder();
            boardToPrint.Append("    ");
            seperator.Append("  =");
            string boardCell;
            for (int i = 0; i < m_BoardWidth; i++)
            {
                seperator.Append("====");
                boardToPrint.Append(columnLetter + "   ");
                columnLetter++;
            }

            for (int i = 0; i < m_BoardHight; i++)
            {
                boardToPrint.Append("\n");
                boardToPrint.Append(seperator);
                boardToPrint.Append("\n");
                boardToPrint.Append((i + 1) + " |");
                for (int j = 0; j < m_BoardWidth; j++)
                {
                    boardCell = string.Format(" {0} |", m_CurrentGameStateBoard[i, j]);
                    boardToPrint.Append(boardCell);
                }
            }

            boardToPrint.Append("\n");
            boardToPrint.Append(seperator);
            Console.WriteLine(boardToPrint.ToString());
        }

        internal void ShowBoard(string i_Slot1 = null, string i_Slot2 = null)
        {
            if (i_Slot1 == null && i_Slot2 == null)
            {
                PrintBoard();
            }

            if (i_Slot1 != null && i_Slot2 == null)
            {
                byte[] index = SlotToIndex(i_Slot1);
                m_CurrentGameStateBoard[index[0], index[1]] = m_GameBoard[index[0], index[1]];
                m_ComputerMemory[index[0], index[1]] = m_GameBoard[index[0], index[1]];
                PrintBoard();
            }

            if (i_Slot1 != null && i_Slot2 != null)
            {
                byte[] index1 = SlotToIndex(i_Slot1);
                byte[] index2 = SlotToIndex(i_Slot2);
                m_CurrentGameStateBoard[index1[0], index1[1]] = m_GameBoard[index1[0], index1[1]];
                m_CurrentGameStateBoard[index2[0], index2[1]] = m_GameBoard[index2[0], index2[1]];
                m_ComputerMemory[index2[0], index2[1]] = m_GameBoard[index2[0], index2[1]];
                PrintBoard();
                if (!IsGussedCorrectly(i_Slot1, i_Slot2))
                {
                    m_CurrentGameStateBoard[index1[0], index1[1]] = ' ';
                    m_CurrentGameStateBoard[index2[0], index2[1]] = ' ';
                }
            }
        }

        internal bool IsValidSlot(string i_Slot, char mode)
        {
            bool ans = true;
            byte[] index = new byte[2];
            if (i_Slot.Equals(string.Empty) || (i_Slot.Length != 2 && !i_Slot.Equals("Q")))
            {
                if (mode == 'p')
                {
                    Console.WriteLine("please enter a valid slot like A1, B2 etc...");
                }

                return false;
            }

            if (i_Slot == "Q")
            {
                return true;
            }

            if (ans == true && (i_Slot[0] < 'A' || i_Slot[0] > 'Z') || (i_Slot[1] < '0' || i_Slot[1] > '9'))
            {
                ans = false;
                if (mode == 'p')
                {
                    Console.WriteLine("please enter a valid slot like A1, B2 etc...");
                }
            }

            if (ans == true && !i_Slot.Equals("Q") && ((i_Slot[0] < 'A' || i_Slot[0] > ('A' + m_BoardWidth - 1)) || (i_Slot[1] < '1' || i_Slot[1] > ('1' + m_BoardHight - 1))))
            {
                ans = false;
                if (mode == 'p')
                {
                    Console.WriteLine("the slot you entered is out of bounds, please try again");
                }
            }

            index = SlotToIndex(i_Slot);
            if (ans == true && !i_Slot[0].Equals("Q") && m_CurrentGameStateBoard[index[0], index[1]] != ' ')
            {
                ans = false;
                if (mode == 'p')
                {
                    Console.WriteLine("this slot is not valid beacuse its is already fliped, please try again");
                }
            }

            return ans;
        }

        internal bool IsGussedCorrectly(string i_Slot1, string i_Slot2) // מחזיר 0 או 1 ומדפיס כל הכבוד בהתאם ומוחק בסוף את המסך
        {
            bool ans = false;
            byte[] index1 = new byte[2];
            byte[] index2 = new byte[2];
            index1 = SlotToIndex(i_Slot1);
            index2 = SlotToIndex(i_Slot2);
            if (m_GameBoard[index1[0], index1[1]] == m_GameBoard[index2[0], index2[1]])
            {
                ans = true;
                m_CurrentGameStateBoard[index1[0], index1[1]] = m_GameBoard[index1[0], index1[1]];
                m_CurrentGameStateBoard[index2[0], index2[1]] = m_GameBoard[index2[0], index2[1]];
            }

            return ans;
        }

        internal byte[] SlotToIndex(string i_Slot)
        {
            byte[] index = { (byte)(byte.Parse(i_Slot[1].ToString()) - 1), (byte)((char)(i_Slot[0]) - 65) };
            return index;
        }

        internal string IndexToSlot(int row, int colum)
        {
            string Slot;
            Slot = (char)(colum + 65) + (row + 1).ToString();
            Console.WriteLine(Slot);
            return Slot;
        }

        internal bool IsGameFinished(byte i_Player1Score, byte i_Player2Score)
        {
            bool ans = false;
            if (((i_Player1Score + i_Player2Score) * 2) == m_BoardHight * m_BoardWidth)
            {
                ans = true;
            }

            return ans;
        }

        internal bool IsGameTerminated(string i_chosenWord)
        {
            bool ans = false;
            if (i_chosenWord == "Q")
            {
                ans = true;
                Console.WriteLine("Goodbye, See you next time!");
                System.Threading.Thread.Sleep(2000);
                Environment.Exit(0);
            }

            return ans;
        }
    }
}