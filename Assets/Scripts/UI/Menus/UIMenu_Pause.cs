using System;
using UnityEngine;

/// <summary>
/// Classe che gestisce il Pannello di Pausa
/// </summary>
public class UIMenu_Pause : UIMenu_Base
{
    #region Actions
    /// <summary>
    /// Evento che notifica la pressione del bottone Resume
    /// </summary>
    public Action ResumeButtonPressed;
    /// <summary>
    /// Evento che notifica la pressione del bottone MainMenu
    /// </summary>
    public Action MainMenuButtonPressed;
    #endregion

    /// <summary>
    /// Riferimento al level pause controller
    /// </summary>
    private LevelPauseController pauseCtrl;

    /// <summary>
    /// Override funzione che gestisce accensione/spegnimento menù
    /// </summary>
    /// <param name="_value"></param>
    public override void ToggleMenu(bool _value)
    {
        base.ToggleMenu(_value);

        if (_value)
            pauseCtrl = controller.GetGameManager().GetLevelManager().GetLevelPauseController();
        else if (pauseCtrl != null)
            pauseCtrl = null;
    }

    /// <summary>
    /// Funzione che gestisce il bottone di Resume del pannello
    /// </summary>
    public void ResumeButton()
    {
        if (pauseCtrl != null)
            pauseCtrl.SetPause(false);

        ResumeButtonPressed?.Invoke();
    }

    /// <summary>
    /// Funzione che gestisce il bottone di MainMenu del pannello
    /// </summary>
    public void MainMenuButton()
    {
        MainMenuButtonPressed?.Invoke();
    }
}
