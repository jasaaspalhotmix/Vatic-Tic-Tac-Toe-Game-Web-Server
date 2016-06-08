using System.Collections.Generic;
using System.Net;
using System.Reflection;
using Server.Core;
using TicTacToe.Core;

namespace TicTacToeServer.Core
{
    public class Program
    {
        private static void Main(string[] args)
        {

            var settings =
                new GameSettings.gameSetting(3, "x", "@"
                    , (int) PlayerValues.playerVals.Human
                    , false, false, false);

            var endPoint = new IPEndPoint((IPAddress.Loopback), 8080);
            var zSocket = new ZSocket(endPoint);
            var properties = new ServerProperties("c:/", new DirectoryProcessor(), new FileProcessor(), 8080,
                new HttpResponse(), new ServerTime(), new Printer(),
                new TicTacToeGame(new User(), new Ai(), settings));
            var server = new MainServer(zSocket, properties,
                new HttpServiceFactory(new Service404()),
                new List<string>() { "TicTacToeServer.Core" },
                new List<Assembly>() { Assembly.GetExecutingAssembly() });

            while (true)
            {
                server.Run();
            }
        }
    }
}