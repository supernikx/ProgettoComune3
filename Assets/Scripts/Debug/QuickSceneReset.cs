using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickSceneReset : MonoBehaviour
{
    GroupController groupCtrl;
    GameManager gm;
    UI_Manager uiMng;

    public void Setup(GameManager _gm, GroupController _groupCtrl, UI_Manager _uiMng)
    {
        gm = _gm;
        uiMng = _uiMng;
        groupCtrl = _groupCtrl;
        groupCtrl.OnGroupDead += HandleOnGroupDead;
    }

    private void HandleOnGroupDead()
    {
        groupCtrl.Enable(false);
        PoolManager.instance.ResetPoolObjects(ObjectTypes.Boss1Bullet);
        PoolManager.instance.ResetPoolObjects(ObjectTypes.PlayerBullet);

        UI_Controller currentUiCtrl = uiMng.GetCurrentUIController();
        UIMenu_EndGame endGamePanel = currentUiCtrl.GetMenu<UIMenu_EndGame>();
        if (endGamePanel != null)
        {
            endGamePanel.RetryButtonPressed += HandleOnRetry;
            currentUiCtrl.SetCurrentMenu<UIMenu_EndGame>();
        }
        else
        {
            HandleOnRetry();
        }
    }

    private void HandleOnRetry()
    {
        PoolManager.instance.ResetPoolObjects(ObjectTypes.Boss1Bullet);
        PoolManager.instance.ResetPoolObjects(ObjectTypes.PlayerBullet);

        UI_Controller currentUiCtrl = uiMng.GetCurrentUIController();
        UIMenu_EndGame endGamePanel = currentUiCtrl.GetMenu<UIMenu_EndGame>();
        endGamePanel.RetryButtonPressed -= HandleOnRetry;

        uiMng.SetDefaultController();
        uiMng.GetCurrentUIController().SetCurrentMenu<UIMenu_Loading>();

        Scene sceneToReload = new Scene();
        Scene swarmScene = new Scene();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene currentScene = SceneManager.GetSceneAt(i);
            if (currentScene.name == "Swarm")
                swarmScene = currentScene;
            else
                sceneToReload = currentScene;
        }

        SceneManager.SetActiveScene(swarmScene);
        SceneManager.sceneUnloaded += HandleOnSceneUnloaded;
        SceneManager.UnloadSceneAsync(sceneToReload);
    }

    private void HandleOnSceneUnloaded(Scene _scene)
    {
        SceneManager.sceneUnloaded -= HandleOnSceneUnloaded;
        SceneManager.sceneLoaded += HandleOnSceneLoaded;

        SceneManager.LoadScene(_scene.name, LoadSceneMode.Additive);
    }

    private void HandleOnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        SceneManager.sceneLoaded -= HandleOnSceneLoaded;
        SceneManager.SetActiveScene(_scene);
        groupCtrl.AgentsSetup();
        NewSceneSetup();
    }

    private void NewSceneSetup()
    {
        LevelManager newLvlMng = FindObjectOfType<LevelManager>();
        newLvlMng.Setup();

        uiMng.Init();
        uiMng.GetCurrentUIController().SetCurrentMenu<UIMenu_Gameplay>();

        gm.SetLevelManager(newLvlMng);
    }

    private void OnDisable()
    {
        if (groupCtrl != null)
            groupCtrl.OnGroupDead -= HandleOnGroupDead;
    }
}
