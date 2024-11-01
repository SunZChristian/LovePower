// ========================================================
// 描述：
// 作者：城门
// 邮箱：136569631@qq.com
// 创建时间：2019/05/28 15:22:08
// 版本：v 1.0
// ========================================================
/* ==============================================================================
 * 功能描述：CMTimer 计时器 
 * 创 建 者：Administrator
 * 邮箱：136569631@qq.com
 * 版本：v 1.0
 * 创建日期：2019/5/28 14:29:18
 * ==============================================================================*/


using System;
using System.Collections.Generic;
using System.Timers;

public class CMTimer
{
    private DateTime startDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
    private double nowTime;
    private Timer svrTimer;

    private Action<string> taskLog;
    private Action<Action<int>, int> taskHandle;
    private static readonly string lockTid = "lockTid";
    /***********************时间定时**********************/
    private int tId;
    private List<int> tIdList = new List<int>();
    private List<int> rectTidList = new List<int>();

    private static readonly string lockTime = "lockTime";
    private List<CMTimeTask> tempTimeList = new List<CMTimeTask>();
    private List<CMTimeTask> taskTimeList = new List<CMTimeTask>();
    private List<int> tempDelTimeList = new List<int>();
    /***********************时间定时**********************/

    /***********************帧定时**********************/
    private int frameCounter;
    private static readonly string lockFrame = "lockFrame";
    private List<CMFrameTask> tempFrameList = new List<CMFrameTask>();
    private List<CMFrameTask> taskFrameList = new List<CMFrameTask>();
    private List<int> tempDelFrameList = new List<int>();
    /***********************帧定时**********************/

    public CMTimer(int interval = 0){
        tIdList.Clear();
        rectTidList.Clear();

        tempTimeList.Clear();
        taskTimeList.Clear();

        tempFrameList.Clear();
        taskFrameList.Clear();

        if(interval != 0)
        {
            svrTimer = new Timer(interval)
            {
                AutoReset = true
            };
            svrTimer.Elapsed += (object sender, ElapsedEventArgs args) => {
                Update();
            };
            svrTimer.Start();
        }
    }

    public void Update()
    {
        CheckTimeTask();
        CheckFrameTask();
        DeleteTimeTask();
        DeleteFrameTask();

        if (rectTidList.Count > 0)
        {
            lock (lockTid)
            {
                RecycleTid();
            }
        }
    }

