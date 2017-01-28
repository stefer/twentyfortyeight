using System;
using System.Linq;

namespace TwentyFortyeight.Lib
{
    public class Game
    {
        private readonly Random rand = new Random();

        public Game(Board board)
        {
            Board = board;
        }

        public Game()
        {
            Board = Board.Create();
        }

        private Board Board { get; set; }

        public void Start()
        {
            Board.Set(RandomBySize(), RandomBySize(), NewNumber());

            do
            {
                Board.Set(RandomBySize(), RandomBySize(), NewNumber());
            }  while (Board.AllCells.Count(cellvalue => cellvalue != 0) < 2);
        }

        public void Right()
        {
            SetRandomAtFreeCell();
            Board.Right();
        }

        private void SetRandomAtFreeCell()
        {
            var freePositions =
                (from column in Enumerable.Range(0, Board.Size)
                from row in Enumerable.Range(0, Board.Size)
                where Board.IsFree(row, column)
                select new Board.Position(row, column)).ToList();

            var position = freePositions[rand.Next(0, freePositions.Count)];

            Board.Set(position.Row, position.Column, NewNumber());
        }

        private int NewNumber()
        {
            return rand.Next() > 0.9 ? 4 : 2;
        }

        private int RandomBySize()
        {
            return rand.Next(0, Board.Size);
        }
    }
}
