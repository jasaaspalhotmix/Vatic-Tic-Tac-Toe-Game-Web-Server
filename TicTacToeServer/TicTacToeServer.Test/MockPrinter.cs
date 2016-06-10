using Moq;
using Server.Core;

namespace TicTacToeServer.Test
{
    public class MockPrinter : IPrinter
    {
        private readonly Mock<IPrinter> _mock;

        public MockPrinter()
        {
            _mock = new Mock<IPrinter>();
        }

        public string Log { get; set; }

        public void Print(string output)
        {
            _mock.Object.Print(output);
        }

        public void PrintToFile(string output, string path)
        {
            _mock.Object.PrintToFile(output, path);
        }

        public void VerifyPrint(string output)
        {
            _mock.Verify(m => m.Print(output), Times.AtLeastOnce);
        }

        public void VerifyPrintToFile(string output, string path)
        {
            _mock.Verify(m => m.PrintToFile(output, path), Times.AtLeastOnce);
        }
    }
}