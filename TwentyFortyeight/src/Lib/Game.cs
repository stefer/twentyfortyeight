using System;
using System.Linq;

namespace TwentyFortyeight.Lib
{
    public class Game
    {
        private readonly Random rand = new Random();

        public Board Board { get; set; } = new Board();

        public void Start()
        {
            Board.Set(RandomBySize(), RandomBySize(), NewNumber());

            do
            {
                Board.Set(RandomBySize(), RandomBySize(), NewNumber());
            }  while (Board.AllCells.Count(cellvalue => cellvalue != 0) < 2);
        }

        private int NewNumber()
        {
            return rand.Next(1, 3) * 2;
        }

        private int RandomBySize()
        {
            return rand.Next(0, Board.Size);
        }
    }
}
