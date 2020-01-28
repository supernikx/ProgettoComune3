using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe che gestisce lo stato di LevelSetup della GameSM
/// </summary>
public class GameLevelSetupState : GameSMStateBase
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
    /// Nome della scena di Swarm da caricare
    /// </summary>
    private string swarmSceneName;
    /// <summary>
    /// Nome della scena di Hub da caricare
    /// </summary>
    private string hubSceneName;

    public override void Enter()
    {
        gm = context.GetGameManager();
        uiMng = gm.GetUIManager();
        swarmSceneName = gm.GetSceneReferenceManager().GetSwarmSceneName();
        hubSceneName = gm.GetSceneReferenceManager().GetHubSceneName();

        uiMng.SetDefaultController();
        uiMng.GetCurrentUIController().SetCurrentMenu<UIMenu_Loading>();

        SceneManager.sceneLoaded += HandleSwarmSceneLoaded;
        SceneManager.LoadScene(swarmSceneName);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di scena caricata del SceneManager
    /// </summary>
    /// <param name="_scene"></param>
    /// <param name="_loadMode"></param>
    private void HandleSwarmSceneLoaded(Scene _scene, LoadSceneMode _loadMode)
    {
        if (_scene.name == swarmSceneName)
        {
            SceneManager.sceneLoaded -= HandleSwarmSceneLoaded;
            SwarmSceneSetup();

            SceneManager.sceneLoaded += HandeHubSceneLoaded;
            SceneManager.LoadScene(hubSceneName, LoadSceneMode.Additive);
        }
    }


    /// <summary>
    /// Funzione che gestisce l'evento di scena caricata del SceneManager
    /// </summary>
    /// <param name="_scene"></param>
    /// <param name="_loadMode"></param>
    private void HandeHubSceneLoaded(Scene _scene, LoadSceneMode _loadMode)
    {
        if (_scene.name == hubSceneName)
        {
            SceneManager.sceneLoaded -= HandeHubSceneLoaded;
            SceneManager.SetActiveScene(_scene);
            HubSceneSetup();

            Complete();
        }
    }

    /// <summary>
    /// Funzione di Setup della scena di Swarm
    /// </summary>
    private void SwarmSceneSetup()
    {
        PoolManager.instance.Setup();
        GroupController groupCtrl = FindObjectOfType<GroupController>();
        groupCtrl.Setup();
    }

    /// <summary>
    /// Funzione di Setup della scena di Hub
    /// </summary>
    private void HubSceneSetup()
    {
        LevelManager lvlMng = FindObjectOfType<LevelManager>();

        gm.SetLevelManager(lvlMng);
        lvlMng.Setup();
    }

    public override void Exit()
    {
        if (uiMng != null)
            uiMng.Init();

        SceneManager.sceneLoaded -= HandleSwarmSceneLoaded;
        SceneManager.sceneLoaded -= HandeHubSceneLoaded;
    }
}
