using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using UnityEditor;
using LitJson;

/// <summary>
/// 先读取数据，然后在写入数据
/// </summary>
public class EditorObjectPos : Editor 
{
    /// <summary>
    /// 读取json数据
    /// </summary>
    [MenuItem("Custom Editor/ReadJson")]
    public static void ReadJson()
    {
        string fileDirName = "Assets/SteamingAssets/file/";
        string filename = string.Empty;
        string path = string.Empty;
        TextAsset textAsset = null;
        Object[] objects = Selection.objects;  //存取选择的多项

        for (int i = 0; i < objects.Length; ++i)
        {
            filename = objects[i].name;
            path = fileDirName + filename + ".txt";
            textAsset = Resources.LoadAssetAtPath(path, typeof (TextAsset)) as TextAsset;
            JsonData[] data = JsonMapper.ToObject<JsonData[]>(textAsset.text);

            int j = 0;
            GameObject parentObj = new GameObject("PosObj");
            foreach (JsonData jd in data)
            {
                Vector3 vec = Util.StrToVector3(jd["pos_" + j.ToString()].ToString(), ',');
                GameObject obj = new GameObject("pos_" + j.ToString(), typeof (GameObject));
                obj.transform.parent = parentObj.transform;
                obj.transform.position = vec;
                j++;
            }
        }
    }

    [MenuItem("Custom Editor/SaveJson")]
    public static void SaveJson()
    {
        string filePath = Application.dataPath + @"/StringAssets/file/pos.txt";
        FileInfo fileInfo = new FileInfo(filePath);
        if (!File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        StreamWriter sw = fileInfo.CreateText();
        StringBuilder sb = new StringBuilder();
        JsonWriter writer = new JsonWriter(sb);

        //从场景中开始一个个的遍历
        foreach (EditorBuildSettingsScene s in EditorBuildSettings.scenes)
        {
            if (s.enabled == true)
            {
                string name = s.path;
                EditorApplication.OpenScene(name);
                GameObject parent = GameObject.Find("PosObj");
                if (parent)
                {
                    writer.WriteArrayStart(); //开始写数据
                    for (int i = 0; i < parent.transform.childCount; i++)
                    {
                        Transform obj = parent.transform.GetChild(i);
                        writer.WriteObjectStart();
                        writer.WritePropertyName(obj.name);
                        writer.Write(obj.position.x.ToString() + "," + obj.position.y.ToString() + "," +
                                     obj.position.z.ToString());
                        writer.WriteObjectEnd();
                    }
                    writer.WriteArrayEnd();
                }
            }
        }

        sw.WriteLine(sb.ToString());
        sw.Close();
        sw.Dispose(); //关闭文件流
        AssetDatabase.Refresh();  //刷新
    }
}
