using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 资源管理
/// </summary>
public class AssetManager
{
    public static Dictionary<string ,AssetData>  cacheAsset = new Dictionary<string, AssetData>();

    /// <summary>
    /// 资源数据
    /// </summary>
    public struct AssetData
    {
        public Object asset;    //存储的对象
        public bool isKeep;    //是否常驻内存
    }

    /// <summary>
    /// 对打包的分类
    /// </summary>
    public enum AssetType
    {
        Scene,
        Model,
        UI,
        Effect,
        Audio
    }

    /// <summary>
    /// 资源加载
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="type"></param>
    /// <param name="isKeep"></param>
    /// <returns></returns>
    public static IEnumerator LoadAsset(string assetName, AssetType type, bool isKeep)
    {
        if (!cacheAsset.ContainsKey(assetName))
        {
            string path = string.Empty;
            switch (type)
            {
                case AssetType.Scene:
                    path = "Scene/";
                    break;
                case AssetType.Model:
                    path = "Model/";
                    break;
                case AssetType.UI:
                    path = "UI/";
                    break;
                case AssetType.Effect:
                    path = "Effect/";
                    break;
                case AssetType.Audio:
                    path = "Audio/";
                    break;
            }

            string assetPath = Util.AppContentDataUri + path + assetName + "unity3d";
#if UNITY_EDITOR
            Object asset = Resources.LoadAssetAtPath("Asset/Prefabs/" + path + assetName + "prefab", typeof (GameObject));
           
#else
            WWW  www = new WWW(assetPath);
            yield return www;

            Object asset = www.assetBundle.mainAsset;

            //www.assetBundle.Unload(false);
#endif
            AssetData assetData = new AssetData();                   //将加载的资源缓存
            assetData.asset = asset;
            assetData.isKeep = isKeep;
            cacheAsset.Add(assetName, assetData);
        }
        yield return null;   //只是加载并没有获取
    }

    /// <summary>
    /// 资源的释放
    /// </summary>
    public static void UnloadAssets()
    {
        foreach (string assetName in cacheAsset.Keys)
        {
            if (!cacheAsset[assetName].isKeep)
            {
                if (cacheAsset[assetName].asset !=null)
                {
                    Resources.UnloadAsset(cacheAsset[assetName].asset);
                    cacheAsset.Remove(assetName);
                }
            }
        }
    }

    /// <summary>
    /// 根据名字获得资源
    /// </summary>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public static GameObject GetGameObject(string assetName)
    {
        return GetGameObject(assetName, null);
    }

    /// <summary>
    /// 获得相对应的资源并指定父类
    /// </summary>
    /// <param name="assetName"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject GetGameObject(string assetName, Transform parent)
    {
        //缓存是否包含assetName，并且assetName没有被释放
        if (cacheAsset.ContainsKey(assetName) && cacheAsset[assetName].asset != null)
        {
            GameObject obj = GameObject.Instantiate(cacheAsset[assetName].asset) as GameObject;
            if (parent != null)
            {
                obj.name = assetName;
                Util.AddChild(obj, parent);
            }
            return obj;
        }
        return null;
    }
}