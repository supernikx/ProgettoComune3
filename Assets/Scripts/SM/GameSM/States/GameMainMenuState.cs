using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di MainMenu della GameSM
/// </summary>
public class GameMainMenuState : GameSMStateBase
{
    /// <summary>
    /// Riferimento all'UI Manager
    /// </summary>
    private UI_Manager uiMng;
    /// <summary>
    /// Riferimento all'UI Controller attuale
    /// </summary>
    private UI_Controller currentUICtrl;
    /// <summary>
    /// Riferimento al MainMenu Panel
    /// </summary>
    private UIMenu_MainMenu mainMenuPanel;

    public override void Enter()
    {
        uiMng = context.GetGameManager().GetUIManager();
        currentUICtrl = uiMng.GetCurrentUIController();

        mainMenuPanel = currentUICtrl.GetMenu<UIMenu_MainMenu>();
        currentUICtrl.SetCurrentMenu<UIMenu_MainMenu>();

        mainMenuPanel.StartButtonPressed += HandleStartButtonPressed;
    }

    /// <summary>
    /// Funzione che gestisce l'evento di Pressione del Bottone Start
    /// </summary>
    private void HandleStartButtonPressed()
    {
        Complete();
    }

    public override void Exit()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.StartButtonPressed -= HandleStartButtonPressed;
    }
}
