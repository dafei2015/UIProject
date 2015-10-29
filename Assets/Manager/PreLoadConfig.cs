using System.Collections;
using UnityEngine;

public class PreLoadConfig:MonoBehaviour
{
    public static string text;
    void Start()
    {
        
    }

    IEnumerator LoadPosFile()
    {
        string path = Util.AppContentDataUri + Const.posfile + ".txt";
        WWW www = new WWW(path);
        yield return www;
        text = www.text;
    }
}