using System;
using System.Collections.Generic;

/// <summary>
/// 事件系统，独立于unity
/// </summary>
public class EventSystem
{
    /// <summary>
    /// 事件分发者
    /// </summary>
    public class Dispatcher
    {
        /// <summary>
        /// 事件类
        /// </summary>
        class Listen
        {
            public int mID; //监听的编号
            public Func<object, object, object, bool> mAction; //监听的事件
        }

        /// <summary>
        /// 分发类
        /// </summary>
        class Dispatched
        {
            public string mEventName;
            public object mArg1, mArg2, mArg3;

            /// <summary>
            /// 构造函数，初始化
            /// </summary>
            /// <param name="eventName"></param>
            /// <param name="arg1"></param>
            /// <param name="arg2"></param>
            /// <param name="arg3"></param>
            /// <returns></returns>
            public Dispatched Set(string eventName, object arg1 = null, object arg2 = null, object arg3 = null)
            {
                this.mEventName = eventName;
                this.mArg1 = arg1;
                this.mArg2 = arg2;
                this.mArg3 = arg3;
                return this;
            }
        }


        //名字和监听的事件列表
        Dictionary<string,List<int>> mRegisteredEvents = new Dictionary<string, List<int>>();

        //事件编号对应的事件
        Dictionary<int,Listen> mRegistered = new Dictionary<int, Listen>();

        //栈 方便存储空闲的事件
        Stack<Listen> mFreeListens = new Stack<Listen>();  
        
        //存储空闲的分发者
        Stack<Dispatched> mFreeDipatcheds = new Stack<Dispatched>();

        //正在使用的事件分发者
        Queue<Dispatched> mDispatcheds = new Queue<Dispatched>();

        int mNextListenID = 4711; //随便的数字
        /// <summary>
        /// 触发器
        /// </summary>
        /// <param name="eventName"></param>
        public void Trigger(string eventName)
        {
            Call(eventName, null, null, null);
        }

        public void Trigger(string eventName,object param)
        {
            Call(eventName, param, null, null);
        }

        public void Trigger(string eventName, object param1,object param2)
        {
            Call(eventName, param1, param2, null);
        }

        public void Trigger(string eventName, object param1,object param2,object param3)
        {
            Call(eventName, param1, param2, param3);
        }

        /// <summary>
        /// 事件的分发
        /// </summary>
        /// <param name="eventName"></param>
        public void Dispatch(string eventName)
        {
            Dispatched d = (mFreeDipatcheds.Count == 0) ? new Dispatched() : mFreeDipatcheds.Pop();
            mDispatcheds.Enqueue(d.Set(eventName));
        }

        public void Dispatch(string eventName,object param)
        {
            Dispatched d = (mFreeDipatcheds.Count == 0) ? new Dispatched() : mFreeDipatcheds.Pop();
            mDispatcheds.Enqueue(d.Set(eventName,param));
        }

        public void Dispatch(string eventName, object param1,object param2)
        {
            Dispatched d = (mFreeDipatcheds.Count == 0) ? new Dispatched() : mFreeDipatcheds.Pop();
            mDispatcheds.Enqueue(d.Set(eventName, param1, param2));
        }

        public void Dispatch(string eventName, object param1,object param2,object param3)
        {
            Dispatched d = (mFreeDipatcheds.Count == 0) ? new Dispatched() : mFreeDipatcheds.Pop();
            mDispatcheds.Enqueue(d.Set(eventName, param1, param2, param3));
        }



        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="action">无参返回值为bool值的委托</param>
        /// <returns></returns>
        public int On(string eventName, Func<bool> action)
        {
            return Register(eventName, delegate(object arg1,object arg2,object arg3)
            {
                action();
                return true;
            }
            );
        }

        /// <summary>
        /// 触发一个事件
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="action">无参无返回值的委托</param>
        /// <returns></returns>
        public int On(string eventName, Action action)
        {
            return Register(eventName, delegate(object arg1, object arg2, object arg3)
            {
                action();
                return true;
            }
            );
        }

        public int On<T>(string eventName, Action<T> action)
        {
            return Register(eventName, delegate(object arg1, object arg2, object arg3)
            {
                T param1;
                try
                {
                    param1 = (T) arg1;
                }
                catch
                {
                    param1 = default(T);
                }
                action(param1);
                return true;
            }
            );
        }

        public int On<T1,T2>(string eventName, Action<T1,T2> action)
        {
            return Register(eventName, delegate(object arg1, object arg2, object arg3)
            {
                T1 param1;
                T2 param2;
                try
                {
                    param1 = (T1)arg1;
                }
                catch
                {
                    param1 = default(T1);
                }

                try
                {
                    param2 = (T2)arg2;
                }
                catch
                {
                    param2 = default(T2);
                }
                action(param1,param2);
                return true;
            }
            );
        }

        public int On<T1, T2,T3>(string eventName, Action<T1, T2,T3> action)
        {
            return Register(eventName, delegate(object arg1, object arg2, object arg3)
            {
                T1 param1;
                T2 param2;
                T3 param3;
                try
                {
                    param1 = (T1)arg1;
                }
                catch
                {
                    param1 = default(T1);
                }

                try
                {
                    param2 = (T2)arg2;
                }
                catch
                {
                    param2 = default(T2);
                }

                try
                {
                    param3 = (T3)arg3;
                }
                catch
                {
                    param3 = default(T3);
                }
                action(param1, param2,param3);
                return true;
            }
            );
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="listenID"></param>
        /// <returns></returns>
        public bool Cancel(int listenID)
        {
            return mRegistered.Remove(listenID);
        }

        /// <summary>
        /// 分发
        /// </summary>
        public void DispatchPending()
        {
            while (mDispatcheds.Count>0)
            {
                Dispatched d = mDispatcheds.Dequeue();
                Call(d.mEventName,d.mArg1,d.mArg2,d.mArg3);
                mFreeDipatcheds.Push(d);
            }
        }


        /// <summary>
        /// 消息的注册
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private int Register(string eventName, Func<object, object, object, bool> action)
        {
            int listenID = ++mNextListenID;
            Listen listen = (mFreeListens.Count == 0) ? new Listen() : mFreeListens.Pop();
            listen.mID = listenID;
            listen.mAction = action;
            mRegistered.Add(listenID,listen);
            List<int> eventList;
            if (!mRegisteredEvents.TryGetValue(eventName,out eventList))
            {
                eventList = mRegisteredEvents[eventName] = new List<int>();
            }
            eventList.Add(listenID);
            return listenID;
        }

        private void Call(string eventName, object arg1, object arg2, object arg3)
        {
            List<int> listenerList; //监听列表，如果没有则添加，有的话删除
            if (mRegisteredEvents.TryGetValue(eventName, out listenerList))
            {
                for (int i = listenerList.Count - 1; i >= 0; --i)
                {
                    Listen listener;
                    if (mRegistered.TryGetValue(listenerList[i],out listener))
                    {
                        //如果此事件存在，则在事件列表和时间名称中删除对应的列表，然后添加到空闲的事件栈中
                        if (!listener.mAction(arg1,arg2,arg3))
                        {
                            mRegistered.Remove(listenerList[i]);
                            mFreeListens.Push(listener);
                            listenerList.RemoveAt(i);
                        } 
                    }
                    else //找不到对应事件，则直接移出相应列表
                    {
                        listenerList.RemoveAt(i);
                    }
                }

                //如果为零，则移除全部
                if (listenerList.Count == 0)
                {
                    mRegisteredEvents.Remove(eventName);
                }
            }
        }
    }
}