using System.Collections.Generic;
using System.Text;
using Microsoft.FSharp.Collections;
using Server.Core;
using TicTacToe.Core;
using TicTacToeServer.Core;
using Xunit;

namespace TicTacToeServer.Test
{
    public class TicTacToeServiceTest
    {
        [Fact]
        public void Make_Not_Null_Class()
        {
            Assert.NotNull(new TicTacToeService());
        }

        [Theory]
        [InlineData("GET / HTTP/1.1")]
        [InlineData("GET / HTTP/1.0")]
        [InlineData("POST / HTTP/1.1")]
        [InlineData("POST / HTTP/1.0")]
        public void Can_Process_Root(string request)
        {
            var service = new TicTacToeService();
            var serverProperties = new ServerProperties(null,
                5555, new ServerTime(),
                new MockPrinter());
            Assert.True(service.CanProcessRequest(request, 
                serverProperties));
        }

        [Theory]
        [InlineData("GET /few HTTP/1.1")]
        [InlineData("GET /wefw HTTP/1.0")]
        [InlineData("POST /qwqw HTTP/1.1")]
        [InlineData("POST /q HTTP/1.0")]
        public void Cant_Process_Root(string request)
        {
            var service = new TicTacToeService();
            var serverProperties = new ServerProperties(null,
                5555, new ServerTime(),
                new MockPrinter());
            Assert.False(service.CanProcessRequest(request, 
                serverProperties));
        }

        [Theory]
        [InlineData("GET / HTTP/1.1")]
        [InlineData("GET / HTTP/1.0")]
        public void Get_Request(string request)
        {
            var formPage = new StringBuilder();
            formPage.Append(@"<!DOCTYPE html>");
            formPage.Append(@"<html>");
            formPage.Append(@"<head><title>Vatic TicTacToe</title></head>");
            formPage.Append(@"<body>");
            formPage.Append(@"<form action=""/"" method=""post"">");
            formPage.Append(
                @"<table style=""width: 100 % "">" +
                "<tr>" +
                @"<td><button name=box type=""submit"" value=""-1-"">-1-</button></td>" +
                @"<td><button name=box type=""submit"" value=""-2-"">-2-</button></td>" +
                @"<td><button name=box type=""submit"" value=""-3-"">-3-</button></td>" +
                "</tr>" +
                "<tr>" +
                @"<td><button name=box type=""submit"" value=""-4-"">-4-</button></td>" +
                @"<td><button name=box type=""submit"" value=""-5-"">-5-</button></td>" +
                @"<td><button name=box type=""submit"" value=""-6-"">-6-</button></td>" +
                "</tr>" +
                "<tr>" +
                @"<td><button name=box type=""submit"" value=""-7-"">-7-</button></td>" +
                @"<td><button name=box type=""submit"" value=""-8-"">-8-</button></td>" +
                @"<td><button name=box type=""submit"" value=""-9-"">-9-</button></td>" +
                "</tr>" +
                "</table>"
                );
            formPage.Append(@"<input type=""hidden"" name=""pos" + 0 + @""" value=""-" +
                            1 + @"-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 1 + @""" value=""-" +
                            2 + @"-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 2 + @""" value=""-" +
                            3 + @"-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 3 + @""" value=""-" +
                            4 + @"-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 4 + @""" value=""-" +
                            5 + @"-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 5 + @""" value=""-" +
                            6 + @"-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 6 + @""" value=""-" +
                            7 + @"-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 7 + @""" value=""-" +
                            8 + @"-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 8 + @""" value=""-" +
                            9 + @"-""><br>");
            formPage.Append(@"</form>");
            formPage.Append(@"</body>");
            formPage.Append(@"</html>");

            var zSocket = new MockZSocket();
            var serverProperties = new ServerProperties(null,
                5555, new ServerTime(),
                new MockPrinter());
            var service = new TicTacToeService();
            var statueCode = service.ProcessRequest(request, 
                new HttpResponse(zSocket), 
                serverProperties);

            Assert.Equal("200 OK", statueCode);
            zSocket.VerifySend(GetByte("HTTP/1.1 200 OK\r\n"),
                GetByteCount("HTTP/1.1 200 OK\r\n"));
            zSocket.VerifySend(GetByte("Cache-Control: no-cache\r\n"),
                GetByteCount("Cache-Control: no-cache\r\n"));
            zSocket.VerifySend(GetByte("Content-Type: text/html\r\n"),
                GetByteCount("Content-Type: text/html\r\n"));
            zSocket.VerifySend(GetByte("Content-Length: "
                + GetByteCount(formPage.ToString())
                + "\r\n\r\n"),
                GetByteCount("Content-Length: "
                + GetByteCount(formPage.ToString())
                + "\r\n\r\n"));

            zSocket.VerifySend(GetByte(formPage.ToString()),
                GetByteCount(formPage.ToString()));
        }

