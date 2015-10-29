using UnityEngine;

/// <summary>
/// 对话框基类
/// </summary>
public class BaseDialog
{
    private string name;            //名字
    private UILabel title;          //标题
    private UILabel prompt;         //提示框
    private GameObject container;   //物体本身

    protected DialogType _type;

    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public DialogType Type
    {
        get { return this._type; }
        set { this._type = value; }
    }

    public GameObject Container
    {
        get { return this.container; }
        set { this.container = value; }
    }

    /// <summary>
    /// 初始化名字，标题，对话框等基本信息
    /// </summary>
    /// <param name="_container"></param>
    public void InitDialog(GameObject _container)
    {
        GameObject topObj = Util.Child(_container, "TopName");
        if (topObj != null)
        {
            this.prompt = topObj.GetComponent<UILabel>();
        }
    }

    /// <summary>
    /// 打开对话框
    /// </summary>
    /// <param name="container"></param>
    /// <param name="data"></param>
    protected void Open(GameObject container, string data)
    {
        this.container = container;
        this.InitDialog(container);

        if (!data.Equals(string.Empty) && this.title != null)
        {
            this.title.text = data;
        }
    }

    /// <summary>
    /// 关闭对话框
    /// </summary>
    protected void Close()
    {
        
    }
}