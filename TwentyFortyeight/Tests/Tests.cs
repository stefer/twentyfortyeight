namespace Tests
{
    using System;
    using System.Linq;
    using TewntyFortyeight;
    using Xunit;


    public class Tests
    {

        [Fact]
        public void NewBoard_IsEmpty() 
        {
            Board board = new Board();

            Assert.Equal(Board.Empty.Cells, board.Cells);
        }

        [Fact]
        public void EmptyBoard_HasOnlyZeroValues()
        {
            Assert.All(
               Board.Empty.Cells, r => 
                    Assert.True(Array.TrueForAll(r, c => c == 0)));
        }

        [Fact]
        public void Start_SetTwoValues()
        {
            Board board = new Board();
            board.Start();

            Assert.Equal(2, board.AllCells.Count(x => x > 0));
            Assert.All(board.AllCells, c => Assert.True(c == 0 || c == 2 || c == 4));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        [InlineData(5, 5)]
        [InlineData(6, 6)]
        public void Right_ShouldShiftAllToRightEdge(int r, int c)
        {
            Board board = new Board();
            board.Set(r, c, 2);
            board.Right();

            Assert.Equal(0, board.Get(r, c));
            Assert.Equal(2, board.Get(r, Board.Last));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        [InlineData(5, 5)]
        [InlineData(6, 6)]
        public void Right_ShouldShiftAllToRightEdge_AndAdd(int r, int c)
        {
            Board board = new Board();
            board.Set(r, Board.Last, 2);
            board.Set(r, c, 2);
            board.Right();

            Assert.Equal(0, board.Get(r, c));
            Assert.Equal(4, board.Get(r, Board.Last));
        }

        [Fact]
        public void Right_ShouldAddIfAllSet()
        {
            Board board = new Board();
            board.Set(0, new int[] { 0, 2, 0, 2, 0, 2, 0, 2});
            board.Right();
            
            Assert.Equal(new int[] { 0, 0, 0, 0, 0, 0, 0, 8 }, board.Get(0));
        }


        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        [InlineData(4, 4)]
        [InlineData(5, 5)]
        [InlineData(6, 6)]
        [InlineData(7, 7)]
        public void Left_ShouldShiftAllToLeftEdge_AndAdd(int r, int c)
        {
            Board board = new Board();
            board.Set(r, Board.First, 2);
            board.Set(r, c, 2);
            board.Left();

            Assert.Equal(0, board.Get(r, c));
            Assert.Equal(4, board.Get(r, Board.First));
        }

        [Fact]
        public void Left_ShouldAddIfAllSet()
        {
            Board board = new Board();
            board.Set(0, new int[] { 0, 2, 0, 2, 0, 2, 0, 2 });
            board.Left();

            Assert.Equal(new int[] { 8, 0, 0, 0, 0, 0, 0, 0 }, board.Get(0));
        }
    }
}
