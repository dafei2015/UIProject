using UnityEngine;

/// <summary>
/// 启动脚本
/// </summary>
public class GlobalGenerator:MonoBehaviour
{
    void Start()
    {
        InitGameManager();
    }

    private void InitGameManager()
    {
        GameObject go = io.Manager;
        if (go==null)
        {
            GameObject original = (GameObject)Resources.Load(Const.GamePrefabDir + "GameManager");
            go = Instantiate(original);
            go.name = "GameManager";
        }
    }
}