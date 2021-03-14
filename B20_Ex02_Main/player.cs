using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B20_Ex02_MemoryGame
{
    internal class Player
    {
        private string m_Name;
        private byte m_Score;
        

        internal Player(string i_name)
        {
            this.m_Name = i_name;
            this.m_Score = 0;
        }

        internal string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        internal byte Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        internal string ComputerFirstMove(Board i_board)
        {
            Random rand = new Random();
            string chosen_Card;
            chosen_Card = ((char)(65 + rand.Next((int)(i_board.Width)))).ToString() + ((char)(49 + rand.Next((int)(i_board.Hight)))).ToString();
            return chosen_Card;
        }

        internal string ComputerSecondMove(Board i_board, string i_FirstSlot)
        {
           Random rand = new Random();
           string chosen_Card = string.Empty;
           byte[] index = i_board.SlotToIndex(i_FirstSlot);
           char card = i_board.ComputerMemory[index[0], index[1]];
           double probabilityOfSmartMove = rand.NextDouble();
            if (probabilityOfSmartMove <= 0.4)
            {
                for (int i = 0; i < i_board.Hight; i++)
                {
                    for (int j = 0; j < i_board.Width; j++)
                    {
                        if (i == index[0] && j == index[1])
                        {
                            continue;
                        }
                        if (i_board.ComputerMemory[i, j] == card)
                        {
                            chosen_Card = i_board.IndexToSlot(i, j);
                        }
                    }
                }
            }
            else
            {
                chosen_Card = ((char)(65 + rand.Next((int)(i_board.Width)))).ToString() + ((char)(49 + rand.Next((int)(i_board.Hight)))).ToString();
            }
            return chosen_Card;
        }
    }
}