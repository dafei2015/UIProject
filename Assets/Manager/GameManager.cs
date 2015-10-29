using UnityEngine;


public class GameManager:MonoBehaviour
{
    void Start()
    {
        AddManagers();
        io.PanelManager.CreatePanle(DialogType.LOGIN);
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// 将所有管理类添加到GameManager里
    /// </summary>
    private void AddManagers()
    {
        Util.Add<PanelManager>(gameObject);
        Util.Add<DialogManager>(gameObject);
        Util.Add<MusicManager>(gameObject);
    }

    private void InitGui()
    {
        GameObject go = io.Gui;
        if (go == null)
        {
            GameObject original = Util.LoadPrefab(Const.PanelPrefabDir + "MainUI.prefab");
            go = Instantiate(original) as GameObject;
            go.name = "MainUI";
        }
    }
}