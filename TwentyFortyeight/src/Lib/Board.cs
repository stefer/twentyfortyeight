using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TewntyFortyeight
{
    public class Board
    {
        public const int Size = 8;
        public const int Last = Size - 1;
        public const int First = 0;

        public static Board Empty = new Board();
        private readonly Random rand = new Random();

        private readonly int[][] cells = new int[Size][]
            { new int[Size], new int[Size] , new int[Size] , new int[Size], new int[Size], new int[Size],  new int[Size], new int[Size] };

        private struct Index
        {
            public int rFrom;
            public int rTo;
            public int cFrom;
            public int cTo;

            public Index(int rFrom, int rTo, int cFrom, int cTo)
            {
                this.rFrom = rFrom;
                this.rTo = rTo;
                this.cFrom = cFrom;
                this.cTo = cTo;
            }
        }

        public int[][] Cells
        {
            get { return cells; }
        }

        public IEnumerable<int> AllCells
        {
            get
            {
                for (int ri = 0; ri < cells.Length; ri++)
                {
                    int[] row = Get(ri);
                    for (int ci = 0; ci < row.Length; ci++)
                    {
                        yield return row[ci];
                    }
                }
            }
        }

        public void Start()
        {
            Set(rand.Next(0, Size), rand.Next(0, Size), rand.Next(1, 3) * 2);
            Set(rand.Next(0, Size), rand.Next(0, Size), rand.Next(1, 3) * 2);
        }

        public void Set(int r, int c, int value)
        {
            cells[r][c] = value;
        }

        public void Set(int r, int[] values)
        {
            if (values.Length != Size) throw new ArgumentException("Invalid array length", nameof(values));

            cells[r] = values;
        }

        public int Get(int r, int c)
        {
            return cells[r][c];
        }

        public int[] Get(int i)
        {
            return Cells[i];
        }

        public void Right()
        {
            ShiftAll(from ri in Sequence(First, Last)
                     from ci in Sequence(First, Last - 1).Reverse()
                     from i in Sequence(ci + 1, Last)
                     select new Index(ri, ri, i - 1, i));

        }

        public void Left()
        {
            ShiftAll(from ri in Sequence(First, Last)
                     from ci in Sequence(First + 1, Last)
                     from i in Sequence(First, ci - 1).Reverse()
                     select new Index(ri, ri, i + 1, i));
        }

        private void ShiftAll(IEnumerable<Index> indices)
        {
            foreach (var item in indices) Shift(item);
        }

        private void Shift(Index id)
        {
            var current = Get(id.rFrom, id.cFrom);
            var candidate = Get(id.rTo, id.cTo);
            if (candidate == 0 || candidate == current)
            {
                current += candidate;
                Set(id.rTo, id.cTo, current);
                Set(id.rFrom, id.cFrom, 0);
            }
        }

        private IEnumerable<int> Sequence(int from, int to)
        {
            for (int i = from; i <= to; i++)
            {
                yield return i;
            }
        }

        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine(" -------------------------------------------------------------");
            for (int r = 0; r < Size; r++)
            {
                for (int c = 0; c < Size; c++)
                {
                    sb.AppendFormat(" | {0,5:0}", cells[r][c]);
                }
                sb.AppendLine(" |");
            }
            sb.AppendLine(" -------------------------------------------------------------");
            return sb.ToString();
        }

    }
}
