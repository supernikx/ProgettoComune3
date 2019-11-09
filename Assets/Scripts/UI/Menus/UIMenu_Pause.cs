using System;
using UnityEngine;

/// <summary>
/// Classe che gestisce il Pannello di Pausa
/// </summary>
public class UIMenu_Pause : UIControllerBase
{
    /// <summary>
    /// Evento che notifica la pressione del bottone Resume
    /// </summary>
    public Action ResumeButtonPressed;
    /// <summary>
    /// Evento che notifica la pressione del bottone MainMenu
    /// </summary>
    public Action MainMenuButtonPressed;

    /// <summary>
    /// Funzione che gestisce il bottone di Resume del pannello
    /// </summary>
    public void ResumeButton()
    {
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
