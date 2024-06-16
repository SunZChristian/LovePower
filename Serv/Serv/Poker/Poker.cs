/* ==============================================================================
 * 功能描述：Poker  
 * 创 建 者：Administrator
 * 邮箱：136569631@qq.com
 * 版本：v 1.0
 * 创建日期：2019/6/5 10:05:55
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Poker
{
    //单例
    public static Poker instance;
    //初始牌顺序
    private List<int> startList = new List<int>();
    //洗牌后顺序
    private List<int> endList = new List<int>();
    //发牌后玩家手中牌列表
    public int[,] playerPokerList = new int[3, 17];
    //底牌列表
    public int[] bottomPokerList = new int[3];

    public Poker()
    {
        instance = this;
        for (int i = 0; i < 54; i++)
        {
            startList.Add(i);
        }
    }

    //洗牌
    public void WashPoker()
    {
        endList = startList;
        for (int i = 53; i >= 0; i--)
        {
            Random rd = new Random();
            int ran = rd.Next(1, i+1);
            int temp = endList[ran];
            endList[ran] = endList[i];
            endList[i] = temp;
        }
    }

    //切牌
    public void CutPoker()
    {
        Random rd = new Random();
        int ran = rd.Next(0, 54);
        //print("切牌位置：" + ran);
        List<int> temp = new List<int>();

        //切牌位置后面前调
        for (int i = ran; i < 54; i++)
        {
            temp.Add(endList[i]);
        }

        //切牌位置前面后调
        for (int i = 0; i < ran; i++)
        {
            temp.Add(endList[i]);
        }

        endList.Clear();
        endList = temp;
    }

    //获取后三张底牌
    public int[] GetBottomThreePoker()
    {
        bottomPokerList = new int[3];
        //洗牌后，后三张留底
        for (int i = 51; i < 54; i++)
        {
            bottomPokerList[53-i] = endList[i];
        }
        return bottomPokerList;
    }

    //发牌
    public int[,] SendPoker(int landIndex)
    {
        playerPokerList = new int[3, 17];
        
        int tempSend = landIndex;
        int index = 0;

        for (int i = 0; i < 51; i++)
        {
            switch (tempSend)
            {
                case 0:
                    tempSend = 1;
                    break;
                case 1:
                    tempSend = 2;
                    break;
                case 2:
                    tempSend = 0;
                    break;
                default:
                    Console.WriteLine("发牌过程出错！");
                    break;
            }
            index = i / 3;
            playerPokerList[tempSend, index] = endList[i];
        }

        return playerPokerList;
    }
}
