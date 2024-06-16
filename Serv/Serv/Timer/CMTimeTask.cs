// ========================================================
// 描述：定时任务数据类
// 作者：城门
// 邮箱：136569631@qq.com
// 创建时间：2019/05/28 09:26:53
// 版本：v 1.0
// ========================================================

using System;

public class CMTimeTask
{
    //定时任务ID
    public int tId;
    //返回函数
    public Action<int> callBack;
    //单位：毫秒
    public double destTime;
    //延迟
    public double delay;
    //执行次数
    public int count;

    public CMTimeTask(int tId, Action<int> callBack, double destTime, double delay, int count)
    {
        this.tId = tId;
        this.callBack = callBack;
        this.destTime = destTime;
        this.delay = delay;
        this.count = count;
    }
}

public class CMFrameTask
{
    //定时任务ID
    public int tId;
    //返回函数
    public Action<int> callBack;
    //单位：帧
    public int  destFrame;
    //延迟
    public int delay;
    //执行次数
    public int count;

    public CMFrameTask(int tId, Action<int> callBack, int destFrame, int delay, int count)
    {
        this.tId = tId;
        this.callBack = callBack;
        this.destFrame = destFrame;
        this.delay = delay;
        this.count = count;
    }
}

public enum CMTimeUnit
{
    MilliSecond,
    Second,
    Minute,
    Hour,
    Day
}

