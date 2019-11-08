using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe che gestisce lo stato di Gameplay della GameSM
/// </summary>
public class GameGameplayState : GameSMStateBase
{
    /// <summary>
    /// Riferimento all'UI Manager
    /// </summary>
    private UI_Manager uiMng;
    /// <summary>
    /// Riferimento al level manager
    /// </summary>
    private LevelManager lvlMng;
    /// <summary>
    /// Riferimento al level scene controller
    /// </summary>
    private LevelSceneController lvlSceneCtrl;

    public override void Enter()
    {
        uiMng = context.GetGameManager().GetUIManager();

        lvlMng = context.GetGameManager().GetLevelManager();
        lvlSceneCtrl = lvlMng.GetLevelSceneController();

        lvlSceneCtrl.OnChangeLevelScene += HandleOnChangeLevelScene;
        uiMng.SetCurrentMenu<UIMenu_Gameplay>();
    }

    /// <summary>
    /// Funzione che gestisce l'evento di cambio scena del livello
    /// </summary>
    private void HandleOnChangeLevelScene()
    {
        lvlMng.GetGroupController().Enable(false);
        Complete(1);
    }

    public override void Exit()
    {
        lvlSceneCtrl.OnChangeLevelScene -= HandleOnChangeLevelScene;
    }
}
