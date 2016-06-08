using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Microsoft.FSharp.Collections;
using Server.Core;
using TicTacToe.Core;

namespace TicTacToeServer.Core
{
    public class TicTacToeService : IHttpServiceProcessor
    {
        public bool CanProcessRequest(string request, ServerProperties serverProperties)
        {
            return CleanRequest(request) == "/";
        }

        public IHttpResponse ProcessRequest(string request, IHttpResponse httpResponse,
            ServerProperties serverProperties)
        {
            return request.Contains("GET /")
                ? GetRequest(httpResponse)
                : PostRequest(request, httpResponse, serverProperties);
        }

        private IHttpResponse PostRequest(string request, IHttpResponse httpResponse,
            ServerProperties serverProperties)
        {
            var game = ((TicTacToeGame) serverProperties.PersistentData);
            var data = WebUtility.UrlDecode(request.Remove(0
                , request.LastIndexOf("\r\n\r\n", StringComparison.Ordinal) + 4));

            var move = data.Substring(0, data.IndexOf("&", StringComparison.Ordinal))
                .Replace("move=", "");

            var ticTacToeBox =
                new TicTacToeBoxClass.TicTacToeBox(
                    ListModule.OfSeq(GetBoxValues(data)));

            ticTacToeBox = (TicTacToeBoxClass.TicTacToeBox)
                game.Play(ticTacToeBox,
                    CleanInput.SanitizeHumanPickedPlace(move, 9));

            var form = MakeForm(ticTacToeBox);
            if (game.CheckForWinner(ticTacToeBox))
            {
                form = form.Replace(@"Input Move:<br>", "");
                form = form.Replace(@"<input type=""text"" name=""move""><br>", "");
                form = form.Replace(@"<input type=""submit"" value=""Submit"">", "");
            }
            httpResponse.HttpStatusCode = "200 OK";
            httpResponse.CacheControl = "no-cache";
            httpResponse.ContentType = "text/html";

            httpResponse.Body = HtmlHeader() + form + HtmlTail();
            return httpResponse;
        }

        private List<string> GetBoxValues(string data)
        {
            var list = new List<string>();
            for (var i = 0; data.Contains("&"); i++)
            {
                data = data.Remove(0, data.IndexOf("&"
                    , StringComparison.Ordinal) + 1);
                var startPos = data.IndexOf("=", StringComparison.Ordinal) + 1;
                var legnth = (data.IndexOf("&", StringComparison.Ordinal)) == -1
                    ? data.Length - startPos
                    : (data.IndexOf("&", StringComparison.Ordinal)) - startPos;
                list.Add(data.Substring(startPos, legnth));
            }
            return list;
        }

        private IHttpResponse GetRequest(IHttpResponse httpResponse)
        {
            var ticTacToeBox = new TicTacToeBoxClass.TicTacToeBox(Game.makeTicTacToeBox(3));
            httpResponse.HttpStatusCode = "200 OK";
            httpResponse.CacheControl = "no-cache";
            httpResponse.ContentType = "text/html";
            httpResponse.Body = HtmlHeader() + MakeForm(ticTacToeBox) + HtmlTail();
            return httpResponse;
        }

        private string CleanRequest(string request)
        {
            var parseVaulue = request.Contains("GET") ? "GET" : "POST";
            var offsets = request.Contains("GET") ? 5 : 6;
            if (request.Contains("HTTP/1.1"))
                return "/" + request.Substring(request.IndexOf(parseVaulue + " /", StringComparison.Ordinal) + offsets,
                    request.IndexOf(" HTTP/1.1", StringComparison.Ordinal) - offsets)
                    .Replace("%20", " ");
            return "/" + request.Substring(request.IndexOf(parseVaulue + " /", StringComparison.Ordinal) + offsets,
                request.IndexOf(" HTTP/1.0", StringComparison.Ordinal) - offsets)
                .Replace("%20", " ");
        }

        private string HtmlHeader()
        {
            var header = new StringBuilder();
            header.Append(@"<!DOCTYPE html>");
            header.Append(@"<html>");
            header.Append(@"<head><title>Vatic TicTacToe</title></head>");
            header.Append(@"<body>");
            return header.ToString();
        }

        private string HtmlTail()
        {
            var tail = new StringBuilder();
            tail.Append(@"</body>");
            tail.Append(@"</html>");
            return tail.ToString();
        }

        private string MakeForm(ITicTacToeBoxClass.ITicTacToeBox ticTacToeBox)
        {
            var formPage = new StringBuilder();
            formPage.Append(@"<table style=""width: 100 % "">");
            for (var i = 0; i < ticTacToeBox.cellCount(); i += 3)
            {
                formPage.Append("<tr>");
                for (var k = 0; k < ticTacToeBox.victoryCellCount(); k++)
                    formPage.Append(
                        $"<td>{WebUtility.HtmlEncode(ticTacToeBox.getGlyphAtLocation(i + k))}</td>");
                formPage.Append("</tr>");
            }
            formPage.Append(@"</table>");
            formPage.Append(@"<form action=""/"" method=""post"">");
            formPage.Append(@"Input Move:<br>");
            formPage.Append(@"<input type=""text"" name=""move""><br>");
            formPage.Append(@"<input type=""submit"" value=""Submit"">");
            for (var i = 0; i < ticTacToeBox.cellCount(); i++)
                formPage.Append(@"<input type=""hidden"" name=""pos" + i + @""" value=""" +
                                ticTacToeBox.getGlyphAtLocation(i) + @"""><br>");
            formPage.Append(@"</form>");

            return formPage.ToString();
        }
    }
}