/* ==============================================================================
 * 功能描述：HandleLandlordMsg  
 * 创 建 者：Administrator
 * 邮箱：136569631@qq.com
 * 版本：v 1.0
 * 创建日期：2019/6/4 13:22:42
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public partial class HandlePlayerMsg
{
    //private static readonly string obj = "lock";
    //public void MsgCountDown(Player player, ProtocolBase protoBase)
    //{
    //    //获取数值
    //    int start = 0;
    //    ProtocolBytes protocol = (ProtocolBytes)protoBase;
    //    string protoName = protocol.GetString(start, ref start);
    //    //获取房间
    //    if (player.tempData.status != PlayerTempData.Status.Fight)
    //        return;
    //    Room room = player.tempData.room;
    //    int num = protocol.GetInt(start, ref start);
    //    room.countNum = num;
    //    CountDownVoid(player, protoBase, room);
    //}

    ////独立线程检测并处理
    //private static void CountDownVoid(Player player, ProtocolBase protoBase,Room room)
    //{
    //    Queue<TaskPack> tpQue = new Queue<TaskPack>();
    //    CMTimer ct = new CMTimer(50);
    //    ct.SetLog((string info) =>
    //    {
    //        Console.WriteLine("ConsoleLog：" + info);
    //    });

    //    int id = ct.AddTimeTask((int tid) => {
    //        int countNum = room.countNum;
    //        if (countNum >= 0)
    //        {
    //            BraSend(player, protoBase, room);
    //        }
    //        else
    //        {
    //            ct.DelTimeTask(tid);
    //        }
            
    //    }, 1000, CMTimeUnit.MilliSecond, 0);

    //    //运行线程回归到主线程（一个线程）
    //    ct.SetHandle((Action<int> cb, int tid) =>{
    //        if(cb != null)
    //        {
    //            lock (obj)
    //            {
    //                tpQue.Enqueue(new TaskPack(tid, cb));
    //            }
    //        }
    //    });


    //    while (true)
    //    {
    //        /*
    //        string ipt = Console.ReadLine();
    //        if (ipt == "d")
    //        {
    //            ct.DelTimeTask(id);
    //        }*/

    //        if (tpQue.Count > 0)
    //        {
    //            TaskPack tp;
    //            lock (obj)
    //            {
    //                tp = tpQue.Dequeue();
    //            }

    //            tp.cb(tp.tId);
    //        }
    //    }
    //}

    //private static void BraSend(Player player, ProtocolBase protoBase, Room room)
    //{
    //    //获取数值
    //    int start = 0;
    //    ProtocolBytes protocol = (ProtocolBytes)protoBase;
    //    string protoName = protocol.GetString(start, ref start);

    //    //广播
    //    ProtocolBytes protocolRet = new ProtocolBytes();
    //    protocolRet.AddString("CountDown");
    //    protocolRet.AddString(player.id);
    //    protocolRet.AddInt(room.countNum);
    //    room.Broadcast(protocolRet);
    //    room.countNum -= 1;
    //}


    //public void MsgSend_Poker(Player player, ProtocolBase protoBase)
    //{
    //    //获取数值
    //    int start = 0;
    //    ProtocolBytes protocol = (ProtocolBytes)protoBase;
    //    string protoName = protocol.GetString(start, ref start);
    //    //获取房间
    //    if (player.tempData.status != PlayerTempData.Status.Fight)
    //        return;
    //    Room room = player.tempData.room;

    //    Poker pok = new Poker();
    //    pok.WashPoker();
    //    int[] bli = pok.GetBottomThreePoker();
    //    int[,] li = pok.SendPoker(room.landIndex);

    //    SendPoker(player, protoBase, li, bli);
    //}

    //private void SendPoker(Player player, ProtocolBase protoBase, int[,] list, int[] bli)
    //{
    //    //获取数值
    //    int start = 0;
    //    ProtocolBytes protocol = (ProtocolBytes)protoBase;
    //    string protoName = protocol.GetString(start, ref start);
    //    //获取房间
    //    if (player.tempData.status != PlayerTempData.Status.Fight)
    //        return;
    //    Room room = player.tempData.room;

    //    string teamPoker1 = null;
    //    string teamPoker2 = null;
    //    string teamPoker3 = null;
    //    string bPoker = null;

    //    for (int i = 0; i < 3; i++)
    //    {
    //        for (int j = 0; j < 17; j++)
    //        {
    //            if (i == 0) teamPoker1 += list[i, j] + ";";
    //            else if(i == 1) teamPoker2 += list[i, j] + ";";
    //            else teamPoker3 += list[i, j] + ";";
    //        }

    //        bPoker += bli[i] + ";";
    //    }

    //    //广播
    //    ProtocolBytes protocolRet = new ProtocolBytes();
    //    protocolRet.AddString("Send_Poker");
    //    protocolRet.AddString(player.id);
    //    protocolRet.AddString(teamPoker1);
    //    protocolRet.AddString(teamPoker2);
    //    protocolRet.AddString(teamPoker3);
    //    protocolRet.AddString(bPoker);
    //    protocolRet.AddInt(room.landIndex);

    //    room.Broadcast(protocolRet);
    //    Console.WriteLine("--------------------------" + protoName);
    //}

    //public void MsgReady_Poker(Player player, ProtocolBase protoBase)
    //{
    //    //获取数值
    //    int start = 0;
    //    ProtocolBytes protocol = (ProtocolBytes)protoBase;
    //    string protoName = protocol.GetString(start, ref start);
    //    //获取房间
    //    if (player.tempData.status != PlayerTempData.Status.Fight)
    //        return;
    //    Room room = player.tempData.room;
    //    room.readyNum += 1;

    //    if(room.readyNum >= 3)
    //    {
    //        MsgSend_Poker(player, protoBase);
    //    }
    //}

    //public void MsgLand_Lord(Player player, ProtocolBase protoBase)
    //{
    //    //获取数值
    //    int start = 0;
    //    ProtocolBytes protocol = (ProtocolBytes)protoBase;
    //    string protoName = protocol.GetString(start, ref start);
    //    //0 叫地主 1 不叫 2加倍倍数*1 3加倍倍数*2 4加倍倍数*3 5不加倍
    //    int isLand = protocol.GetInt(start, ref start);

    //    Console.WriteLine("==============================" + protoName + "  isLand   " + isLand);

    //    //获取房间
    //    if (player.tempData.status != PlayerTempData.Status.Fight)
    //        return;
    //    Room room = player.tempData.room;
    //    if (isLand >= 2)
    //    {
    //        room.mulNum += 1;
    //        if(isLand != 5)
    //        {
    //            room.mulDic.Add(player.tempData.team, isLand - 1);
    //            if (room.landDic.ContainsKey(player.tempData.team))
    //            {
    //                if (room.landDic[player.tempData.team])
    //                {
    //                    if (isLand - 1 > room.multiple)
    //                    {
    //                        room.multiple = isLand - 1;
    //                        room.nowLand = player.tempData.team;
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                Console.WriteLine(" --------字典中没有此队伍--------- ");
    //            }
    //        }
    //        else
    //        {
    //            room.mulDic.Add(player.tempData.team, 0);
    //        }
    //        if (room.mulNum >= 3)
    //        {
    //            room.isLandedBool = true;
    //        }
    //    }
    //    else
    //    {
    //        if (isLand == 0)
    //        {
    //            room.cardNum += 5;
    //            room.landNum += 1;
    //            room.landDic.Add(player.tempData.team, true);
    //        }
    //        else
    //        {
    //            room.noLandNum += 1;
    //            room.landDic.Add(player.tempData.team, false);
    //        }

    //        if(room.noLandNum == 3)//三方都没抢
    //        {
    //            room.nowLand = room.landDic.First().Key;
    //            room.isLandedBool = true;
    //        }
    //    }
    //    Console.WriteLine(" ------------room.mulNum---room.isLandedBool----- " + room.mulNum + "  " + room.isLandedBool);
    //    if (!room.isLandedBool)
    //    {
    //        if (room.landIndex == 0) room.landIndex = 1;
    //        else if (room.landIndex == 1) room.landIndex = 2;
    //        else room.landIndex = 0;
    //        //倍数计算
    //        int mul = room.cardNum;
    //        foreach (int kmul in room.mulDic.Values)
    //        {
    //            if (kmul > 0)
    //            {
    //                mul *= kmul;
    //            }
    //        }

    //        //广播
    //        ProtocolBytes protocolRet = new ProtocolBytes();
    //        protocolRet.AddString("Land_Lord");
    //        protocolRet.AddString(player.id);
    //        protocolRet.AddInt(room.landIndex);
    //        protocolRet.AddInt(room.landNum);
    //        protocolRet.AddInt(room.noLandNum);
    //        protocolRet.AddInt(mul);
    //        room.Broadcast(protocolRet);

    //        Console.WriteLine("--------------------------" + protoName);

    //    }
    //    else
    //    { LandOK(player, protoBase, room); }
    //}

    //public void LandOK(Player player, ProtocolBase protoBase,Room room)
    //{
    //    //获取房间
    //    if (player.tempData.status != PlayerTempData.Status.Fight)
    //        return;

    //    //此局最终倍数
    //    int mul = room.cardNum;
    //    foreach (int kmul in room.mulDic.Values)
    //    {
    //        if(kmul > 0)
    //        {
    //            mul *= kmul;
    //        }
    //    }

    //    for (int i = 1; i <= 3; i++)
    //    {
    //        if(i == room.nowLand)
    //        {
    //            room.pokerNumDic.Add(i, 20);
    //        }else room.pokerNumDic.Add(i, 17);
    //    }
    //    //下一句的首发者
    //    room.landIndex = room.nowLand;

    //    //广播
    //    ProtocolBytes protocolRet = new ProtocolBytes();
    //    protocolRet.AddString("LandOK");
    //    protocolRet.AddString(player.id);
    //    protocolRet.AddInt(room.nowLand);
    //    protocolRet.AddString(room.play_Cards);
    //    protocolRet.AddInt(mul);
    //    room.Broadcast(protocolRet);

    //    room.PlayCardPlayer = room.nowLand;
    //    room.maxCardTeam = room.nowLand;
    //    Console.WriteLine("--------------------------LandOK");
    //}

    //public void MsgPlay_Card(Player player, ProtocolBase protoBase)
    //{
    //    //获取数值
    //    int start = 0;
    //    ProtocolBytes protocol = (ProtocolBytes)protoBase;
    //    string protoName = protocol.GetString(start, ref start);
    //    string nowPlayCard = protocol.GetString(start, ref start);
    //    string PlayEdCard = protocol.GetString(start, ref start);
    //    int pokerNum = protocol.GetInt(start, ref start);

    //    //获取房间
    //    if (player.tempData.status != PlayerTempData.Status.Fight)
    //        return;
    //    Room room = player.tempData.room;
    //    if(nowPlayCard != "0")
    //    {
    //        room.play_Cards = nowPlayCard;
    //        room.maxCardTeam = player.tempData.team;
    //    }

    //    if (room.PlayCardPlayer == 1) room.PlayCardPlayer = 2;
    //    else if (room.PlayCardPlayer == 2) room.PlayCardPlayer = 3;
    //    else room.PlayCardPlayer = 1;

    //    Random rd = new Random();
    //    int rot = rd.Next(0, 360);
    //    int locX = rd.Next(-60, 60);
    //    int locY = rd.Next(-40, 40);

    //    if (room.pokerNumDic.ContainsKey(player.tempData.team))
    //    {
    //        room.pokerNumDic[player.tempData.team] -= pokerNum;
    //    }
    //    string pokerStr = null;
    //    foreach (KeyValuePair<int,int> item in room.pokerNumDic)
    //    {
    //        pokerStr += item.Key + ";" + item.Value + ";";
    //    }

    //    //广播
    //    ProtocolBytes protocolRet = new ProtocolBytes();
    //    protocolRet.AddString("Play_Card");
    //    protocolRet.AddString(player.id);
    //    protocolRet.AddInt(room.PlayCardPlayer);
    //    protocolRet.AddString(nowPlayCard);
    //    protocolRet.AddInt(room.maxCardTeam);
    //    protocolRet.AddString(PlayEdCard);
    //    protocolRet.AddInt(rot);
    //    protocolRet.AddInt(locX);
    //    protocolRet.AddInt(locY);
    //    protocolRet.AddString(pokerStr);
    //    room.Broadcast(protocolRet);

    //    Console.WriteLine("--------------------------" + protoName);
    //}

    //public void MsgCd(Player player, ProtocolBase protoBase)
    //{
    //    //获取数值
    //    int start = 0;
    //    ProtocolBytes protocol = (ProtocolBytes)protoBase;
    //    string protoName = protocol.GetString(start, ref start);
    //    int land = protocol.GetInt(start, ref start);

    //    //获取房间
    //    if (player.tempData.status != PlayerTempData.Status.Fight)
    //        return;
    //    Room room = player.tempData.room;

    //    ProtocolBytes protocolRet = new ProtocolBytes();
    //    protocolRet.AddString("Cd");
    //    protocolRet.AddString(player.id);
    //    protocolRet.AddInt(land);

    //    room.Broadcast(protocolRet);
    //}

    //public void MsgWin(Player player, ProtocolBase protoBase)
    //{
    //    //获取数值
    //    int start = 0;
    //    ProtocolBytes protocol = (ProtocolBytes)protoBase;
    //    string protoName = protocol.GetString(start, ref start);
    //    int win = protocol.GetInt(start, ref start);

    //    //获取房间
    //    if (player.tempData.status != PlayerTempData.Status.Fight)
    //        return;
    //    Room room = player.tempData.room;

    //    ProtocolBytes protocolRet = new ProtocolBytes();
    //    protocolRet.AddString("Win");
    //    protocolRet.AddString(player.id);
    //    protocolRet.AddInt(win);

    //    room.Broadcast(protocolRet);
    //}

    ////清理房间数据
    //public void MsgClearSer(Player player, ProtocolBase protoBase)
    //{
    //    //获取数值
    //    int start = 0;
    //    ProtocolBytes protocol = (ProtocolBytes)protoBase;
    //    string protoName = protocol.GetString(start, ref start);


    //    //获取房间
    //    if (player.tempData.status != PlayerTempData.Status.Fight)
    //        return;

    //    Room room = player.tempData.room;
    //    room.countNum = 15;
    //    room.readyNum = 0;
    //    room.landIndex = 0;
    //    room.multiple = 0;
    //    room.cardNum = 25;
    //    room.landNum = 0;
    //    room.noLandNum = 0;
    //    room.nowLand = 0;
    //    room.isLandedBool = false;
    //    room.mulNum = 0;
    //    room.PlayCardPlayer = -1;
    //    room.maxCardTeam = -1;
    //    room.landDic.Clear();
    //    room.mulDic.Clear();
    //    room.pokerNumDic.Clear();
    //    room.play_Cards = "0";

    //    Console.WriteLine("--------------服务器清理完成---------------------");
    //}

    ////通知客户端清理UI
    //public void MsgClearUI(Player player, ProtocolBase protoBase)
    //{
    //    //获取数值
    //    int start = 0;
    //    ProtocolBytes protocol = (ProtocolBytes)protoBase;
    //    string protoName = protocol.GetString(start, ref start);
    //    int team = protocol.GetInt(start, ref start);

    //    //获取房间
    //    if (player.tempData.status != PlayerTempData.Status.Fight)
    //        return;
    //    Room room = player.tempData.room;
    //    ProtocolBytes protocolRet = new ProtocolBytes();
    //    protocolRet.AddString("ClearUI");
    //    protocolRet.AddString(player.id);
    //    protocolRet.AddInt(team);
    //    room.Broadcast(protocolRet);
    //}
}
