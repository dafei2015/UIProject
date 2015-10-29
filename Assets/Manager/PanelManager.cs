using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 用于创建面板，将面板放到对应位置上,有创建，隐藏，关闭，显示等功能
/// </summary>
public class PanelManager:MonoBehaviour
{
    private Transform parent;
    private string path;

    /// <summary>
    /// 面板存放位置
    /// </summary>
    public Transform Parent
    {
        get
        {
            if (this.parent == null)
            {
                GameObject gui = io.Gui;
                if (gui)
                {
                    this.parent = gui.transform.Find("Camera");
                }
            }
            return this.parent;
        }
    }

    public void CreatePanle(DialogType type)
    {
#if UNITY_EDITOR
        string typename = Util.ConvertPanelName(type);
        this.CreatePanle(typename);
#else
        base.StartCoroutine(this.OnCreatePanel(type));
#endif
    }

    /// <summary>
    /// 将游戏打包成ios或Android 后运行的，打包成assetbundle
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private IEnumerator OnCreatePanel(DialogType type)
    {
        path = Util.AppContentDataUri + "UI/" + type.ToString() + "Panel.unity3d";
        GameObject go = null;

        WWW bundle = new WWW(path);
        yield return bundle;  //等待加载完成后进行其他操作

        try
        {
            if (bundle.assetBundle.Contains(type.ToString()+"Panel"))
            {
                go =
                    Instantiate(bundle.assetBundle.LoadAsset(type.ToString() + "Panel", typeof (GameObject))) as
                        GameObject;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());   
        }

        go.name = type.ToString() + "Panel";
        go.transform.parent = UIContainer.instance.transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        bundle.assetBundle.Unload(false);  //加载完成后卸载
    }

    /// <summary>
    /// 调试用
    /// </summary>
    /// <param name="name"></param>
    public void CreatePanle(string name)
    {
        //避免两个面板同时出现
        if (this.Parent.FindChild(name) != null)
        {
            return;
        }

        GameObject go = Util.LoadPrefab(Const.PanelPrefabDir + name + ".prefab");
        if ( go == null)
        {
            return;
        }

        GameObject gameObject = null;
        //遇到特殊情况，就是不能讲MainPanel放到UIroot下
        if (name == "MainPanel")
        {
            //先将材质赋给MainUI，其中background是一直存在的
            Util.SetBackground("MainUI/MainGround");
            gameObject = Instantiate(go) as GameObject;
            go.transform.localPosition = new Vector3(0f,0f,0f);
            go.name = name;
            //this.OnCreatePanel(name, go);
        }
        else
        {
            gameObject = Util.AddChild(go, UIContainer.instance.transform);
            gameObject.name = name;
        }
      

        //如果再次改变位置的话使用下面的函数
        //this.OnCreatePanel(name, gameObject);
    }

    /// <summary>
    /// 如果子物体位置不在零零点时，调用此方法
    /// </summary>
    /// <param name="name"></param>
    /// <param name="gameObject"></param>
    private void OnCreatePanel(string name, GameObject go)
    {
        DialogType type = Util.ConvertPanelType(name);
        switch (type)
        {
            case DialogType.LOGIN:
                this.OnLoginPanel(go);
                break;
            case DialogType.CHARACTER:
                break;
            case DialogType.MAIN:
                break;
            case DialogType.WORLD:
                break;
            case DialogType.DUPLICATE:
                break;
            case DialogType.FIGHT:
                break;
            case DialogType.BAG:
                break;
        }
    }

    private void OnLoginPanel(GameObject go)
    {
        go.transform.localPosition = new Vector3(0f,0f,0f);
        io.Container.loginPanel = go;
    }
}