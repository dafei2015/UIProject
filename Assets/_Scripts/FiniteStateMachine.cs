
using System.Collections.Generic;
using UnityEditor;

public class FiniteStateMachine
{
    //进入状态
    public delegate void EnterState(string stateName);

    //加入状态
    public delegate void PushState(string stateName, string lastStateName);

    //弹出状态
    public delegate void PopState();

    //声明字典存储名字和状态
    protected Dictionary<string, FSState> mStates;

    //进入点
    protected string mEnterPoint;

    //先进后出的堆栈
    protected Stack<FSState> mStateStack;

    /// <summary>
    /// 构造函数，对字段初始化
    /// </summary>
    public FiniteStateMachine()
    {
        mStates = new Dictionary<string, FSState>();
        mStateStack = new Stack<FSState>();
        mEnterPoint = null;
    }

    //当前状态
    public FSState CurrentState
    {
        get
        {
            if (mStateStack.Count == 0)
            {
                return null;
            }
            return mStateStack.Peek(); //返回当前栈顶元素但不移出
        }
    }
    /// <summary>
    /// 状态的更新
    /// </summary>
    public void Update()
    {
        //每帧更新，判断如果当前状态为空，则将当前进入点状态放入到栈中
        if (CurrentState == null)
        {
            mStateStack.Push(mStates[mEnterPoint]);
            CurrentState.StateObject.OnEnter(null); //然后进入当前状态
        }

        //然后更新当前状态
        CurrentState.StateObject.OnUpdate();
    }

    /// <summary>
    /// 注册，即将该状态加入到列表中
    /// </summary>
    /// <param name="stateName"></param>
    /// <param name="stateObject"></param>
    public void Register(string stateName, IState stateObject)
    {
        if (mStates.Count == 0)
        {
            mEnterPoint = stateName;
        }
        mStates.Add(stateName,new FSState(stateObject,this,stateName,Enter,Push,Pop));
    }

    /// <summary>
    /// 返回一个状态
    /// </summary>
    /// <param name="stateName"></param>
    /// <returns></returns>
    public FSState State(string stateName)
    {
        return mStates[stateName];
    }

    /// <summary>
    /// 设置入口点
    /// </summary>
    /// <param name="startName"></param>
    public void EnterPoint(string startName)
    {
        mEnterPoint = startName;
    }

    /// <summary>
    /// 进入某个状态
    /// </summary>
    /// <param name="stateName"></param>
    public void Enter(string stateName)
    {
        Push(stateName, Pop(stateName));
    }

    public void Push(string newState)
    {
        string lastName = null;

        //如果栈中有状态将该状态标记为上一状态
        if (mStateStack.Count > 1)
        {
            lastName = mStateStack.Peek().StateName;
        }

        Push(newState,lastName);
    }

    /// <summary>
    /// 对字典和堆栈的操作
    /// </summary>
    /// <param name="stateName"></param>
    /// <param name="lastStateName"></param>
    protected void Push(string stateName, string lastStateName)
    {
        //将该状态放入栈顶
        mStateStack.Push(mStates[stateName]);
        
        //得到该状态并将该状态的下一个状态标记为lastStateName
        mStateStack.Peek().StateObject.OnEnter(lastStateName);
    }

    public void Pop()
    {
        Pop(null);
    }

    protected string Pop(string newName)
    {
        FSState lastState = mStateStack.Peek();
        string newState = null;
        if (newName == null && mStateStack.Count>1)
        {
            int index = 0;
            foreach (FSState item in mStateStack)
            {
                if (index++ == mStateStack.Count - 2)
                {
                    newState = item.StateName;
                }
            }
        }
        else
        {
            newState = newName;
        }

        string lastStateName = null;
        if (lastState != null)
        {
            lastStateName = lastState.StateName;
            //栈顶状态退出后将进入倒数第二个状态，即标记该状态离开后进入下一个状态
            lastState.StateObject.OnExit(newState);
        }

        mStateStack.Pop();
        return lastStateName;
    }

    /// <summary>
    /// 触发
    /// </summary>
    /// <param name="newName"></param>
    public void Trigger(string eventName, object param1 = null, object param2 = null, object param3 = null)
    {
        CurrentState.Trigger(eventName,param1,param2,param3);
    }
}