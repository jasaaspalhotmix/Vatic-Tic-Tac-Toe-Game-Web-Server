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
            var correctOutput = new StringBuilder();
            correctOutput.Append(@"<!DOCTYPE html>");
            correctOutput.Append(@"<html>");
            correctOutput.Append(@"<head><title>Vatic Server 404 Error Page</title></head>");
            correctOutput.Append(@"<body>");
            correctOutput.Append(@"<h1>404, Can not process request on port 5555</h1>");
            correctOutput.Append(@"</body>");
            correctOutput.Append(@"</html>");
            var httpPackage = new HttpResponse();
            var serverProperties = new ServerProperties(null,
                5555, new HttpResponse(), new ServerTime(),
                new MockPrinter());
            var service404 = new Service404();
            httpPackage = (HttpResponse)service404.ProcessRequest("", httpPackage, serverProperties);
            Assert.Equal("404 Not Found", httpPackage.HttpStatusCode);
            Assert.Equal(correctOutput.ToString(), httpPackage.Body);
        }
    }
}