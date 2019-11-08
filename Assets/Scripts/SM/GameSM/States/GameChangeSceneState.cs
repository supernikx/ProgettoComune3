using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe che gestisce lo stato di cambio scena di gioco
/// </summary>
public class GameChangeSceneState : GameSMStateBase
{
    /// <summary>
    /// Riferimento al GameManager
    /// </summary>
    private GameManager gm;
    /// <summary>
    /// Riferimento al Level Manager
    /// </summary>
    private LevelManager lvlMng;
    /// <summary>
    /// Riferimento all'UI Manager
    /// </summary>
    private UI_Manager uiMng;
    /// <summary>
    /// Riferimento al nome della scena da caricare
    /// </summary>
    private string sceneToLoadName;
    /// <summary>
    /// Riferimento alla scena da scaricare
    /// </summary>
    private Scene sceneToUnload;

    public override void Enter()
    {
        gm = context.GetGameManager();
        uiMng = gm.GetUIManager();
        uiMng.SetCurrentMenu<UIMenu_Loading>();

        lvlMng = gm.GetLevelManager();
        sceneToUnload = lvlMng.GetLevelSceneController().GetCurrentScene();
        sceneToLoadName = lvlMng.GetLevelSceneController().GetNextSceneName();

        SceneManager.sceneLoaded += HandleOnNewSceneLoaded;
        SceneManager.LoadScene(sceneToLoadName, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di caricamento della nuova scena
    /// </summary>
    /// <param name="_loadedScene"></param>
    /// <param name="_mode"></param>
    private void HandleOnNewSceneLoaded(Scene _loadedScene, LoadSceneMode _mode)
    {
        if (_loadedScene.name == sceneToLoadName)
        {
            GroupController groupCtrl = context.GetGameManager().GetLevelManager().GetGroupController();
            SceneManager.sceneLoaded -= HandleOnNewSceneLoaded;

            SceneManager.SetActiveScene(_loadedScene);
            SceneManager.UnloadSceneAsync(sceneToUnload);

            NewSceneSetup();

            Complete();
        }
    }

    /// <summary>
    /// Funzione che esegue il Setup della nuova scena
    /// </summary>
    private void NewSceneSetup()
    {
        LevelManager newLvlMng = FindObjectOfType<LevelManager>();
        GroupController groupCtrl = context.GetGameManager().GetLevelManager().GetGroupController();


        lvlMng = newLvlMng;
        gm.SetLevelManager(newLvlMng);
        lvlMng.Setup(gm);
    }

    public override void Exit()
    {
        SceneManager.sceneLoaded -= HandleOnNewSceneLoaded;
    }
}
