using System.Collections.Generic;
using System.Text;
using Server.Core;

namespace TicTacToeServer.Core
{
    public class Service404 : IHttpServiceProcessor
    {
        public bool CanProcessRequest(string request,
            ServerProperties serverProperties)
        {
            return false;
        }

        public string ProcessRequest(string request,
            IHttpResponse httpResponse,
            ServerProperties serverProperties)
        {
            var errorPage = new StringBuilder();
            errorPage.Append(@"<!DOCTYPE html>");
            errorPage.Append(@"<html>");
            errorPage.Append(@"<head><title>Vatic Server 404 Error Page</title></head>");
            errorPage.Append(@"<body>");
            errorPage.Append(@"<h1>404, Can not process request on port " + serverProperties.Port + "</h1>");
            errorPage.Append(@"</body>");
            errorPage.Append(@"</html>");
            httpResponse.SendHeaders(new List<string>
            {
                "HTTP/1.1 404 Not Found\r\n",
                "Cache-Control: no-cache\r\n",
                "Content-Type: text/html\r\n",
                "Content-Length: "
                + (Encoding.ASCII.GetByteCount(errorPage.ToString())) +
                "\r\n\r\n"
            });

            httpResponse.SendBody(Encoding
                .ASCII.GetBytes(errorPage.ToString()),
                Encoding.ASCII.GetByteCount(errorPage.ToString()));

            return "404 Not Found";
        }
    }
}