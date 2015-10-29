using UnityEngine;

/// <summary>
/// 直接被dialogManager操作
/// </summary>
public class DialogInfo:BaseDialog
{
    private AysnState asynState = AysnState.Completed;

    public AysnState AsynState
    {
        get { return asynState; }
        set { asynState = value; }
    }

    public void Close()
    {
        base.Close();
    }

    public void OpenDialog(GameObject container)
    {
        this.asynState = AysnState.Processing;
        base.Open(container,string.Empty);
    }
}