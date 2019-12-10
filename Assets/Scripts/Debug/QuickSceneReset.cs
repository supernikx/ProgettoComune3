using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickSceneReset : MonoBehaviour
{
    GroupController groupCtrl;

    public void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;
        groupCtrl.OnGroupDead += HandleOnGroupDead;
    }

    private void HandleOnGroupDead()
    {
        PoolManager.instance.ResetPoolObjects(ObjectTypes.Boss1Bullet);
        PoolManager.instance.ResetPoolObjects(ObjectTypes.PlayerBullet);

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
    }

    private void OnDisable()
    {
        if (groupCtrl != null)
            groupCtrl.OnGroupDead -= HandleOnGroupDead;
    }
}
