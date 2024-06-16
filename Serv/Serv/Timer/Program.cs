
using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;

namespace ConsoleTimer
{
    class Program
    {
        private static readonly string obj = "lock";
        /*
        static void Main(string[] args)
        {
            Console.WriteLine("Console Start");
            //Test1();
            Test2();
            //TimerTest();
            Console.ReadKey();
        }*/

        //在主线程检测并处理
        static void Test1()
        {
            CMTimer ct = new CMTimer();
            ct.SetLog((string info) =>
            {
                Console.WriteLine("ConsoleLog：" + info);
            });

            ct.AddTimeTask((int tid) =>
            {
                Console.WriteLine("Time：" + DateTime.Now);
                Console.WriteLine("Process线程ID：{0}", Thread.CurrentThread.ManagedThreadId.ToString());
            }, 1000, CMTimeUnit.MilliSecond, 0);

            while (true)
            {
                ct.Update();
            }
        }

        //独立线程检测并处理
        static void Test2()
        {
            Queue<TaskPack> tpQue = new Queue<TaskPack>();
            CMTimer ct = new CMTimer(50);
            ct.SetLog((string info) =>
            {
                Console.WriteLine("ConsoleLog：" + info);
            });

            int id = ct.AddTimeTask((int tid) =>{
                Console.WriteLine("Process线程ID：{0}", Thread.CurrentThread.ManagedThreadId.ToString());
            }, 1000, CMTimeUnit.MilliSecond, 0);

            //运行线程回归到主线程（一个线程）
            //ct.SetHandle((Action<int> cb, int tid) =>{
            //    if(cb != null)
            //    {
            //        lock (obj)
            //        {
            //            tpQue.Enqueue(new TaskPack(tid, cb));
            //        }
            //    }
            //});

            while (true)
            {
                string ipt = Console.ReadLine();
                if(ipt == "d")
                {
                    ct.DelTimeTask(id);
                }

                if(tpQue.Count > 0)
                {
                    TaskPack tp;
                    lock (obj)
                    {
                        tp = tpQue.Dequeue();
                    }
                    
                    tp.cb(tp.tId);
                }
            }
        }

        static void TimerTest()
        {
            System.Timers.Timer t = new System.Timers.Timer(1000);
            t.AutoReset = true;
            t.Elapsed += (object sender, ElapsedEventArgs args) => {
                Console.WriteLine("Time：" + DateTime.Now);
                Console.WriteLine("Process线程ID：{0}", Thread.CurrentThread.ManagedThreadId.ToString());
            };
            t.Start();
        }
    }
}

class TaskPack {
    public int tId;
    public Action<int> cb;

    public TaskPack(int tId,Action<int> cb) {
        this.tId = tId;
        this.cb = cb;
    }
}
