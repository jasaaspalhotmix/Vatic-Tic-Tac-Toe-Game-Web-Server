using TicTacToe.Core;
using TicTacToeServer.Core;
using Moq;

namespace TicTacToeServer.Test
{
    public class MockUser : IUser
    {
        private readonly Mock<IUser> _mock;

        public MockUser()
        {
            _mock = new Mock<IUser>();
        }
        public ITicTacToeBoxClass.ITicTacToeBox Move(ITicTacToeBoxClass.ITicTacToeBox ticTacToeBox, int move, string playerSymbol, string aISymbol)
        {
            return _mock.Object.Move(ticTacToeBox, move, playerSymbol, aISymbol);
        }

        public MockUser StubMove(ITicTacToeBoxClass.ITicTacToeBox stubBox)
        {
            _mock.Setup(m => m.Move(It.IsAny<ITicTacToeBoxClass.ITicTacToeBox>(),
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(stubBox);
            return this;
        }
    }
}