namespace Tests
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using TwentyFortyeight.Lib;
    using Xunit;

    public class BoardTests
    {
        [Fact]
        public void NewBoard_IsEmpty() 
        {
            BoardImplementation board = new BoardImplementation();

            Assert.Equal(Board.Empty.Cells, board.Cells);
        }

        [Fact]
        public void EmptyBoard_HasOnlyZeroValues()
        {
            Assert.All(
               Board.Empty.Cells, r => 
                    Assert.True(Array.TrueForAll(r, c => c == 0)));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public void Right_ShouldShiftAllToRightEdge(int r, int c)
        {
            BoardImplementation board = new BoardImplementation();
            board.Set(r, c, 2);
            board.Right();

            Assert.Equal(0, board.Get(r, c));
            Assert.Equal(2, board.Get(r, Board.Last));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public void Right_ShouldShiftAllToRightEdge_AndAdd(int r, int c)
        {
            BoardImplementation board = new BoardImplementation();
            board.Set(r, Board.Last, 2);
            board.Set(r, c, 2);
            board.Right();

            Assert.Equal(0, board.Get(r, c));
            Assert.Equal(4, board.Get(r, Board.Last));
        }

        [Fact]
        public void Right_ShouldAddIfAllSet()
        {
            BoardImplementation board = new BoardImplementation();
            board.SetRow(0, new int[] { 2, 2, 0, 2});
            board.Right();
            
            Assert.Equal(new int[] { 0, 0, 2, 4 }, board.GetRow(0));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 3)]
        public void Left_ShouldShiftAllToLeftEdge_AndAdd(int r, int c)
        {
            BoardImplementation board = new BoardImplementation();
            board.Set(r, Board.First, 2);
            board.Set(r, c, 2);
            board.Left();

            Assert.Equal(0, board.Get(r, c));
            Assert.Equal(4, board.Get(r, Board.First));
        }

        [Fact]
        public void Left_ShouldAddIfAllSet()
        {
            BoardImplementation board = new BoardImplementation();
            board.SetRow(0, new int[] { 0, 2, 2, 2 });
            board.Left();

            Assert.Equal(new int[] { 4, 2, 0, 0 }, board.GetRow(0));
        }

        [Fact]
        public void Down_ShouldAddIfAllSet()
        {
            BoardImplementation board = new BoardImplementation();
            board.SetColumn(0, new int[] { 2, 2, 0, 2 });
            board.Down();

            Assert.Equal(new int[] { 0, 0, 2, 4 }, board.GetColumn(0));
        }

        [Fact]
        public void Up_ShouldAddIfAllSet()
        {
            BoardImplementation board = new BoardImplementation();
            board.SetColumn(0, new int[] { 0, 2, 2, 2 });
            board.Up();

            Assert.Equal(new int[] { 4, 2, 0, 0 }, board.GetColumn(0));
        }

    }
}
