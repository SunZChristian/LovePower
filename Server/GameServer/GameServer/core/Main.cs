using System;

namespace Serv
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			RoomMgr roomMgr = new RoomMgr ();
			//DataMgr dataMgr = new DataMgr ();
			ServNet servNet = new ServNet();
			servNet.proto = new ProtocolBuf();
			servNet.proto.Initialize();
			servNet.Start("127.0.0.1",1234);

			while(true)
			{
				string str = Console.ReadLine();
				switch(str)
				{
				case "quit":
					servNet.Close();
					return;
				case "print":
					servNet.Print();
					break;
				}
			}

		}
	}
}
