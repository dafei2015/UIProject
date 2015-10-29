using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 将Resource下的界面加载
/// </summary>
public class UIContainer:MonoBehaviour
{
    public static UIContainer instance;

    public GameObject loginPanel;
    public GameObject mainPanel;
    public GameObject fightPanel;
    public GameObject duplicatePanel;
    public GameObject worldPanel;
    public GameObject taskPanel;

    //存放对象列表，为了便于管理
    public List<GameObject> panles = new List<GameObject>();

    //获取所有面板
    public List<GameObject> AllPlane
    {
        get
        {
            this.panles.Clear();
            this.AddPanel(this.loginPanel);
            this.AddPanel(this.mainPanel);
            this.AddPanel(this.taskPanel);
            this.AddPanel(this.worldPanel);
            this.AddPanel(this.duplicatePanel);
            this.AddPanel(this.fightPanel);
            return this.panles;
        }
    }

    /// <summary>
    /// 封装List的Add方法
    /// </summary>
    private void AddPanel(GameObject go)
    {
        if (go!=null)
        {
            this.panles.Add(go);
        }
    }

    /// <summary>
    /// 情况面板列表
    /// </summary>
    public void ClearAll()
    {
        List<GameObject> all = AllPlane;
        foreach (GameObject go in all)
        {
            if (go!=null)
            {
                Destroy(go);
            }
        }

        panles.Clear();
    }

    void Start()
    {
        DontDestroyOnLoad(base.gameObject);
        if (instance == null)
        {
            instance = this;
        }
    }
}