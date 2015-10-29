using System.Collections;
using UnityEngine;

public class DialogManager:MonoBehaviour
{
    //管理类的操作判断对象是否存在，增加，删除，重置，获取信息，清除
    private Hashtable dialogs = new Hashtable(); //对整个对话框操作

    /// <summary>
    /// 判断是否存在
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool DialogExist(DialogType type)
    {
        return this.dialogs.ContainsKey(type);
    }

    /// <summary>
    /// 根据类型增加对话框
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public DialogInfo AddDialog(DialogType type)
    {
        DialogInfo dialogInfo = new DialogInfo();
        dialogInfo.Type = type;
        this.dialogs.Add(type,dialogInfo);

        return dialogInfo;
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void ResetDialog()
    {
        IDictionaryEnumerator enumerator = this.dialogs.GetEnumerator();
        while (enumerator.MoveNext())
        {
            DialogInfo dialogInfo = enumerator.Value as DialogInfo;
            dialogInfo.AsynState = AysnState.Completed;
        }
    }

    /// <summary>
    /// 根据类型获取对话框信息
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public DialogInfo GetDianDialogInfo(DialogType type)
    {
        if (!this.DialogExist(type))
        {
            return this.AddDialog(type);
        }

        return this.dialogs[type] as DialogInfo;
    }

    public void RemoveDialog(DialogType type)
    {
        if (this.DialogExist(type))
        {
            this.dialogs.Remove(type);
        }
    }

    public void ClearDialog()
    {
        this.dialogs.Clear();
    }

    public Transform GetDialog(DialogType type)
    {
        if (type == DialogType.NONE)
        {
            return null;
        }

        //对ManiPanel特殊处理，因为MainPanel不在GUi下
        string str = Util.ConvertPanelName(type);
        if (str == "MainPanel")
        {
            Transform obj = null;
            if (GameObject.Find("MainPanel"))
            {
                obj = GameObject.Find("MainPanel").transform;
            }
            return obj;
        }

        return io.Gui.transform.Find(str);
    }
}