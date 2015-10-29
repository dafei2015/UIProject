using System.Collections;
using System.IO;
using UnityEngine;

/// <summary>
/// 重新将Resources封装，将文本，图像和实例化都进行封装
/// </summary>
public class Resource
{
    private static Hashtable texts = new Hashtable();
    private static Hashtable images = new Hashtable();
    private static Hashtable prefabs = new Hashtable();

    /// <summary>
    /// 在编辑模式下加载预设体，然后保存到哈希表中
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static GameObject LoadPrefab(string path)
    {
        object obj = Resource.prefabs[path];
        if (obj == null)
        {
            Resource.prefabs.Remove(path);
            //Resources.LoadAssetAtPath 加载所在路径的资源，只在编辑模式下使用
#if UNITY_EDITOR
            GameObject gameObject = (GameObject) Resources.LoadAssetAtPath(path, typeof (GameObject));
#else
            GameObject gameObject = (GameObject) Resources.Load(path, typeof(GameObject));
#endif
            Resource.prefabs.Add(path,gameObject);
            return gameObject;
        }
        return obj as GameObject;
    }

    /// <summary>
    /// 读取文本信息
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="ext">后缀名</param>
    /// <returns></returns>
    public static string LoadTextFile(string path, string ext)
    {
        object obj = Resource.texts[path];
        if (obj == null)
        {
            Resource.texts.Remove(path);
            string text = string.Empty;
            string pathstr = Util.AppContentDataUri + path + ext;
            text = File.ReadAllText(pathstr);
            Resource.texts.Add(path,text);
            return text;
        }
        return obj as string;
    }

    /// <summary>
    /// 根据路径加载图片
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Texture2D LoadTexture(string path)
    {
        object obj = Resource.images[path];
        if (obj == null)
        {
            Resource.images.Remove(path);
            Texture2D texture2D = (Texture2D) Resources.Load(path, typeof (Texture2D));
            Resource.images.Add(path, texture2D);
            return texture2D;
        }
        return obj as Texture2D;
    }
}