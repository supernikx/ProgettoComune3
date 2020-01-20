using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Classe che funziona da manager per il gioco
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    /// <summary>
    /// Funzione che imposta lo script come Singleton
    /// </summary>
    private void Singleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion

    /// <summary>
    /// Riferimento al Controller della GameSM
    /// </summary>
    private GameSMController smCtrl;
    /// <summary>
    /// Riferimento all'UI Manager
    /// </summary>
    private UI_Manager uiMng;
    /// <summary>
    /// Riferimento al scene reference manager
    /// </summary>
    private SceneReferenceManager sceneRefMng;
    /// <summary>
    /// Riferimento al LevelManager
    /// </summary>
    private LevelManager lvlMng;

    protected virtual void Awake()
    {
        Singleton();
    }

    protected virtual void Start()
    {
        smCtrl = GetComponent<GameSMController>();

        GameSMController.Context context = new GameSMController.Context(this);
        smCtrl.Setup(context);
    }

    #region API
    #region Setter
    /// <summary>
    /// Funzione che imposta l'UIManager
    /// </summary>
    /// <param name="_uiMng"></param>
    public void SetUIManager(UI_Manager _uiMng)
    {
        uiMng = _uiMng;
    }

    /// <summary>
    /// Funzione che ritorna il Scene Referenze Manager
    /// </summary>
    /// <param name="_sceneRefMng"></param>
    public void SetSceneReferenceManager(SceneReferenceManager _sceneRefMng)
    {
        sceneRefMng = _sceneRefMng;
    }

    /// <summary>
    /// Funzione che imposta il level manager
    /// </summary>
    /// <param name="_lvlMng"></param>
    public void SetLevelManager(LevelManager _lvlMng)
    {
        lvlMng = _lvlMng;
    }
    #endregion

    #region Getter
    /// <summary>
    /// Funzione che ritorna l'ui Manager
    /// </summary>
    /// <returns></returns>
    public UI_Manager GetUIManager()
    {
        return uiMng;
    }

    /// <summary>
    /// Funzione che ritorna il scene reference manager
    /// </summary>
    /// <returns></returns>
    public SceneReferenceManager GetSceneReferenceManager()
    {
        return sceneRefMng;
    }

    /// <summary>
    /// Funzione che ritorna il LevelManager
    /// </summary>
    /// <returns></returns>
    public LevelManager GetLevelManager()
    {
        return lvlMng;
    }
    #endregion
    #endregion
}
