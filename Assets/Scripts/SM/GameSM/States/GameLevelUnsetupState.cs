using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe che gestisce lo stato di LevelUnsetup della GameSM
/// </summary>
public class GameLevelUnsetupState : GameSMStateBase
{
    /// <summary>
    /// Riferimento al GameManager
    /// </summary>
    private GameManager gm;
    /// <summary>
    /// Riferimento all'UI Manager
    /// </summary>
    private UI_Manager uiMng;
    /// <summary>
    /// Nome della scena del MainMenu
    /// </summary>
    private string mainMenuSceneName;

    public override void Enter()
    {
        gm = context.GetGameManager();
        uiMng = gm.GetUIManager();
        uiMng.SetDefaultController();
        uiMng.GetCurrentUIController().SetCurrentMenu<UIMenu_Loading>();

        mainMenuSceneName = gm.GetSceneReferenceManager().GetMainMenuSceneName();
        PersistentData.spawnPointID = -1;

        SceneManager.sceneLoaded += HandleOnSceneLoaded;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di Scena caricata
    /// </summary>
    /// <param name="_scene"></param>
    /// <param name="_mode"></param>
    private void HandleOnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        if (mainMenuSceneName == _scene.name)
        {
            SceneManager.SetActiveScene(_scene);
            Complete();
        }
    }

    public override void Exit()
    {
        if (uiMng != null)
            uiMng.Init();

        SceneManager.sceneLoaded -= HandleOnSceneLoaded;
    }
}
