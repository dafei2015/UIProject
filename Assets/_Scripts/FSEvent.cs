using System;


/// <summary>
/// 状态机事件的封装
/// </summary>
public class FSEvent
{
    protected FiniteStateMachine.EnterState mEnterDelegate;
    protected FiniteStateMachine.PushState mPushDelegate;
    protected FiniteStateMachine.PopState mPopDelegate;

    protected enum EventType
    {
        NONE,
        ENTER,
        PUSH,
        POP
    };

    protected string mEventName;
    protected FSState mStateOwner;
    protected string mTargetState;
    protected FiniteStateMachine mOwner;
    protected EventType eType;

    public Func<object, object, object, bool> mAction = null;

    public FSEvent(string mEventName, string mTargetState, FSState mStateOwner, FiniteStateMachine mOwner, FiniteStateMachine.EnterState mEnterDelegate, FiniteStateMachine.PushState mPushDelegate, FiniteStateMachine.PopState mPopDelegate)
    {
        this.mEventName = mEventName;
        this.mTargetState = mTargetState;
        this.mStateOwner = mStateOwner;
        this.mOwner = mOwner;
        this.mEnterDelegate = mEnterDelegate;
        this.mPushDelegate = mPushDelegate;
        this.mPopDelegate = mPopDelegate;
        this.eType = EventType.NONE;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="stateName"></param>
    /// <returns></returns>
    public FSState Enter(string stateName)
    {
        mTargetState = stateName;
        eType = EventType.ENTER;
        return mStateOwner;
    }

    public FSState Push(string stateName)
    {
        mTargetState = stateName;
        eType = EventType.PUSH;
        return mStateOwner;
    }

    public void Pop()
    {
        eType = EventType.POP;
    }

    public void Execute(object o1, object o2, object o3)
    {
        if (eType == EventType.POP)
        {
            mPopDelegate();
        }
        else if (eType == EventType.PUSH)
        {
            mPushDelegate(mTargetState, mOwner.CurrentState.StateName);
        }
        else if (eType == EventType.ENTER)
        {
            mEnterDelegate(mTargetState);
        }
        else if (mAction!=null)
        {
            mAction(o1, o2, o3);
        }
    }
}