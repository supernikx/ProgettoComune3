using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Classe che gestisce il Pannello di Gameplay
/// </summary>
public class UIMenu_Gameplay : UIMenu_Base
{
    [Header("General References")]
    //Riferimento al pannello del Boss
    [SerializeField]
    private UISubmenu_Boss bossPanel;
    //Riferimento al pannello di agent
    [SerializeField]
    private UISubmenu_AgentCounter agentPanel;
    //Riferimento al pannello di vittoria
    [SerializeField]
    private UISubmenu_Win winPanel;
    //Riferimento al pannello di tutorial
    [SerializeField]
    private UISubmenu_Tutorial tutorialPanel;

    /// <summary>
    /// Riferimento al level scene controller
    /// </summary>
    private LevelSceneController lvlSceneCtrl;
    /// <summary>
    /// Riferimento al group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al group shoot controller
    /// </summary>
    private GroupShootController groupShootCtrl;
    /// <summary>
    /// Tempo massimo di ricarica
    /// </summary>
    private float maxReloadingTime;

    /// <summary>
    /// Override funzione che esegue il setup del pannello
    /// </summary>
    public override void CustomSetup(UIManagerBase _manage)
    {
        base.CustomSetup(_manage);
        ToggleBossPanel(false);
        ToggleWinPanel(false);        
    }

    /// <summary>
    /// Override funzione che gestisce accensione/spegnimento menù
    /// </summary>
    /// <param name="_value"></param>
    public override void ToggleMenu(bool _value)
    {
        base.ToggleMenu(_value);

        if (_value)
        {
            lvlSceneCtrl = controller.GetGameManager().GetLevelManager().GetLevelSceneController();
            groupCtrl = controller.GetGameManager().GetLevelManager().GetGroupController();
            groupShootCtrl = groupCtrl.GetGroupShootController();

            agentPanel.Setup(groupCtrl);

            LevelBossController.OnBossFightStart += HandleOnBossFightStart;
            LevelBossController.OnBossFightEnd += HandleOnBossFightEnd;

            LevelTutorialController.OnTutorialOpen += HandleOnTutorialPanelOpen;
            LevelTutorialController.OnTutorialClose += HandleOnTutorialPanelClosed;

            lvlSceneCtrl.OnChangeLevelScene += HandleOnChangeLevelScene;
        }
        else
        {
            if (groupCtrl != null)
                groupCtrl = null;

            if (lvlSceneCtrl != null)
                lvlSceneCtrl.OnChangeLevelScene -= HandleOnChangeLevelScene;

            LevelBossController.OnBossFightStart -= HandleOnBossFightStart;
            LevelBossController.OnBossFightEnd -= HandleOnBossFightEnd;

            LevelTutorialController.OnTutorialOpen -= HandleOnTutorialPanelOpen;
            LevelTutorialController.OnTutorialClose -= HandleOnTutorialPanelClosed;
        }
    }

    /// <summary>
    /// Funzione che attiva/disattiva il pannello del boss
    /// </summary>
    /// <param name="_toggle"></param>
    public void ToggleBossPanel(bool _toggle)
    {
        bossPanel.gameObject.SetActive(_toggle);
    }

    /// <summary>
    /// Funzione che attiva/disattiva il pannello di vittoria
    /// </summary>
    /// <param name="_toggle"></param>
    public void ToggleWinPanel(bool _toggle)
    {
        winPanel.gameObject.SetActive(_toggle);
    }

    /// <summary>
    /// Funzione che attiva/disattiva il pannello di tutorial
    /// </summary>
    /// <param name="_toggle"></param>
    public void ToggleTutorialPanel(bool _toggle)
    {
        tutorialPanel.gameObject.SetActive(_toggle);
    }

    #region Handles
    /// <summary>
    /// Funzione che gestisce l'evento dell'inizio della bossfight
    /// </summary>
    /// <param name="obj"></param>
    private void HandleOnBossFightStart(BossControllerBase _bossCtrl)
    {
        ToggleBossPanel(true);
        bossPanel.Setup(_bossCtrl);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di fine della bossfight
    /// </summary>
    /// <param name="_bossCtrl"></param>
    /// /// <param name="_win"></param>
    private void HandleOnBossFightEnd(BossControllerBase _bossCtrl, bool _win)
    {
        ToggleBossPanel(false);

        if (_win)
            ToggleWinPanel(true);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di apertura del pannello tutorial
    /// </summary>
    /// <param name="_trigger"></param>
    private void HandleOnTutorialPanelOpen(TutorialTrigger _trigger)
    {
        tutorialPanel.SetupTutorialPanel(_trigger.GetTutorialSprite(), _trigger.GetTutorialTitleText(), _trigger.GetTutorialText());
        ToggleTutorialPanel(true);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di chiusura del pannello tutorial
    /// </summary>
    /// <param name="_trigger"></param>
    private void HandleOnTutorialPanelClosed(TutorialTrigger _trigger)
    {
        ToggleTutorialPanel(false);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di cambio scena
    /// </summary>
    private void HandleOnChangeLevelScene()
    {
        ToggleBossPanel(false);
        ToggleWinPanel(false);
    }
    #endregion
}
