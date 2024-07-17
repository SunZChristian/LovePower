using System;
using System.Collections.Generic;

public partial class HandlePlayerMsg
{
	//获取房间列表
	public void MsgGetRoomList(Player player, ProtocolBase protoBase)
	{
		player.Send (RoomMgr.instance.GetRoomList());
	}

	//创建房间
	public void MsgCreateRoom(Player player, ProtocolBase protoBase)
	{
        //ProtocolBytes protocol = new ProtocolBytes ();
        //protocol.AddString ("CreateRoom");
        ////条件检测
        //if (player.tempData.status != PlayerTempData.Status.None) 
        //{
        //	Console.WriteLine ("MsgCreateRoom Fail " + player.id);
        //	protocol.AddInt(-1);
        //	player.Send (protocol);
        //	return;
        //}
        //RoomMgr.instance.CreateRoom (player);
        //protocol.AddInt(0);
        //player.Send (protocol);
        //Console.WriteLine ("MsgCreateRoom Ok " + player.id);

        ProtocolBuf protocol = new ProtocolBuf();
        
        SCCreateRoom msg = new SCCreateRoom();

        if (player.tempData.status != PlayerTempData.Status.None)
        {
            //已经在房间中
            msg.Code = 101;
            msg.Message = "已经在房间中了";
        }
        else 
        {
            RoomMgr.instance.CreateRoom(player,out var resultCode);
            if (resultCode == 200)
            {
                msg.Code = 200;
                msg.Message = "创建成功";
            }
            else if(resultCode == 101)
            {
                msg.Code = 101;
                msg.Message = "房间已存在,创建失败";
            }
        }
        protocol.Serialize<SCCreateRoom>(msg);
        player.Send(protocol);
    }

	//加入房间
	public void MsgEnterRoom(Player player, ProtocolBase protoBase)
	{
		ProtocolBuf protocol = new ProtocolBuf();

		SCJoinRoom msg = new SCJoinRoom();

		//判断房间是否存在
		if (RoomMgr.instance.list.Count <= 0)
		{
			//还没有创建房间
			msg.Code = 1001;
			msg.Message = "还没有创建房间";
		}
		else
		{
			//房间存在
			var room = RoomMgr.instance.list[0];
			if (room.status == Room.Status.Prepare)
			{
				//还在准备
			}
			else if (room.status == Room.Status.Playing)
			{ 
				//正在播放，同步下目前的时间
			}

			//添加玩家
			if (room.AddPlayer(player))
			{
				msg.Code = 200;
				msg.Message = "进入成功";

				//给房主发条消息，有人成功进入了
				MsgOtherPlayerEnter();
			}
			else
			{
				msg.Code = 1002;
				msg.Message = "房间人数已满？这不可能！";
			}
        }

		protocol.Serialize<SCJoinRoom>(msg);
		player.Send(protocol);
    }

    //获取房间信息
    public void MsgGetRoomInfo(Player player, ProtocolBase protoBase)
	{
		
		if (player.tempData.status != PlayerTempData.Status.Room) 
		{
			Console.WriteLine ("MsgGetRoomInfo status err " + player.id);
			return;
		}
		Room room = player.tempData.room;
		player.Send (room.GetRoomInfo());
	}

	//离开房间
	public void MsgLeaveRoom(Player player, ProtocolBase protoBase)
	{
		ProtocolBytes protocol = new ProtocolBytes ();
		protocol.AddString ("LeaveRoom");

        //Console.WriteLine("MsgLeaveRoom status err player.tempData.status  " + player.tempData.status + "   " + PlayerTempData.Status.Room);

        /*
        //条件检测
        if (player.tempData.status != PlayerTempData.Status.Room) 
		{
			Console.WriteLine ("MsgLeaveRoom status err " + player.id);
			protocol.AddInt (-1);
			player.Send (protocol);
			return;
		}*/

		//处理
		protocol.AddInt (0);
		player.Send (protocol);
		Room room = player.tempData.room;
		RoomMgr.instance.LeaveRoom (player);
		//广播
		if(room != null)
			room.Broadcast(room.GetRoomInfo());
	}

    public void MsgOtherPlayerEnter()
    {
        ProtocolBuf protocol = new ProtocolBuf();

        
    }
}