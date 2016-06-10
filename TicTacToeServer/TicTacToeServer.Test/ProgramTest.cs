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

            string[] argsHelloWorld = {"-p", invaildPorts.ToString()};
            var mockPrinterOne = new MockPrinter();
            var serverMadeHelloWorld = Program.MakeServer(argsHelloWorld, mockPrinterOne);
            Assert.Null(serverMadeHelloWorld);
            mockPrinterOne.VerifyPrint(correctOutput.ToString());

            string[] args = {"-p", invaildPorts.ToString(), "-d", "C:\\"};
            var mockPrinterTwo = new MockPrinter();
            var serverMade = Program.MakeServer(args, mockPrinterTwo);
            Assert.Null(serverMade);
            mockPrinterTwo.VerifyPrint(correctOutput.ToString());

            var mockPrinterThree = new MockPrinter();
            string[] argsSwaped = {"-d", "C:\\", "-p", invaildPorts.ToString()};
            var serverMadeSwaped = Program.MakeServer(argsSwaped, mockPrinterThree);
            Assert.Null(serverMadeSwaped);
            mockPrinterThree.VerifyPrint(correctOutput.ToString());
        }

        [Fact]
        public void Make_Dirctory_Server_Correct()
        {
            var mockPrinter = new MockPrinter();
            string[] args = {"-p", "32000", "-d", "C:\\"};
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.NotNull(serverMade);
        }

        [Fact]
        public void Make_Dirctory_Server_Twice_Same_Port()
        {
            var mockPrinter = new MockPrinter();
            var correctOutput = new StringBuilder();

            string[] args = {"-p", "8765", "-d", "C:\\"};
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.NotNull(serverMade);

            var serverMadeInvaild = Program.MakeServer(args, mockPrinter);
            Assert.Null(serverMadeInvaild);
            mockPrinter.VerifyPrint("Another Server is running on that port");
        }

        [Fact]
        public void Make_Dirctory_Server_Correct_Arg_Backwords()
        {
            var mockPrinter = new MockPrinter();
            string[] args = {"-d", "C:\\", "-p", "2020"};
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.NotNull(serverMade);
        }

        [Fact]
        public void Make_Dirctory_Server_Inncorect_Correct_Not_Dir()
        {
            var mockPrinter = new MockPrinter();
            var correctOutput = new StringBuilder();

            string[] args = {"-d", "Hello", "-p", "3258"};
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.Null(serverMade);

            mockPrinter.VerifyPrint("Not a vaild directory");
        }

        [Fact]
        public void Make_Dirctory_Server_Inncorect_Correct_Not_Port()
        {
            var mockPrinter = new MockPrinter();
            var correctOutput = new StringBuilder();
            correctOutput.Append("Invaild Port Detected.");
            correctOutput.Append("Vaild Ports 2000 - 65000");

            string[] args = {"-d", "C:\\", "-p", "hello"};
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.Null(serverMade);
            mockPrinter.VerifyPrint(correctOutput.ToString());
        }


        [Fact]
        public void Make_Dirctory_Server_Inncorect_Correct()
        {
            var mockPrinter = new MockPrinter();
            var correctOutput = new StringBuilder();

            string[] args = {"-p", "32000", "-d", "-d"};
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.Null(serverMade);

            mockPrinter.VerifyPrint("Not a vaild directory");
        }


        [Fact]
        public void Make_Hello_World_Server_Correct()
        {
            var mockPrinter = new MockPrinter();
            string[] args = {"-p", "9560"};
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.NotNull(serverMade);
        }

        [Fact]
        public void Make_Hello_World_Incorrect_Correct()
        {
            var mockPrinter = new MockPrinter();
            string[] args = {"2750", "-p"};
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.Null(serverMade);
        }

        [Fact]
        public void Make_Hello_World_Incorrect_Correct_No_Port()
        {
            var mockPrinter = new MockPrinter();
            string[] args = {"-p", "-p"};
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
            correctOutput.Append("or -p PORT -d DIRECTORY\n");
            correctOutput.Append("Examples:\n");
            correctOutput.Append("Server.exe -p 8080 -d C:/\n");
            correctOutput.Append("Server.exe -d C:/HelloWorld -p 5555\n");
            correctOutput.Append("Server.exe -p 9999");

            var args = new[] {"-s"};
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.Null(serverMade);
            mockPrinter.VerifyPrint(correctOutput.ToString());
        }

        [Fact]
        public void Main_Starting_Program()
        {
            string[] args = {};
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

        [Fact]
        public void Make_Log_File()
        {
            var mockPrinter = new MockPrinter();

            string[] args = {"-p", "10560", "-l", "c:/TestLog"};
            var serverMade = Program.MakeServer(args, mockPrinter);
            Assert.Equal("c:/TestLog", mockPrinter.Log);
            Assert.NotNull(serverMade);
        }
    }
}