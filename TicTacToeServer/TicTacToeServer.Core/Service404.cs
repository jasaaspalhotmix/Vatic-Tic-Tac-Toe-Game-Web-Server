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

        public IHttpResponse ProcessRequest(string request, 
            IHttpResponse httpResponse,
            ServerProperties serverProperties)
        {
            httpResponse.HttpStatusCode = "404 Not Found";
            httpResponse.CacheControl = "no-cache";
            httpResponse.ContentType = "text/html";
            var errorPage = new StringBuilder();
            errorPage.Append(@"<!DOCTYPE html>");
            errorPage.Append(@"<html>");
            errorPage.Append(@"<head><title>Vatic Server 404 Error Page</title></head>");
            errorPage.Append(@"<body>");
            errorPage.Append(@"<h1>404, Can not process request on port " + serverProperties.Port + "</h1>");
            errorPage.Append(@"</body>");
            errorPage.Append(@"</html>");
            httpResponse.Body = errorPage.ToString();
            httpResponse.ContentLength =
                Encoding.ASCII.GetByteCount(httpResponse.Body);

            return httpResponse;
        }
    }
}