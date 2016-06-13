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
                .Replace("box=", "");

            if (move.StartsWith("-") && move.EndsWith("-") && move.Length > 2)
                move = move.Replace("-", "");

            var ticTacToeBox =
                new TicTacToeBoxClass.TicTacToeBox(
                    ListModule.OfSeq(GetBoxValues(data)));

            var errorMesageCode = Game.isUserInputCorrect(ticTacToeBox,
                move, game.Setting.playerGlyph, game.Setting.aIGlyph);

            if (errorMesageCode == Translate.Blank)
                ticTacToeBox = (TicTacToeBoxClass.TicTacToeBox)
                    game.Play(ticTacToeBox,
                        CleanInput.SanitizeHumanPickedPlace(move, 9));

            httpResponse.HttpStatusCode = "200 OK";
            httpResponse.CacheControl = "no-cache";
            httpResponse.ContentType = "text/html";

            httpResponse.Body = HtmlHeader() +
                                Form(ticTacToeBox,
                                    game, errorMesageCode, serverProperties)
                                + HtmlTail();
            return httpResponse;
        }

        private string Form(ITicTacToeBoxClass.ITicTacToeBox ticTacToeBox,
            TicTacToeGame game, int errorMesageCode, ServerProperties serverProperties)
        {
            var form = MakeForm(ticTacToeBox);
            form = RemoveButton(game.Setting.playerGlyph, form);
            form = RemoveButton(game.Setting.aIGlyph, form);
            var errorMessage = errorMesageCode != Translate.Blank
                ? "<p>" +
                  Translator.translator(Translator.language.english,
                      errorMesageCode) + "</p>"
                : "";
            form = errorMessage += form;
            if (!game.CheckForWinner((TicTacToeBoxClass.TicTacToeBox) ticTacToeBox))
                return form;
            form = "<p>Game Over</p>"
                   + @"<a href=""http://127.0.0.1:" + serverProperties.Port
                   + @"""><button>Another Game?</button></a>"
                   + form;
            for (var i = 0; i < ticTacToeBox.cellCount(); i++)
            {
                form = RemoveButton("-" + (i + 1) + "-", form);
            }
            return form;
        }

        private string RemoveButton(string symbol, string form)
        {
            return form.Replace(@"<button name=box type=""submit"" value="""
                                + symbol + @""">"
                                + symbol + "</button>",
                symbol);
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
            formPage.Append(@"<form action=""/"" method=""post"">");
            formPage.Append(@"<table style=""width: 100 % "">");
            for (var i = 0; i < ticTacToeBox.cellCount(); i += 3)
            {
                formPage.Append("<tr>");
                for (var k = 0; k < ticTacToeBox.victoryCellCount(); k++)
                    formPage.Append(
                        @"<td><button name=box type=""submit"" value=""" +
                        WebUtility.HtmlEncode(ticTacToeBox.getGlyphAtLocation(i + k))
                        + @""">" +
                        $"{WebUtility.HtmlEncode(ticTacToeBox.getGlyphAtLocation(i + k))}" +
                        "</button></td>");
                formPage.Append("</tr>");
            }
            formPage.Append(@"</table>");
            for (var i = 0; i < ticTacToeBox.cellCount(); i++)
                formPage.Append(@"<input type=""hidden"" name=""pos" + i + @""" value=""" +
                                ticTacToeBox.getGlyphAtLocation(i) + @"""><br>");
            formPage.Append(@"</form>");

            return formPage.ToString();
        }
    }
}