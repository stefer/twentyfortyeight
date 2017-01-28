namespace Tests
{
    using Moq;

    using TwentyFortyeight.Lib;
    using Xunit;

    public class GameTests
    {
        [Fact]
        public void Start_SetTwoValues()
        {
            Mock<Board> boardMock = new Mock<Board>();
            Game game = new Game(boardMock.Object);

            boardMock.Setup(x => x.AllCells).Returns(new int[] { 2, 2 });

            game.Start();

            boardMock.Verify(x => x.Set(It.IsAny<int>(), It.IsAny<int>(), It.Is<int>(v => v == 2 || v == 4)), Times.AtLeast(2));
        }

        [Fact]
        public void Right_SetOneNewValue()
        {
            Mock<Board> boardMock = new Mock<Board>();
            Game game = new Game(boardMock.Object);

            boardMock.Setup(x => x.IsFree(2, 3)).Returns(true);

            game.Right();

            boardMock.Verify(x => x.Set(2, 3, It.Is<int>(v => v == 2 || v == 4)));
        }
    }
}
