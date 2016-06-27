using System.Text;
using Server.Core;
using TicTacToeServer.Core;
using Xunit;

namespace TicTacToeServer.Test
{
    public class Service404Test
    {
        [Fact]
        public void Make_New_Class_Not_Null()
        {
            Assert.NotNull(new Service404());
        }

        [Fact]
        public void Cant_Process_Anything()
        {
            var service404 = new Service404();
            Assert.False(service404.CanProcessRequest("", null));
        }

        [Fact]
        public void Error_Message()
        {
            var zSocket = new MockZSocket();
            var correctOutput = new StringBuilder();
            correctOutput.Append(@"<!DOCTYPE html>");
            correctOutput.Append(@"<html>");
            correctOutput.Append(@"<head><title>Vatic Server 404 Error Page</title></head>");
            correctOutput.Append(@"<body>");
            correctOutput.Append(@"<h1>404, Can not process request on port 5555</h1>");
            correctOutput.Append(@"</body>");
            correctOutput.Append(@"</html>");
            var serverProperties = new ServerProperties(null,
                5555, new ServerTime(),
                new MockPrinter());
            var service404 = new Service404();
            service404.ProcessRequest("", new HttpResponse(zSocket),
                serverProperties);

            zSocket.VerifySend(GetByte("HTTP/1.1 404 Not Found\r\n"),
                GetByteCount("HTTP/1.1 404 Not Found\r\n"));
            zSocket.VerifySend(GetByte("Cache-Control: no-cache\r\n"),
                GetByteCount("Cache-Control: no-cache\r\n"));
            zSocket.VerifySend(GetByte("Content-Type: text/html\r\n"),
                GetByteCount("Content-Type: text/html\r\n"));
            zSocket.VerifySend(GetByte("Content-Length: "
                + GetByteCount(correctOutput.ToString())
                + "\r\n\r\n"),
                GetByteCount("Content-Length: "
                + GetByteCount(correctOutput.ToString())
                + "\r\n\r\n"));

            zSocket.VerifySend(GetByte(correctOutput.ToString()),
                GetByteCount(correctOutput.ToString()));
        }
        private int GetByteCount(string message)
        {
            return Encoding.ASCII.GetByteCount(message);
        }

        private byte[] GetByte(string message)
        {
            return Encoding.ASCII.GetBytes(message);
        }
    }
}