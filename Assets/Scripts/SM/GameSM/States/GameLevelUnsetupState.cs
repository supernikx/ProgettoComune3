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
    public override void Enter()
    {
        SceneManager.sceneLoaded += HandleOnSceneLoaded;
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Funzione che gestisce l'evento di Scena caricata
    /// </summary>
    /// <param name="_scene"></param>
    /// <param name="_mode"></param>
    private void HandleOnSceneLoaded(Scene _scene, LoadSceneMode _mode)
    {
        SceneManager.SetActiveScene(_scene);
        Complete();
    }

    public override void Exit()
    {
        SceneManager.sceneLoaded -= HandleOnSceneLoaded;
    }
}
