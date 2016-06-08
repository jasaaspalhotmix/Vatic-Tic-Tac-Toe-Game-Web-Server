using Moq;
using Server.Core;

namespace TicTacToeServer.Test
{
    internal class MockMainServer : IMainServer
    {
        private readonly Mock<IMainServer> _mock;

        public MockMainServer()
        {
            _mock = new Mock<IMainServer>();
        }

        public void StopNewConnAndCleanUp()
        {
            _mock.Object.StopNewConnAndCleanUp();
        }


        public bool AcceptingNewConn => _mock.Object.AcceptingNewConn;

        void IMainServer.RunningProcess(object poolObject)
        {
            _mock.Object.RunningProcess(poolObject);
        }

        public void Run()
        {
            _mock.Object.Run();
        }

        public void VerifyRun()
        {
            _mock.Verify(m => m.Run(), Times.Once);
        }

        public void VerifyAcceptingNewConn()
        {
            _mock.Verify(m => m.AcceptingNewConn, Times.AtLeastOnce);
        }

        public void VerifyStopNewConnAndCleanUp()
        {
            _mock.Verify(m => m.StopNewConnAndCleanUp(), Times.AtLeastOnce);
        }


        public MockMainServer StubAcceptingNewConn()
        {
            _mock.Setup(m => m.AcceptingNewConn).Returns(false);
            return this;
        }
    }
}