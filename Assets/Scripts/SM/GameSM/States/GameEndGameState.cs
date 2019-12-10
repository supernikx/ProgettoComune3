using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe che gestisce lo stato di fine partita
/// </summary>
public class GameEndGameState : GameSMStateBase
{
    /// <summary>
    /// Riferimento al GameManager
    /// </summary>
    private GameManager gm;
    /// <summary>
    /// Riferimento al group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento all'UI Manager
    /// </summary>
    private UI_Manager uiMng;
    /// <summary>
    /// Riferiemento al pannello di endgame
    /// </summary>
    private UIMenu_EndGame endGamePanel;

    public override void Enter()
    {
        gm = context.GetGameManager();
        groupCtrl = gm.GetLevelManager().GetGroupController();
        uiMng = context.GetGameManager().GetUIManager();
        endGamePanel = uiMng.GetMenu<UIMenu_EndGame>();

        endGamePanel.RetryButtonPressed += HandleRetryButtonPressed;

        groupCtrl.Enable(false);
        uiMng.SetCurrentMenu<UIMenu_EndGame>();
    }

    /// <summary>
    /// Funzione che gestisce l'evento di retry button
    /// </summary>
    private void HandleRetryButtonPressed()
    {
        uiMng.SetCurrentMenu<UIMenu_Loading>();

        PoolManager.instance.ResetPoolObjects(ObjectTypes.Boss1Bullet);
        PoolManager.instance.ResetPoolObjects(ObjectTypes.PlayerBullet);

        Scene sceneToReload = new Scene();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene currentScene = SceneManager.GetSceneAt(i);
            if (currentScene.name != gm.GetSceneReferenceManager().GetSwarmSceneName())
            {
                sceneToReload = currentScene;
                break;
            }
        }

        SceneManager.sceneUnloaded += HandleOnSceneUnloaded;
        SceneManager.UnloadSceneAsync(sceneToReload);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di scaricamento della scena
    /// </summary>
    /// <param name="_scene"></param>
    private void HandleOnSceneUnloaded(Scene _scene)
    {
        SceneManager.sceneUnloaded -= HandleOnSceneUnloaded;
        SceneManager.sceneLoaded += HandleOnSceneLoaded;

        SceneManager.LoadScene(_scene.name, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di caricamento della scena
    /// </summary>
    /// <param name="_scene"></param>
    /// <param name="_mode"></param>
    private void HandleOnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        SceneManager.sceneLoaded -= HandleOnSceneLoaded;
        SceneManager.SetActiveScene(_scene);
        groupCtrl.AgentsSetup();
        NewSceneSetup();

        Complete();
    }

    /// <summary>
    /// Funzione che esegue il Setup della nuova scena
    /// </summary>
    private void NewSceneSetup()
    {
        LevelManager newLvlMng = FindObjectOfType<LevelManager>();
        gm.SetLevelManager(newLvlMng);
        newLvlMng.Setup();
    }

    public override void Exit()
    {
        SceneManager.sceneUnloaded -= HandleOnSceneUnloaded;
        SceneManager.sceneLoaded -= HandleOnSceneLoaded;

        if (endGamePanel != null)
            endGamePanel.RetryButtonPressed -= HandleRetryButtonPressed;
    }
}
