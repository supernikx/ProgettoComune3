using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe che gestisce la scena del livello
/// </summary>
public class LevelSceneController : MonoBehaviour
{
    #region Actions
    public Action OnChangeLevelScene;
    #endregion

    /// <summary>
    /// Riferimento alla scena attuale
    /// </summary>
    private Scene currentScene;
    /// <summary>
    /// Riferimento al nome della scena da caricare
    /// </summary>
    private string nextSceneName;

    /// <summary>
    /// Funzione di Setup dello script
    /// </summary>
    public void Setup()
    {
        currentScene = SceneManager.GetActiveScene();
        ChangeSceneTrigger.OnExitTriggered += HandleOnExitTriggered;
    }

    /// <summary>
    /// Funzione che gestisce il trigger di cambio scena
    /// </summary>
    /// <param name="_sceneToLoad"></param>
    private void HandleOnExitTriggered(string _sceneToLoad)
    {
        nextSceneName = _sceneToLoad;
        OnChangeLevelScene?.Invoke();
    }

    #region API
    #region Getter
    /// <summary>
    /// Funzione che ritorna la scena attuale
    /// </summary>
    /// <returns></returns>
    public Scene GetCurrentScene()
    {
        return currentScene;
    }

    /// <summary>
    /// Funzione che ritorna il nome della scena da caricare
    /// </summary>
    /// <returns></returns>
    public string GetNextSceneName()
    {
        return nextSceneName;
    }
    #endregion
    #endregion

    private void OnDisable()
    {
        ChangeSceneTrigger.OnExitTriggered -= HandleOnExitTriggered;
    }
}
