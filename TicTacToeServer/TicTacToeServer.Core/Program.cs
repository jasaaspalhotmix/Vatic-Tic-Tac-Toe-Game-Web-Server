using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Server.Core;
using TicTacToe.Core;

namespace TicTacToeServer.Core
{
    public class Program
    {
        private const int LowerBoundPort = 2000;
        private const int UpperBoundPort = 65000;
        private const string PortOption = "-p";

        public static int Main(string[] args)
        {
            RunServer(MakeServer(args, new DefaultPrinter()));
            return 0;
        }

        public static void RunServer(IMainServer runningServer)
        {
            if (runningServer == null) return;
            do
            {
                runningServer.Run();
            } while (runningServer.AcceptingNewConn);
        }

        private static string ParseArg(string option,
            IReadOnlyList<string> args)
        {
            for (var i = 0; i < args.Count; i++)
            {
                if (args[i] == option && i + 1 < args.Count)
                    return args[i + 1];
            }
            return null;
        }

        private static bool IsArrayClean(IReadOnlyList<string> args)
        {
            var outref = 0;
            if (args.Count > 0)
                return
                    args.Where(arg => arg.Contains("-")
                                      && !int.TryParse(arg, out outref))
                        .All(arg => arg == PortOption);
            return false;
        }

        public static IMainServer MakeServer(string[] args, IPrinter io)
        {
            var port = ParseArg(PortOption, args);
            var arrayClean = IsArrayClean(args);
            try
            {
                if (port != null && arrayClean)
                    return TicTacToeServer(port, io);
                io.Print(WrongNumberOfArgs());
                return null;
            }
            catch (Exception)
            {
                io.Print("Another Server is running on that port");
                return null;
            }
        }

        public static IMainServer TicTacToeServer(string port,
            IPrinter io)
        {
            var portConverted = PortWithinRange(port, io);
            if (portConverted == -1) return null;
            var endPoint = new IPEndPoint((IPAddress.Loopback),
                portConverted);
            var zSocket = new DefaultZSocket(endPoint);
            var properties = new ServerProperties(null,
                portConverted,
                new ServerTime(), io,
                new TicTacToeGame(new User(), new Ai(),
                    MakeSettings()));
            return new MainServer(zSocket, properties,
                new HttpServiceFactory(new Service404()),
                new DefaultRequestProcessor(),
                new List<string> { "TicTacToeServer.Core" },
                new List<Assembly> { Assembly.GetExecutingAssembly() });
        }


        private static int PortWithinRange(string port, IPrinter io)
        {
            int portconvert;
            if (int.TryParse(port, out portconvert))
            {
                if (portconvert >= LowerBoundPort && portconvert
                    <= UpperBoundPort) return portconvert;
                io.Print(GetInvaildPortError());
                return -1;
            }
            io.Print(GetInvaildPortError());
            return -1;
        }

        private static string WrongNumberOfArgs()
        {
            var error = new StringBuilder();
            error.Append("Invaild Number of Arguments.\n");
            error.Append("Can only be -p PORT\n");
            error.Append("Examples:\n");
            error.Append("Server.exe -p 8080\n");
            error.Append("Server.exe -p 9999");

            return error.ToString();
        }

        private static string GetInvaildPortError()
        {
            var error = new StringBuilder();
            error.Append("Invaild Port Detected.");
            error.Append("Vaild Ports 2000 - 65000");

            return error.ToString();
        }

        private static GameSettings.gameSetting MakeSettings()
        {
            return new GameSettings.gameSetting(3, "x", "@"
                , (int)PlayerValues.playerVals.Human
                , false, false, false);
        }
    }
}