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
    /// Riferimento al MainMenu Panel
    /// </summary>
    private UIMenu_MainMenu mainMenuPanel;

    public override void Enter()
    {
        uiMng = context.GetGameManager().GetUIManager();

        mainMenuPanel = uiMng.GetMenu<UIMenu_MainMenu>();
        uiMng.SetCurrentMenu<UIMenu_MainMenu>();

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
        mainMenuPanel.StartButtonPressed -= HandleStartButtonPressed;
    }
}
