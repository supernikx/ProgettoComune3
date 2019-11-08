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
    [Header("Scene Settings")]
    //Nome della scena di Hub da caricare
    [SerializeField]
    private string swarmSceneName;
    [SerializeField]
    private string hubSceneName;

    /// <summary>
    /// Riferimento al GameManager
    /// </summary>
    private GameManager gm;
    /// <summary>
    /// Riferimento all'UI Manager
    /// </summary>
    private UI_Manager uiMng;

    public override void Enter()
    {
        gm = context.GetGameManager();
        uiMng = gm.GetUIManager();
        uiMng.SetCurrentMenu<UIMenu_Loading>();

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
        FindObjectOfType<GroupController>().Setup();
    }

    /// <summary>
    /// Funzione di Setup della scena di Hub
    /// </summary>
    private void HubSceneSetup()
    {
        LevelManager lvlMng = FindObjectOfType<LevelManager>();

        gm.SetLevelManager(lvlMng);
        lvlMng.Setup(gm);
    }

    public override void Exit()
    {
        SceneManager.sceneLoaded -= HandleSwarmSceneLoaded;
        SceneManager.sceneLoaded -= HandeHubSceneLoaded;
    }
}
