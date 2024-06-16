using System;
using System.Collections.Generic;
using System.Linq; 

//房间
public class Room
{
	//状态
	public enum Status
	{
		Prepare = 1,
		Fight = 2 ,
	}
	public Status status = Status.Prepare;
	//玩家
	public int maxPlayers = 3;
	public Dictionary<string,Player> list = new Dictionary<string,Player>();
    //倒计时计时
    public int countNum = 15;
    //准备人数
    public int readyNum = 0;
    //轮到谁叫地主
    public int landIndex = 0;
    //此轮加倍倍数
    public int multiple = 0;
    //此轮基数
    public int cardNum = 25;
    //此轮叫地主人数
    public int landNum = 0;
    //此轮不叫地主人数
    public int noLandNum = 0;
    //目前地主在谁手上
    public int nowLand = 0;
    //抢地主状态
    public bool isLandedBool = false;
    //此轮翻倍人数
    public int mulNum = 0;

    //轮到谁出牌
    public int PlayCardPlayer = -1;
    //最大牌出自谁手
    public int maxCardTeam = -1;
    //抢过地主的记录
    public Dictionary<int, bool> landDic = new Dictionary<int, bool>();
    //总共的翻倍倍数
    public Dictionary<int, int> mulDic = new Dictionary<int, int>();
    //剩余牌数
    public Dictionary<int, int> pokerNumDic = new Dictionary<int, int>();

    //play_Cards = "05X06X06"
    //记录上家出的牌 前2位记录出牌类别 0单张 1对子 2三张 3三带一 4三带二 5单顺子 6双顺子 7三顺子 8飞机带翅膀 9四带二 10炸弹 11火箭（双王）05X(单顺子)
    //中间2位记录的是 主要牌面 06X（即从6开始的单顺子 6 7 8 9 10...）
    //后两位 出牌的个数 06(最后得出 6 7 8 9 10 J)
    public string play_Cards = "0";

    //添加玩家
    public bool AddPlayer(Player player)
	{
		lock (list) 
		{
			if (list.Count >= maxPlayers)
				return false;
			PlayerTempData tempData = player.tempData;
			tempData.room = this; 
			tempData.team = SwichTeam ();
			tempData.status = PlayerTempData.Status.Room;
            if (list.Count == 0)
				tempData.isOwner = true;
			string id = player.id;
			list.Add(id, player);
		}
		return true;
	}
	
	//分配队伍
	public int SwichTeam()
	{
		int count1 = 0;
		int count2 = 0;
        int count3 = 0;
        foreach (Player player in list.Values)
		{
			if(player.tempData.team == 1) count1++;
			if(player.tempData.team == 2) count2++;
            if(player.tempData.team == 3) count3++;
        }
        if (count1 == 0)
            return 1;
        else if (count2 == 0)
            return 2;
        else if (count3 == 0)
            return 3;
        else return 0;
    }

	//删除玩家
	public void DelPlayer(string id)
	{
		lock (list) 
		{
			if (!list.ContainsKey(id))
				return;
			bool isOwner = list[id].tempData.isOwner;
			list[id].tempData.status = PlayerTempData.Status.None;
			list.Remove(id);
			if(isOwner)
				UpdateOwner();
		}
	}

	//更换房主
	public void UpdateOwner()
	{
		lock (list) 
		{
			if(list.Count <= 0)
				return;
			
			foreach(Player player in list.Values)
			{
				player.tempData.isOwner = false;
			}
			
			Player p = list.Values.First();
			p.tempData.isOwner = true;
		}
	}

	//广播
	public void Broadcast(ProtocolBase protocol)
	{
		foreach(Player player in list.Values)
		{
			player.Send(protocol);
		}
	}

	//房间信息
	public ProtocolBytes GetRoomInfo()
	{
		ProtocolBytes protocol = new ProtocolBytes ();
		protocol.AddString ("GetRoomInfo");
		//房间信息
		protocol.AddInt (list.Count);
		//每个玩家信息
		foreach(Player p in list.Values)
		{
			protocol.AddString(p.id);
			protocol.AddInt(p.tempData.team);
			protocol.AddInt(p.data.win);
			protocol.AddInt(p.data.fail);
			int isOwner = p.tempData.isOwner? 1: 0;
			protocol.AddInt(isOwner);
		}
		return protocol;
	}

	//房间能否开战
	public bool CanStart()
	{
        /*
		if (status != Status.Prepare)
			return false;*/
		
		int count1 = 0;
		int count2 = 0;
        int count3 = 0;

        foreach (Player player in list.Values)
		{
			if(player.tempData.team == 1) count1++;
			if(player.tempData.team == 2) count2++;
            if(player.tempData.team == 3) count3++;
        }
		
        /*
		if (count1 < 1 || count2 < 1 || count3 < 1)
			return false;*/
		
		return true;
	}


	public void StartFight()
	{
		ProtocolBytes protocol = new ProtocolBytes ();
		protocol.AddString ("Fight");
		status = Status.Fight;
		int teamPos1 = 1;
		int teamPos2 = 1;
        int teamPos3 = 1;
        lock (list) 
		{
			protocol.AddInt(list.Count);
			foreach(Player p in list.Values)
			{
				p.tempData.hp = 200;
				protocol.AddString(p.id);
				protocol.AddInt(p.tempData.team);
				if(p.tempData.team == 1)
					protocol.AddInt(teamPos1++);
				else if(p.tempData.team == 2)
                    protocol.AddInt(teamPos2++);
                else if (p.tempData.team == 3)
                    protocol.AddInt(teamPos3++);
                else
                {
                    Console.WriteLine("-----");
                }

                p.tempData.status = PlayerTempData.Status.Fight;
            }
			Broadcast(protocol);
		}
	}

    //胜负判断
    private int IsWin()
	{
		if (status != Status.Fight)
			return 0;
		
		int count1 = 0;
		int count2 = 0;
        int count3 = 0;
        foreach (Player player in list.Values)
		{
			PlayerTempData pt = player.tempData;
			if(pt.team == 1 && pt.hp > 0) count1++;
			if(pt.team == 2 && pt.hp > 0) count2++;
            if(pt.team == 3 && pt.hp > 0) count3++;
        }
		
		if(count1 > 0 && count2 <= 0 && count3 <= 0) return 1;
        if(count2 > 0 && count1 <= 0 && count3 <= 0) return 2;
        if(count3 > 0 && count1 <= 0 && count2 <= 0) return 3;
        return 0;
	}


	public void UpdateWin()
	{
		int isWin = IsWin();
		if (isWin == 0)
			return;
		//改变状态 数值处理
		lock (list) 
		{
			status = Status.Prepare;
			foreach (Player player in list.Values) 
			{
				player.tempData.status = PlayerTempData.Status.Room;
				if (player.tempData.team == isWin)
					player.data.win++;
				else
					player.data.fail++;
			}
		}
		//广播
		ProtocolBytes protocol = new ProtocolBytes();
		protocol.AddString ("Result");
		protocol.AddInt (isWin);
		Broadcast (protocol);
	}


	//中途退出战斗
	public void ExitFight(Player player)
	{
		//摧毁坦克
		if (list [player.id] != null)
			list [player.id].tempData.hp = -1;
		//广播消息
		ProtocolBytes protocolRet = new ProtocolBytes();
		protocolRet.AddString ("Hit");
		protocolRet.AddString (player.id);
		protocolRet.AddString (player.id);
		protocolRet.AddFloat (999);
		Broadcast (protocolRet);
		//增加失败次数
		if (IsWin () == 0)
			player.data.fail++;
		//胜负判断
		UpdateWin();
	}
}