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
    /// Riferimento al group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al level scene controller
    /// </summary>
    private LevelSceneController lvlSceneCtrl;
    /// <summary>
    /// Riferimento al level pause controller
    /// </summary>
    private LevelPauseController lvlPauseCtrl;

    public override void Enter()
    {
        uiMng = context.GetGameManager().GetUIManager();

        lvlMng = context.GetGameManager().GetLevelManager();
        lvlSceneCtrl = lvlMng.GetLevelSceneController();
        lvlPauseCtrl = lvlMng.GetLevelPauseController();
        groupCtrl = lvlMng.GetGroupController();

        groupCtrl.OnGroupDead += HandleOnGroupDead;
        lvlSceneCtrl.OnChangeLevelScene += HandleOnChangeLevelScene;
        lvlPauseCtrl.OnGamePause += HandleOnGamePause;

        uiMng.SetCurrentMenu<UIMenu_Gameplay>();
    }

    #region Handles
    /// <summary>
    /// Funzione che gestisce l'evento di cambio scena del livello
    /// </summary>
    private void HandleOnChangeLevelScene()
    {
        Complete(1);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di inizio pausa
    /// </summary>
    private void HandleOnGamePause()
    {
        Complete(2);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di morte del gruppo
    /// </summary>
    private void HandleOnGroupDead()
    {
        Complete(3);
    }
    #endregion

    public override void Exit()
    {
        if (lvlSceneCtrl != null)
            lvlSceneCtrl.OnChangeLevelScene -= HandleOnChangeLevelScene;

        if (lvlPauseCtrl != null)
            lvlPauseCtrl.OnGamePause -= HandleOnGamePause;

        if (groupCtrl != null)
            groupCtrl.OnGroupDead -= HandleOnGroupDead;
    }
}
