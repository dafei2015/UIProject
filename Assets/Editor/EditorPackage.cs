using UnityEditor;
using UnityEngine;

/// <summary>
/// 资源打包
/// </summary>
public class EditorPackag:Editor
{
    [MenuItem("Custom Editor/Create All Unity3D")]
    static void CrateAssetBundelsAll()
    {
        Caching.CleanCache();  //先清空缓存
        Object[] selectedAsset = Selection.GetFiltered(typeof (Object), SelectionMode.Unfiltered); //选择器
        string path = Application.streamingAssetsPath;

        path = EditorUtility.OpenFolderPanel("save", path, ""); //打开文件面板
        if (string.IsNullOrEmpty(path))
        {
            return;
        }

        foreach (Object obj in selectedAsset)
        {
            BuildPipeline.BuildAssetBundle(obj, null, path + "/" + obj.name + "unity3d",
                BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets,
                BuildTarget.Android);

        }

        AssetDatabase.Refresh();  //刷新数据库

    }
}