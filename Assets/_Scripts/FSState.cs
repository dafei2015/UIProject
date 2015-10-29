using System;
using System.Collections.Generic;


/// <summary>
/// 有限机状态的封装,不适用继承实现封装
/// </summary>
public class FSState
{
    protected FiniteStateMachine.EnterState mEnterDelegate;
    protected FiniteStateMachine.PushState mPushDelegate;
    protected FiniteStateMachine.PopState mPopDelegate;

    protected IState mStateObejct;
    protected string mStateName;
    protected FiniteStateMachine mOwner;
    protected Dictionary<string, FSEvent> mTranslationEvents;

    public FSState(IState mStateObejct, FiniteStateMachine mOwner, string mStateName, FiniteStateMachine.EnterState mEnterDelegate, FiniteStateMachine.PushState mPushDelegate, FiniteStateMachine.PopState mPopDelegate)
    {
        this.mStateObejct = mStateObejct;
        this.mOwner = mOwner;
        this.mStateName = mStateName;
        this.mEnterDelegate = mEnterDelegate;
        this.mPushDelegate = mPushDelegate;
        this.mPopDelegate = mPopDelegate;
        this.mTranslationEvents = new Dictionary<string, FSEvent>();
    }

    public IState StateObject
    {
        get { return mStateObejct; }
    }

    public string StateName
    {
        get { return mStateName; }
    }

    /// <summary>
    /// 状态机的触发
    /// </summary>
    /// <param name="eventName"></param>
    /// <returns></returns>
    public FSEvent On(string eventName)
    {
        FSEvent newEvent = new FSEvent(eventName,null,this,mOwner,mEnterDelegate,mPushDelegate,mPopDelegate);
        mTranslationEvents.Add(eventName,newEvent);
        return newEvent;
    }

   /// <summary>
   /// 定义一个模板，可以返回自身的状态，达到可以连写的目的
   /// </summary>
   /// <typeparam name="T"></typeparam>
   /// <param name="eventName"></param>
   /// <param name="action">带有一个参数返回值为bool类型的参数</param>
   /// <returns></returns>
    public FSState On<T>(string eventName, Func<T, bool> action)
    {
        FSEvent newEvent = new FSEvent(eventName, null, this, mOwner, mEnterDelegate, mPushDelegate, mPopDelegate);
        newEvent.mAction = delegate(object o1, object o2, object o3)
        {
            T param1;
            try
            {
                param1 = (T) o1;
            }
            catch
            {
                param1 = default(T);
            }
            action(param1);
            return true;
        };

        mTranslationEvents.Add(eventName,newEvent);
        return this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventName"></param>
    /// <param name="action">带有一个参数，无返回值的委托</param>
    /// <returns></returns>
    public FSState On<T>(string eventName, Action<T> action)
    {
        FSEvent newEvent = new FSEvent(eventName, null, this, mOwner, mEnterDelegate, mPushDelegate, mPopDelegate);
        newEvent.mAction = delegate(object o1, object o2, object o3)
        {
            T param1;
            try
            {
                param1 = (T)o1;
            }
            catch
            {
                param1 = default(T);
            }
            action(param1);
            return true;
        };

        mTranslationEvents.Add(eventName, newEvent);
        return this;
    }

    /// <summary>
    /// 状态触发函数
    /// </summary>
    /// <param name="name"></param>
    public void Trigger(string name,object param1 = null,object param2=null,object param3=null)
    {
        mTranslationEvents[name].Execute(param1, param2, param3);
    }
}