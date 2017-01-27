using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TwentyFortyeight.Lib
{

    public abstract class Board
    {
        public const int Size = 4;
        public const int Last = Size - 1;
        public const int First = 0;

        public static Board Empty = new BoardImplementation();

        public virtual int[][] Cells { get; }

        public virtual IEnumerable<int> AllCells { get; }

        public static Board Create()
        {
            return new BoardImplementation();
        }

        public abstract void Set(int v1, int v2, int v3);
    }

    public class BoardImplementation : Board
    {

        private readonly int[][] cells = new int[Size][]
            { new int[Size], new int[Size] , new int[Size] , new int[Size] };

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

        public override int[][] Cells
        {
            get { return cells; }
        }

        public override IEnumerable<int> AllCells
        {
            get
            {
                for (int ri = 0; ri < cells.Length; ri++)
                {
                    int[] row = GetRow(ri);
                    for (int ci = 0; ci < row.Length; ci++)
                    {
                        yield return row[ci];
                    }
                }
            }
        }

        public int Get(int r, int c)
        {
            return cells[r][c];
        }

        public override void Set(int r, int c, int value)
        {
            cells[r][c] = value;
        }

        public int[] GetRow(int i)
        {
            return Cells[i];
        }

        public void SetRow(int r, int[] values)
        {
            if (values.Length != Size) throw new ArgumentException("Invalid array length", nameof(values));

            cells[r] = values;
        }

        public int[] GetColumn(int ci)
        {
            return (from ri in Sequence(First, Last) select Get(ri, ci))
                    .ToArray();
        }

        public void SetColumn(int ci, int[] values)
        {
            foreach (var ri in Sequence(0, Last))
            {
                Set(ri, ci, values[ri]);
            }
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

        public void Up()
        {
            ShiftAll(from ci in Sequence(First, Last)
                     from ri in Sequence(First + 1, Last)
                     from i in Sequence(First, ri - 1).Reverse()
                     select new Index(i + 1, i, ci, ci));
        }

        public void Down()
        {
            ShiftAll(from ci in Sequence(First, Last)
                     from ri in Sequence(First, Last - 1).Reverse()
                     from i in Sequence(ri + 1, Last)
                     select new Index(i - 1, i, ci, ci));
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
            return Enumerable.Range(from, to - from + 1);
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