    private void DeleteTimeTask()
    {
        if(tempDelTimeList.Count > 0)
        {
            lock (lockTime)
            {
                for (int i = 0; i < tempDelTimeList.Count; i++)
                {
                    bool isDel = false;
                    int delTid = tempDelTimeList[i];
                    for (int j = 0; j < taskTimeList.Count; j++)
                    {
                        CMTimeTask task = taskTimeList[j];
                        if(task.tId == delTid)
                        {
                            taskTimeList.RemoveAt(j);
                            rectTidList.Add(delTid);
                            isDel = true;
                            LogInfo("Del taskTimeList ID：" + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                            break;
                        }
                    }
                    if (isDel)
                    {
                        continue;
                    }
                    for (int j = 0; j < tempTimeList.Count; j++)
                    {
                        CMTimeTask task = tempTimeList[j];
                        if(task.tId == delTid)
                        {
                            tempTimeList.RemoveAt(j);
                            rectTidList.Add(delTid);
                            LogInfo("Del tempTimeList ID：" + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                            break;
                        }
                    }
                }

                tempDelTimeList.Clear();
            }
        }
    }
    private void DeleteFrameTask()
    {
        if (tempDelFrameList.Count > 0)
        {
            lock (lockFrame)
            {
                for (int i = 0; i < tempDelFrameList.Count; i++)
                {
                    bool isDel = false;
                    int delTid = tempDelFrameList[i];
                    for (int j = 0; j < taskFrameList.Count; j++)
                    {
                        CMFrameTask task = taskFrameList[j];
                        if (task.tId == delTid)
                        {
                            taskFrameList.RemoveAt(j);
                            rectTidList.Add(delTid);
                            isDel = true;
                            LogInfo("Del taskFrameList ID：" + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                            break;
                        }
                    }
                    if (isDel)
                    {
                        continue;
                    }
                    for (int j = 0; j < tempFrameList.Count; j++)
                    {
                        CMFrameTask task = tempFrameList[j];
                        if (task.tId == delTid)
                        {
                            tempFrameList.RemoveAt(j);
                            rectTidList.Add(delTid);
                            LogInfo("Del tempFrameList ID：" + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
                            break;
                        }
                    }
                }

                tempDelFrameList.Clear();
            }
        }
    }
    private void CheckTimeTask()
    {
        if(tempTimeList.Count > 0)
        {
            lock (lockTime)
            {
                //加入缓存区的定时任务
                for (int tempIndex = 0; tempIndex < tempTimeList.Count; tempIndex++)
                {
                    taskTimeList.Add(tempTimeList[tempIndex]);
                }
                tempTimeList.Clear();
            }
        }
        
        //遍历检测条件是否达到条件
        nowTime = GetUTCMilliSeconds();
        for (int i = 0; i < taskTimeList.Count; i++)
        {
            CMTimeTask task = taskTimeList[i];
            //nowTime 和 task.destTime 作比较
            if (nowTime.CompareTo(task.destTime) < 0)
            {
                continue;
            }
            else
            {
                Action<int> cb = task.callBack;
                try
                {
                    if(taskHandle != null)
                    {
                        taskHandle(cb, task.tId);
                    }
                    else
                    {
                        if (cb != null)
                        {
                            cb(task.tId);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogInfo(e.ToString());
                }

                //移除已经完成的任务
                if (task.count == 1)
                {
                    taskTimeList.RemoveAt(i);
                    i--;
                    rectTidList.Add(task.tId);
                }
                else
                {
                    if (task.count != 0)
                    {
                        task.count -= 1;
                    }
                    task.destTime += task.delay;
                }

            }
        }
    }
    private void CheckFrameTask()
    {
        if(tempFrameList.Count > 0)
        {
            lock (lockFrame)
            {
                //加入缓存区的定时任务
                for (int tempIndex = 0; tempIndex < tempFrameList.Count; tempIndex++)
                {
                    taskFrameList.Add(tempFrameList[tempIndex]);
                }
                tempFrameList.Clear();
            }
        }

        frameCounter += 1;
        //遍历检测条件是否达到条件
        for (int i = 0; i < taskFrameList.Count; i++)
        {
            CMFrameTask task = taskFrameList[i];
            if (frameCounter < task.destFrame)
            {
                continue;
            }
            else
            {
                Action<int> cb = task.callBack;
                try
                {
                    if (taskHandle != null)
                    {
                        taskHandle(cb, task.tId);
                    }
                    else
                    {
                        if (cb != null)
                        {
                            cb(task.tId);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogInfo(e.ToString());
                }

                //移除已经完成的任务
                if (task.count == 1)
                {
                    taskFrameList.RemoveAt(i);
                    i--;
                    rectTidList.Add(task.tId);
                }
                else
                {
                    if (task.count != 0)
                    {
                        task.count -= 1;
                    }
                    task.destFrame += task.delay;
                }

            }
        }
    }

    #region 时间定时任务
    public int AddTimeTask(Action<int> callBack, double delay, CMTimeUnit timeUnit = CMTimeUnit.MilliSecond, int count = 1)
    {
        LogInfo("Add Time Task.");

        if (timeUnit != CMTimeUnit.MilliSecond)
        {
            switch (timeUnit)
            {
                case CMTimeUnit.Second:
                    delay = delay * 1000;
                    break;
                case CMTimeUnit.Minute:
                    delay = delay * 1000 * 60;
                    break;
                case CMTimeUnit.Hour:
                    delay = delay * 1000 * 60 * 60;
                    break;
                case CMTimeUnit.Day:
                    delay = delay * 1000 * 60 * 60 * 24;
                    break;
                default:
                    LogInfo("Add Task TimeUnit Type Error...");
                    break;
            }
        }

        nowTime = GetUTCMilliSeconds();
        int tid = GetTid();
        lock (lockTime)
        {
            tempTimeList.Add(new CMTimeTask(tid, callBack, nowTime + delay, delay, count));
        }
        return tid;
    }
    public void DelTimeTask(int tId)
    {
        lock(lockTime)
        {
            tempDelTimeList.Add(tId);
            LogInfo("TempDel ID：" + System.Threading.Thread.CurrentThread.ManagedThreadId.ToString());
        }

        /*
        bool exist = false;
        for (int i = 0; i < taskTimeList.Count; i++)
        {
            CMTimeTask task = taskTimeList[i];
            if (task.tId == tId)
            {
                taskTimeList.RemoveAt(i);
                for (int j = 0; j < tIdList.Count; j++)
                {
                    if (tIdList[j] == tId)
                    {
                        tIdList.RemoveAt(j);
                        break;
                    }
                }
                exist = true;
                break;
            }
        }

        if (!exist)
        {
            for (int i = 0; i < tempTimeList.Count; i++)
            {
                CMTimeTask task = tempTimeList[i];
                if (task.tId == tId)
                {
                    tempTimeList.RemoveAt(i);
                    for (int j = 0; j < tIdList.Count; j++)
                    {
                        if (tIdList[j] == tId)
                        {
                            tIdList.RemoveAt(j);
                            break;
                        }
                    }
                    exist = true;
                    break;
                }
            }
        }

        return exist;
        */
    }
    public bool ReplaceTimeTask(int tId, Action<int> callBack, double delay, CMTimeUnit timeUnit = CMTimeUnit.MilliSecond, int count = 1)
    {
        if (timeUnit != CMTimeUnit.MilliSecond)
        {
            switch (timeUnit)
            {
                case CMTimeUnit.Second:
                    delay = delay * 1000;
                    break;
                case CMTimeUnit.Minute:
                    delay = delay * 1000 * 60;
                    break;
                case CMTimeUnit.Hour:
                    delay = delay * 1000 * 60 * 60;
                    break;
                case CMTimeUnit.Day:
                    delay = delay * 1000 * 60 * 60 * 24;
                    break;
                default:
                    LogInfo("Add Task TimeUnit Type Error...");
                    break;
            }
        }

        nowTime = GetUTCMilliSeconds();
        CMTimeTask newTask = new CMTimeTask(tId, callBack, nowTime + delay, delay, count);

        bool isRep = false;
        for (int i = 0; i < taskTimeList.Count; i++)
        {
            if (taskTimeList[i].tId == tId)
            {
                taskTimeList[i] = newTask;
                isRep = true;
                break;
            }
        }

        if (!isRep)
        {
            for (int i = 0; i < tempTimeList.Count; i++)
            {
                if (tempTimeList[i].tId == tId)
                {
                    tempTimeList[i] = newTask;
                    isRep = true;
                    break;
                }
            }
        }

        return isRep;
    }
    #endregion

    #region 帧定时任务
    public int AddFrameTask(Action<int> callBack, int delay, int count = 1)
    {
        int tid = GetTid();
        lock (lockFrame)
        {
            tempFrameList.Add(new CMFrameTask(tid, callBack, frameCounter + delay, delay, count));
        }
        return tid;
    }
    public void DelFrameTask(int tId)
    {
        lock (lockFrame)
        {
            tempDelFrameList.Add(tId);
        }
        
        /*
        bool exist = false;
        for (int i = 0; i < taskFrameList.Count; i++)
        {
            CMFrameTask task = taskFrameList[i];
            if (task.tId == tId)
            {
                taskFrameList.RemoveAt(i);
                for (int j = 0; j < tIdList.Count; j++)
                {
                    if (tIdList[j] == tId)
                    {
                        tIdList.RemoveAt(j);
                        break;
                    }
                }
                exist = true;
                break;
            }
        }

        if (!exist)
        {
            for (int i = 0; i < tempFrameList.Count; i++)
            {
                CMFrameTask task = tempFrameList[i];
                if (task.tId == tId)
                {
                    tempFrameList.RemoveAt(i);
                    for (int j = 0; j < tIdList.Count; j++)
                    {
                        if (tIdList[j] == tId)
                        {
                            tIdList.RemoveAt(j);
                            break;
                        }
                    }
                    exist = true;
                    break;
                }
            }
        }

        return exist;
        */
    }
    public bool ReplaceFrameTask(int tId, Action<int> callBack, int delay, int count = 1)
    {
        CMFrameTask newTask = new CMFrameTask(tId, callBack, frameCounter + delay, delay, count);

        bool isRep = false;
        for (int i = 0; i < taskFrameList.Count; i++)
        {
            if (taskFrameList[i].tId == tId)
            {
                taskFrameList[i] = newTask;
                isRep = true;
                break;
            }
        }

        if (!isRep)
        {
            for (int i = 0; i < tempFrameList.Count; i++)
            {
                if (tempFrameList[i].tId == tId)
                {
                    tempFrameList[i] = newTask;
                    isRep = true;
                    break;
                }
            }
        }

        return isRep;
    }
    #endregion

    #region 公共API函数
    public void SetHandle(Action<Action<int>,int> handle)
    {
        taskHandle = handle;
    }
    public void Reset()
    {
        tId = 0;
        tIdList.Clear();
        rectTidList.Clear();

        tempTimeList.Clear();
        taskTimeList.Clear();

        tempFrameList.Clear();
        taskFrameList.Clear();

        taskLog = null;
        svrTimer.Stop();
    }
    public void SetLog(Action<string> log)
    {
        taskLog = log;
    }
    public int GetYear()
    {
        return GetLocalDateTime().Year;
    }
    public int GetMonth()
    {
        return GetLocalDateTime().Month;
    }
    public int GetDay()
    {
        return GetLocalDateTime().Day;
    }
    public int GetWeek()
    {
        return (int)GetLocalDateTime().DayOfWeek;
    }
    public DateTime GetLocalDateTime()
    {
        DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(startDateTime.AddMilliseconds(nowTime));
        return dt;
    }
    public double GetMilliSecondsTime()
    {
        return nowTime;
    }
    public string GetLocalTimeStr()
    {
        DateTime dt = GetLocalDateTime();
        string str = GetTimeStr(dt.Hour) + ":" + GetTimeStr(dt.Minute) + ":" + GetTimeStr(dt.Second);
        return str;
    }
    #endregion

    #region 工具函数
    private void LogInfo(string info)
    {
        if (taskLog != null)
        {
            taskLog(info);
        }
    }
    private int GetTid()
    {
        lock (lockTid)
        {
            tId += 1;

            //安全代码，以防万一
            while (true)
            {
                if (tId == int.MaxValue)
                {
                    tId = 0;
                }
                bool used = false;
                for (int i = 0; i < tIdList.Count; i++)
                {
                    if (tId == tIdList[i])
                    {
                        used = true;
                        break;
                    }
                }
                if (!used)
                {
                    tIdList.Add(tId);
                    break;
                }
                else
                {
                    tId += 1;
                } 
            }

        }
        return tId;
    }
    private void RecycleTid()
    {
        for (int i = 0; i < rectTidList.Count; i++)
        {
            int tid = rectTidList[i];
            for (int j = 0; j < tIdList.Count; j++)
            {
                if(tid == tIdList[j])
                {
                    tIdList.RemoveAt(j);
                    break;
                }
            }
        }

        rectTidList.Clear();
    }
    private double GetUTCMilliSeconds()
    {
        TimeSpan ts = DateTime.UtcNow - startDateTime;
        return ts.TotalMilliseconds;
    }
    private string GetTimeStr(int time)
    {
        if (time < 10)
        {
            return "0" + time;
        }
        else return time.ToString();
    }
    #endregion

}

