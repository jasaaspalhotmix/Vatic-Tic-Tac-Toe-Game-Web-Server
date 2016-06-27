using System.Text;
using TicTacToeServer.Core;
using Xunit;

namespace TicTacToeServer.Test
{
    public class ProgramTest
    {
        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1000)]
        [InlineData(1999)]
        [InlineData(65001)]
        [InlineData(9999999)]
        public void Out_Of_Range_Ports(int invaildPorts)
        {
            var correctOutput = new StringBuilder();
            correctOutput.Append("Invaild Port Detected.");
            correctOutput.Append("Vaild Ports 2000 - 65000");

            string[] argsTicTacToe = { "-p", invaildPorts.ToString() };
            var mockPrinterOne = new MockPrinter();
            var serverMadeTicTacToe = Program.MakeServer(argsTicTacToe, mockPrinterOne);
            Assert.Null(serverMadeTicTacToe);
            mockPrinterOne.VerifyPrint(correctOutput.ToString());
        }

        [Fact]
        public void Make_Dirctory_Server_Twice_Same_Port()
        {
            var mockPrinter = new MockPrinter();
            var correctOutput = new StringBuilder();

            string[] args = { "-p", "8765" };
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.NotNull(serverMade);

            var serverMadeInvaild = Program.MakeServer(args, mockPrinter);
            Assert.Null(serverMadeInvaild);
            mockPrinter.VerifyPrint("Another Server is running on that port");
        }


        [Fact]
        public void Make_Dirctory_Server_Inncorect_Correct_Not_Port()
        {
            var mockPrinter = new MockPrinter();
            var correctOutput = new StringBuilder();
            correctOutput.Append("Invaild Port Detected.");
            correctOutput.Append("Vaild Ports 2000 - 65000");

            string[] args = { "-p", "hello" };
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.Null(serverMade);
            mockPrinter.VerifyPrint(correctOutput.ToString());
        }


        [Fact]
        public void Make_TicTacToe_Server_Correct()
        {
            var mockPrinter = new MockPrinter();
            string[] args = { "-p", "9560" };
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.NotNull(serverMade);
        }

        [Fact]
        public void Make_TicTacToe_Incorrect_Correct()
        {
            var mockPrinter = new MockPrinter();
            string[] args = { "2750", "-p" };
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.Null(serverMade);
        }

        [Fact]
        public void Make_TicTacToe_Incorrect_Correct_No_Port()
        {
            var mockPrinter = new MockPrinter();
            string[] args = { "-p", "-p" };
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.Null(serverMade);
        }

        [Fact]
        public void Make_Server_Inncorect_NoArgs()
        {
            var mockPrinter = new MockPrinter();
            var correctOutput = new StringBuilder();
            correctOutput.Append("Invaild Number of Arguments.\n");
            correctOutput.Append("Can only be -p PORT\n");
            correctOutput.Append("Examples:\n");
            correctOutput.Append("Server.exe -p 8080\n");
            correctOutput.Append("Server.exe -p 9999");

            var args = new[] { "-s" };
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.Null(serverMade);
            mockPrinter.VerifyPrint(correctOutput.ToString());
        }

        [Fact]
        public void Main_Starting_Program()
        {
            string[] args = { };
            Assert.Equal(0, Program.Main(args));
        }

        [Fact]
        public void Test_Running_Of_Server()
        {
            var mockServer = new MockMainServer().StubAcceptingNewConn();
            Program.RunServer(mockServer);
            mockServer.VerifyRun();
            mockServer.VerifyAcceptingNewConn();
        }

    }
}