        [Fact]
        public void Game_Is_Going()
        {
            var userTicTacToeBox = new List<string>
                    {
                        "x",
                        "-2-",
                        "-3-",
                        "-4-",
                        "-5-",
                        "-6-",
                        "-7-",
                        "-8-",
                        "-9-"
                    };

            var aITicTacToeBox = new List<string>
                    {
                        "x",
                        "@",
                        "-3-",
                        "-4-",
                        "-5-",
                        "-6-",
                        "-7-",
                        "-8-",
                        "-9-"
                    };

            var settings = new GameSettings.gameSetting(3, "x", "@"
                , (int)PlayerValues.playerVals.Human
                , false, false, false);
            var formPage = new StringBuilder();
            formPage.Append(@"<!DOCTYPE html>");
            formPage.Append(@"<html>");
            formPage.Append(@"<head><title>Vatic TicTacToe</title></head>");
            formPage.Append(@"<body>");
            formPage.Append(@"<form action=""/"" method=""post"">");
            formPage.Append(
                @"<table style=""width: 100 % "">" +
                "<tr>" +
                @"<td>x</td>" +
                @"<td>@</td>" +
                @"<td><button name=box type=""submit"" value=""-3-"">-3-</button></td></tr>" +
                "<tr>" +
                @"<td><button name=box type=""submit"" value=""-4-"">-4-</button></td>" +
                @"<td><button name=box type=""submit"" value=""-5-"">-5-</button></td>" +
                @"<td><button name=box type=""submit"" value=""-6-"">-6-</button></td></tr>" +
                "<tr>" +
                @"<td><button name=box type=""submit"" value=""-7-"">-7-</button></td>" +
                @"<td><button name=box type=""submit"" value=""-8-"">-8-</button></td>" +
                @"<td><button name=box type=""submit"" value=""-9-"">-9-</button></td></tr>"
                + "</table>"
                );
            formPage.Append(@"<input type=""hidden"" name=""pos" + 0 + @""" value=""x""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 1 + @""" value=""@""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 2 + @""" value=""-3-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 3 + @""" value=""-4-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 4 + @""" value=""-5-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 5 + @""" value=""-6-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 6 + @""" value=""-7-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 7 + @""" value=""-8-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 8 + @""" value=""-9-""><br>");
            formPage.Append(@"</form>");
            formPage.Append(@"</body>");
            formPage.Append(@"</html>");
            var service = new TicTacToeService();
            var serverProperties = new ServerProperties(null,
                5555, new ServerTime(),
                new MockPrinter(),
                new TicTacToeGame(new MockUser()
                    .StubMove(new TicTacToeBoxClass.TicTacToeBox(
                        ListModule.OfSeq(userTicTacToeBox)))
                     , new MockAi()
                     .StubMove(new TicTacToeBoxClass.TicTacToeBox(
                        ListModule.OfSeq(aITicTacToeBox))),
                    settings));
            var request = new StringBuilder();
            request.Append("POST / HTTP/1.1\r\n");
            request.Append("Host: localhost:8080\r\n");
            request.Append("Connection: keep-alive\r\n");
            request.Append("Content-Length: 86\r\n");
            request.Append("Cache-Control: max-age=0\r\n");
            request.Append("Origin: http://localhost:8080\r\n");
            request.Append("Upgrade-Insecure-Requests: 1\r\n");
            request.Append(
                "User-Agent: Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36\r\n");
            request.Append("Content-Type: application/x-www-form-urlencoded\r\n");
            request.Append("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8\r\n");
            request.Append("Referer: http://localhost:8080/\r\n");
            request.Append("Accept-Encoding: gzip, deflate\r\n");
            request.Append("Accept-Language: en-US,en;q=0.8\r\n\r\n");

            request.Append("box=-1-&pos0=-1-&pos1=-2-&pos2=-3-&pos3=-4-&pos4=-5-&pos5=-6-&pos6=-7-&pos7=-8-&pos8=-9-");

            var zSocket = new MockZSocket();
            var statueCode = service.ProcessRequest(request.ToString(), 
                new HttpResponse(zSocket), 
                serverProperties);

            Assert.Equal("200 OK", statueCode);
            zSocket.VerifySend(GetByte("HTTP/1.1 200 OK\r\n"),
                GetByteCount("HTTP/1.1 200 OK\r\n"));
            zSocket.VerifySend(GetByte("Cache-Control: no-cache\r\n"),
                GetByteCount("Cache-Control: no-cache\r\n"));
            zSocket.VerifySend(GetByte("Content-Type: text/html\r\n"),
                GetByteCount("Content-Type: text/html\r\n"));
            zSocket.VerifySend(GetByte("Content-Length: "
                + GetByteCount(formPage.ToString())
                + "\r\n\r\n"),
                GetByteCount("Content-Length: "
                + GetByteCount(formPage.ToString())
                + "\r\n\r\n"));
        }

        [Fact]
        public void Game_Is_Over()
        {
            var userTicTacToeBox = new List<string>
                    {
                        "x",
                        "x",
                        "x",
                        "x",
                        "x",
                        "x",
                        "x",
                        "x",
                        "x"
                    };

            var aITicTacToeBox = new List<string>
                    {
                        "x",
                        "x",
                        "x",
                        "x",
                        "x",
                        "x",
                        "x",
                        "x",
                        "x"
                    };

            var settings = new GameSettings.gameSetting(3, "x", "@"
                , (int)PlayerValues.playerVals.Human
                , false, false, false);
            var formPage = new StringBuilder();
            formPage.Append(@"<!DOCTYPE html>");
            formPage.Append(@"<html>");
            formPage.Append(@"<head><title>Vatic TicTacToe</title></head>");
            formPage.Append(@"<body>");
            formPage.Append(@"<p>Game Over</p>");
            formPage.Append(@"<a href=""http://127.0.0.1:5555""><button>Another Game?</button></a>");
            formPage.Append(@"<form action=""/"" method=""post"">");
            formPage.Append(
                @"<table style=""width: 100 % "">" +
                "<tr>" +
                @"<td>-1-</td>" +
                @"<td>x</td>" +
                @"<td>x</td></tr>" +
                "<tr>" +
                @"<td>x</td>" +
                @"<td>x</td>" +
                @"<td>x</td></tr>" +
                "<tr>" +
                @"<td>x</td>" +
                @"<td>x</td>" +
                @"<td>-9-</td></tr>"
                + "</table>"
                );
            formPage.Append(@"<input type=""hidden"" name=""pos" + 0 + @""" value=""-1-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 1 + @""" value=""x""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 2 + @""" value=""x""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 3 + @""" value=""x""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 4 + @""" value=""x""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 5 + @""" value=""x""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 6 + @""" value=""x""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 7 + @""" value=""x""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 8 + @""" value=""-9-""><br>");
            formPage.Append(@"</form>");
            formPage.Append(@"</body>");
            formPage.Append(@"</html>");
            var service = new TicTacToeService();
            var serverProperties = new ServerProperties(null,
                5555, new ServerTime(),
                new MockPrinter(),
                new TicTacToeGame(new MockUser()
                    .StubMove(new TicTacToeBoxClass.TicTacToeBox(
                        ListModule.OfSeq(userTicTacToeBox)))
                     , new MockAi()
                     .StubMove(new TicTacToeBoxClass.TicTacToeBox(
                        ListModule.OfSeq(aITicTacToeBox))),
                    settings));
            var request = new StringBuilder();
            request.Append("POST / HTTP/1.1\r\n");
            request.Append("Host: localhost:8080\r\n");
            request.Append("Connection: keep-alive\r\n");
            request.Append("Content-Length: 86\r\n");
            request.Append("Cache-Control: max-age=0\r\n");
            request.Append("Origin: http://localhost:8080\r\n");
            request.Append("Upgrade-Insecure-Requests: 1\r\n");
            request.Append(
                "User-Agent: Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36\r\n");
            request.Append("Content-Type: application/x-www-form-urlencoded\r\n");
            request.Append("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*;q=0.8\r\n");
            request.Append("Referer: http://localhost:8080/\r\n");
            request.Append("Accept-Encoding: gzip, deflate\r\n");
            request.Append("Accept-Language: en-US,en;q=0.8\r\n\r\n");

            request.Append("box=-9-&pos0=-1-&pos1=x&pos2=x&pos3=x&pos4=x&pos5=x&pos6=x&pos7=x&pos8=-9-");

            var zSocket = new MockZSocket();
            var statueCode = service.ProcessRequest(request.ToString(), 
                new HttpResponse(zSocket), 
                serverProperties);

            Assert.Equal("200 OK", statueCode);
            zSocket.VerifySend(GetByte("HTTP/1.1 200 OK\r\n"),
                GetByteCount("HTTP/1.1 200 OK\r\n"));
            zSocket.VerifySend(GetByte("Cache-Control: no-cache\r\n"),
                GetByteCount("Cache-Control: no-cache\r\n"));
            zSocket.VerifySend(GetByte("Content-Type: text/html\r\n"),
                GetByteCount("Content-Type: text/html\r\n"));
            zSocket.VerifySend(GetByte("Content-Length: "
                + GetByteCount(formPage.ToString())
                + "\r\n\r\n"),
                GetByteCount("Content-Length: "
                + GetByteCount(formPage.ToString())
                + "\r\n\r\n"));
        }

        [Fact]
        public void User_Moves()
        {

            var settings = new GameSettings.gameSetting(3, "x", "@"
                , (int)PlayerValues.playerVals.Human
                , false, false, false);
            var formPage = new StringBuilder();
            formPage.Append(@"<!DOCTYPE html>");
            formPage.Append(@"<html>");
            formPage.Append(@"<head><title>Vatic TicTacToe</title></head>");
            formPage.Append(@"<body>");
            formPage.Append(@"<p>Game Over</p>");
            formPage.Append(@"<a href=""http://127.0.0.1:5555""><button>Another Game?</button></a>");
            formPage.Append(@"<form action=""/"" method=""post"">");
            formPage.Append(
                @"<table style=""width: 100 % "">" +
                "<tr>" +
                @"<td>x</td>" +
                @"<td>@</td>" +
                @"<td>x</td></tr>" +
                "<tr>" +
                @"<td>x</td>" +
                @"<td>@</td>" +
                @"<td>x</td></tr>" +
                "<tr>" +
                @"<td>@</td>" +
                @"<td>x</td>" +
                @"<td>@</td></tr>"
                + "</table>"
                );
            formPage.Append(@"<input type=""hidden"" name=""pos" + 0 + @""" value=""x""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 1 + @""" value=""@""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 2 + @""" value=""x""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 3 + @""" value=""x""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 4 + @""" value=""@""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 5 + @""" value=""x""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 6 + @""" value=""@""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 7 + @""" value=""x""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 8 + @""" value=""@""><br>");
            formPage.Append(@"</form>");
            formPage.Append(@"</body>");
            formPage.Append(@"</html>");
            var service = new TicTacToeService();
            var serverProperties = new ServerProperties(null,
                5555, new ServerTime(),
                new MockPrinter(),
                new TicTacToeGame(new User(), new Ai(), settings));
            var request = new StringBuilder();
            request.Append("POST / HTTP/1.1\r\n");
            request.Append("Host: localhost:8080\r\n");
            request.Append("Connection: keep-alive\r\n");
            request.Append("Content-Length: 86\r\n");
            request.Append("Cache-Control: max-age=0\r\n");
            request.Append("Origin: http://localhost:8080\r\n");
            request.Append("Upgrade-Insecure-Requests: 1\r\n");
            request.Append(
                "User-Agent: Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36\r\n");
            request.Append("Content-Type: application/x-www-form-urlencoded\r\n");
            request.Append("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8\r\n");
            request.Append("Referer: http://localhost:8080/\r\n");
            request.Append("Accept-Encoding: gzip, deflate\r\n");
            request.Append("Accept-Language: en-US,en;q=0.8\r\n\r\n");

            request.Append("box=-1-&pos0=-1-&pos1=@&pos2=x&pos3=x&pos4=@&pos5=x&pos6=@&pos7=x&pos8=@");

            var zSocket = new MockZSocket();
            var statueCode = service.ProcessRequest(request.ToString(), 
                new HttpResponse(zSocket), 
                serverProperties);

            Assert.Equal("200 OK", statueCode);
            zSocket.VerifySend(GetByte("HTTP/1.1 200 OK\r\n"),
                GetByteCount("HTTP/1.1 200 OK\r\n"));
            zSocket.VerifySend(GetByte("Cache-Control: no-cache\r\n"),
                GetByteCount("Cache-Control: no-cache\r\n"));
            zSocket.VerifySend(GetByte("Content-Type: text/html\r\n"),
                GetByteCount("Content-Type: text/html\r\n"));
            zSocket.VerifySend(GetByte("Content-Length: "
                + GetByteCount(formPage.ToString())
                + "\r\n\r\n"),
                GetByteCount("Content-Length: "
                + GetByteCount(formPage.ToString())
                + "\r\n\r\n"));
        }

        [Fact]
        public void User_Moves_Spot_Taken()
        {

            var settings = new GameSettings.gameSetting(3, "x", "@"
                , (int)PlayerValues.playerVals.Human
                , false, false, false);
            var formPage = new StringBuilder();
            formPage.Append(@"<!DOCTYPE html>");
            formPage.Append(@"<html>");
            formPage.Append(@"<head><title>Vatic TicTacToe</title></head>");
            formPage.Append(@"<body>");
            formPage.Append(@"<p>Spot Taken</p>");
            formPage.Append(@"<form action=""/"" method=""post"">");
            formPage.Append(
                @"<table style=""width: 100 % "">" +
                "<tr>" +
                @"<td>x</td>" +
                @"<td>@</td>" +
                @"<td><button name=box type=""submit"" value=""-3-"">-3-</button></td></tr>" +
                "<tr>" +
                @"<td><button name=box type=""submit"" value=""-4-"">-4-</button></td>" +
                @"<td><button name=box type=""submit"" value=""-5-"">-5-</button></td>" +
                @"<td><button name=box type=""submit"" value=""-6-"">-6-</button></td></tr>" +
                "<tr>" +
                @"<td><button name=box type=""submit"" value=""-7-"">-7-</button></td>" +
                @"<td><button name=box type=""submit"" value=""-8-"">-8-</button></td>" +
                @"<td><button name=box type=""submit"" value=""-9-"">-9-</button></td></tr>"
                + "</table>"
                );

            formPage.Append(@"<input type=""hidden"" name=""pos" + 0 + @""" value=""x""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 1 + @""" value=""@""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 2 + @""" value=""-3-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 3 + @""" value=""-4-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 4 + @""" value=""-5-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 5 + @""" value=""-6-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 6 + @""" value=""-7-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 7 + @""" value=""-8-""><br>");
            formPage.Append(@"<input type=""hidden"" name=""pos" + 8 + @""" value=""-9-""><br>");
            formPage.Append(@"</form>");
            formPage.Append(@"</body>");
            formPage.Append(@"</html>");
            var service = new TicTacToeService();
            var serverProperties = new ServerProperties(null,
                5555, new ServerTime(),
                new MockPrinter(),
                new TicTacToeGame(new User(), new Ai(), settings));
            var request = new StringBuilder();
            request.Append("POST / HTTP/1.1\r\n");
            request.Append("Host: localhost:8080\r\n");
            request.Append("Connection: keep-alive\r\n");
            request.Append("Content-Length: 86\r\n");
            request.Append("Cache-Control: max-age=0\r\n");
            request.Append("Origin: http://localhost:8080\r\n");
            request.Append("Upgrade-Insecure-Requests: 1\r\n");
            request.Append(
                "User-Agent: Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36\r\n");
            request.Append("Content-Type: application/x-www-form-urlencoded\r\n");
            request.Append("Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*;q=0.8\r\n");
            request.Append("Referer: http://localhost:8080/\r\n");
            request.Append("Accept-Encoding: gzip, deflate\r\n");
            request.Append("Accept-Language: en-US,en;q=0.8\r\n\r\n");

            request.Append("box=1&pos0=x&pos1=@&pos2=-3-&pos3=-4-&pos4=-5-&pos5=-6-&pos6=-7-&pos7=-8-&pos8=-9-");

            var zSocket = new MockZSocket();
            var statueCode = service.ProcessRequest(request.ToString(),
                new HttpResponse(zSocket),
                serverProperties);

            Assert.Equal("200 OK", statueCode);
            zSocket.VerifySend(GetByte("HTTP/1.1 200 OK\r\n"),
                GetByteCount("HTTP/1.1 200 OK\r\n"));
            zSocket.VerifySend(GetByte("Cache-Control: no-cache\r\n"),
                GetByteCount("Cache-Control: no-cache\r\n"));
            zSocket.VerifySend(GetByte("Content-Type: text/html\r\n"),
                GetByteCount("Content-Type: text/html\r\n"));
            zSocket.VerifySend(GetByte("Content-Length: "
                + GetByteCount(formPage.ToString())
                + "\r\n\r\n"),
                GetByteCount("Content-Length: "
                + GetByteCount(formPage.ToString())
                + "\r\n\r\n"));
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


