using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 一些通用函数方法
/// </summary>
public class Util : MonoBehaviour
{
    /// <summary>
    /// 只能读取不能修改，此时分两种情况
    /// 1.直接放到项目根路径下来保存文件，这在移动端是没有访问权限的
    /// 2，在项目中创建StreamingAssets文件夹保存文件，可使用Application.dataPath读取文件操作，在不同平台下读取方式不同
    /// </summary>
    public static Uri AppContentDataUri
    {
        get
        {
            string dataPath = Application.dataPath;
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    return new Uri(dataPath +"/Raw/");
                case RuntimePlatform.Android:
                    return new Uri("jar:file://"+dataPath+"!/assets/");
                    
            }
            return new Uri(dataPath+"/StreamingAssets/");
        }
    }

    /// <summary>
    /// 固定路径，存在沙盒中，既可以读也可以写
    /// </summary>
    public static Uri AppPersistentDataUri
    {
        get
        {
           return new Uri(Application.persistentDataPath+"/");
        }
    }

    /// <summary>
    /// 删除一个对象
    /// </summary>
    /// <param name="go"></param>
    public static void SafeDestroy(GameObject go)
    {
        if (go != null)
        {
            Destroy(go);
        }
    }

    public static void SafeDestroy(Transform go)
    {
        if (go!=null)
        {
            SafeDestroy(go.gameObject);
        }
    }

    /// <summary>
    /// 清除内存
    /// </summary>
    public static void ClearMemory()
    {
        GC.Collect();
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// 将组件添加到对应的物体上
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <returns></returns>
    public static T Add<T>(GameObject go) where T : Component
    {
        if (go != null)
        {
            T[] compentents = go.GetComponents<T>();
            for (int i = 0; i < compentents.Length; i++)
            {
                if (compentents[i] != null)
                {
                    Destroy(compentents[i]);
                }
            }
            return go.AddComponent<T>();
        }
        return (T) ((object) null);
    }

    public static T Add<T>(Transform go) where T : Component
    {
        return Add<T>(go.gameObject);
    }

    /// <summary>
    /// 增加孩子物体
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject AddChild(GameObject prefab, Transform parent)
    {
        return NGUITools.AddChild(parent.gameObject,prefab);
    }

    /// <summary>
    /// 获取对象子节点上的某个组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <param name="subNode"></param>
    /// <returns></returns>
    public static T Get<T>(GameObject go, string subNode) where T : Component
    {
        if (go != null)
        {
            Transform transform = go.transform.FindChild(subNode);
            if (transform != null)
            {
                return transform.GetComponent<T>();
            }
        }
        return (T) ((object) null);
    }

    public static T Get<T>(Component go, string subNode) where T : Component
    {
        if (go != null)
        {
            return go.transform.FindChild(subNode).GetComponent<T>();
        }
        return (T)((object)null);
    }

    /// <summary>
    /// 直接获取同层级物体
    /// </summary>
    /// <param name="go"></param>
    /// <param name="subNode"></param>
    /// <returns></returns>
    public static GameObject Peer(GameObject go, string subNode)
    {
        return Util.Peer(go.transform, subNode);
    }

    public static GameObject Peer(Transform go, string subNode)
    {
        Transform transform = go.parent.FindChild(subNode);
        if (transform == null)
        {
            return null;
        }
        return transform.gameObject;
    }

    /// <summary>
    /// 获取孩子物体
    /// </summary>
    /// <param name="go"></param>
    /// <param name="subNode"></param>
    /// <returns></returns>
    public static GameObject Child(GameObject go, string subNode)
    {
        return Util.Child(go.transform, subNode);
    }

    public static GameObject Child(Transform go, string subNode)
    {
        Transform transform = go.FindChild(subNode);
        if (transform == null)
        {
            return null;
        }
        return transform.gameObject;
    }
    /// <summary>
    /// 加载预设体
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static GameObject LoadPrefab(string path)
    {
        return Resource.LoadPrefab(path);
    }

    /// <summary>
    /// 默认调用文本文件
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string LoadText(string path)
    {
        return Util.LoadTextFile(path, "txt");
    }

    public static string LoadXML(string path)
    {
        return Util.LoadTextFile(path, "xml");
    }
    public static string LoadTextFile(string path, string ext)
    {
        return Resource.LoadTextFile(path, ext);
    }

    /// <summary>
    /// 加载图片
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Texture2D LoadTexture(string path)
    {
        return Resource.LoadTexture(path);
    }

    public static Dictionary<string ,string> textDic = new Dictionary<string, string>();
    /// <summary>
    /// 从配置文件中读取本文信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetTextFromConfig(string id)
    {
        try
        {
            return textDic[id];
        }
        catch (Exception)
        {

            return id;
        }
    }

    /// <summary>
    /// 将字符串转换为类型
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static DialogType ConvertPanelType(string name)
    {
        switch (name)
        {
            case "LoginPanel":
                return DialogType.LOGIN;
            case "MainPanel":
                return DialogType.MAIN;
            case "WorldPanel":
                return DialogType.WORLD;
            case "DuplicatePanel":
                return DialogType.DUPLICATE;
        }
        return DialogType.NONE;
    }

    /// <summary>
    /// 将类型转换为字符串
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string ConvertPanelName(DialogType type)
    {
        switch (type)
        {
            case DialogType.NONE:
                break;
            case DialogType.LOGIN:
                return "LoginPanel";
            case DialogType.CHARACTER:
                return "CharacterPanel";
            case DialogType.MAIN:
                return "MainPanel";
            case DialogType.WORLD:
                return "WorldPanel";
            case DialogType.DUPLICATE:
                return "DuplicatePanel";
            case DialogType.FIGHT:
                return "FightPanel";
        }
        return string.Empty;
    }

    /// <summary>
    /// 将字符串转换为向量
    /// </summary>
    /// <param name="position"></param>
    /// <param name="c">分隔符</param>
    /// <returns></returns>
    public static Vector3 StrToVector3(string position, char c)
    {
        string[] array = position.Split(c);
        float x = Util.Float(array[0]);
        float y = Util.Float(array[1]);
        float z = Util.Float(array[2]);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// 将字符串转换为float 
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    public static float Float(object o)
    {
        return (float) Math.Round((double) Convert.ToSingle(o), 2);
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="type"></param>
    public static void HideDialog(DialogType type)
    {
        Transform dialog = GetDialog(type);
        if (dialog != null)
        {
            dialog.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <param name="type"></param>
    public static void ShowDialog(DialogType type)
    {
        Transform dialog = GetDialog(type);
        if (dialog != null)
        {
            dialog.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 关闭面板
    /// </summary>
    /// <param name="type"></param>
    public static void CloseDialog(DialogType type)
    {
        Transform dialog = GetDialog(type);
        if (dialog != null)
        {
            Destroy(dialog.gameObject);
        }
        
    }

    public static void ClearPanle()
    {
        io.Container.ClearAll();
    }

    /// <summary>
    /// 隐藏打开的对话框
    /// </summary>
    /// <returns>返回打开的数量</returns>
    public static int CloseOpenDialogs()
    {
        int num = 0;

        List<GameObject> allPanel = io.Container.AllPlane;
        for (int i = 0; i < allPanel.Count; i++)
        {
            GameObject gameObject = allPanel[i];
            if (!gameObject) continue;
            DialogType dialogType = Util.ConvertPanelType(gameObject.name);
            if (dialogType == DialogType.NONE) continue;
            Util.HideDialog(dialogType);
            num ++;
        }
        return num;
    }

    /// <summary>
    /// 判断是否已经存在
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static bool CanOpen(DialogType type)
    {
        List<GameObject> allPanel = io.Container.AllPlane;
        for (int i = 0; i < allPanel.Count; i++)
        {
            GameObject gameObject = allPanel[i];
            if (!gameObject) continue;
            DialogType dialogType = Util.ConvertPanelType(gameObject.name);
            if (dialogType == type)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 根据类型获得对应的面板
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private static Transform GetDialog(DialogType type)
    {
        if (type == DialogType.NONE)
        {
            return null;
        }

        Transform dialog = io.DialogManager.GetDialog(type);
        return dialog;
    }

    /// <summary>
    /// 根据名字设置背景图
    /// </summary>
    /// <param name="background">图片位置放在Resources文件夹下，可根据名字直接加载</param>
    public static void SetBackground(string background)
    {
        GameObject go = GameObject.FindWithTag("BackGround");
        if (go == null)
        {
            return;
        }

        UITexture component = go.GetComponent<UITexture>();
        if (!string.IsNullOrEmpty(background))
        {
            Texture2D texture2D = Util.LoadTexture(background);
            component.mainTexture = texture2D;
        }
        else
        {
            component.mainTexture = null;
        }
    }

    /// <summary>
    /// 卸载图片纹理
    /// </summary>
    /// <param name="go"></param>
    public static void UnLoadTexture(GameObject go)
    {
        if (go != null)
        {
            UITexture compontent = go.GetComponent<UITexture>();
            if (compontent != null)
            {
                //卸载纹理
                Resources.UnloadAsset(compontent.material.mainTexture);
            }
        }
    }

    /// <summary>
    /// 根据纹理直接卸载
    /// </summary>
    /// <param name="texture"></param>
    public static void UnLoadTexture(UITexture texture)
    {
        if (texture == null)
        {
            return;
        }
        Resources.UnloadAsset(texture.material.mainTexture);
    }
}
