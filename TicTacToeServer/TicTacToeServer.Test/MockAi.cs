using TicTacToe.Core;
using TicTacToeServer.Core;
using Moq;

namespace TicTacToeServer.Test
{
    public class MockAi : IAi
    {
        private readonly Mock<IAi> _mock;

        public MockAi()
        {
            _mock = new Mock<IAi>();
        }
        public ITicTacToeBoxClass.ITicTacToeBox Move(
            ITicTacToeBoxClass.ITicTacToeBox ticTacToeBox, 
            GameSettings.gameSetting settings)
        {
            return _mock.Object.Move(ticTacToeBox, settings);
        }

        public MockAi StubMove(ITicTacToeBoxClass.ITicTacToeBox stubBox)
        {
            _mock.Setup(m => m.Move(
                It.IsAny<ITicTacToeBoxClass.ITicTacToeBox>(),
                It.IsAny<GameSettings.gameSetting>())).Returns(stubBox);
            return this;
        }
    }
}