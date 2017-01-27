namespace Tests
{
    using System.Linq;

    using TwentyFortyeight.Lib;
    using Xunit;

    public class GameTests
    {
        [Fact]
        public void Start_SetTwoValues()
        {
            Game game = new Game();
            game.Start();

            Assert.Equal(2, game.Board.AllCells.Count(x => x > 0));
            Assert.All(game.Board.AllCells, c => Assert.True(c == 0 || c == 2 || c == 4));
        }
    }
}
