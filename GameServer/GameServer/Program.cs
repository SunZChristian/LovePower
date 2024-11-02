using AhpilyServer;
using System;
using System.Collections.Generic;

namespace GameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            EncodeTool.Initialize();

            ServerPeer server = new ServerPeer();
            //指定所关联的应用
            server.SetApplication(new NetMsgCenter());
            server.Start("127.0.0.1",1234, 10);

            Console.ReadKey();
        }
    }
}
