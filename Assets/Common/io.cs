using UnityEngine;

/// <summary>
/// 主要用于管理调用Manager
/// </summary>
public class io:MonoBehaviour
{
    private static GameObject _manager;
    private static GameManager _gameManager;
    private static PanelManager _panelManager;
    private static DialogManager _dialogManager;
    private static MusicManager _musicManager;

    private static UIContainer _container;
    public static GameObject Manager
    {
        get
        {
            if (_manager == null)
            {
                io._manager = GameObject.FindWithTag("GameManager");
            }
            return _manager;
        }
    }

    public static GameManager GameManager
    {
        get
        {
            if (_gameManager == null)
            {
                io._gameManager = io.Manager.GetComponent<GameManager>();
            }
            return _gameManager;
        }
    }

    public static PanelManager PanelManager
    {
        get
        {
            if (_panelManager == null)
            {
                io._panelManager = io.Manager.GetComponent<PanelManager>();
            }
            return _panelManager;
        }
    }

    public static MusicManager MusicManager
    {
        get
        {
            if (_musicManager == null)
            {
                io._musicManager = io.Manager.GetComponent<MusicManager>();
            }
            return _musicManager;
        }
    }

    public static DialogManager DialogManager
    {
        get
        {
            if (_dialogManager == null)
            {
                io._dialogManager = io.Manager.GetComponent<DialogManager>();
            }
            return _dialogManager;
        }
    }

    public static GameObject Gui
    {
        get
        {
            return GameObject.FindWithTag("GUI");
        }
    }

    public static UIContainer Container
    {
        get
        {
            if (_container)
            {
                io._container = io.Gui.GetComponent<UIContainer>();
            }
            return _container;
        }
    }
}