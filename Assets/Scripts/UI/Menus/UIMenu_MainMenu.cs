using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Classe che gestisce il Pannello di MainMenu
/// </summary>
public class UIMenu_MainMenu : UIMenu_Base
{
    #region Actions
    /// <summary>
    /// Evento che notifica la pressione del bottone Start
    /// </summary>
    public Action StartButtonPressed;
    #endregion

    /// <summary>
    /// Funzione che gestisce il bottone di Start del pannello
    /// </summary>
    public void StartButton()
    {
        StartButtonPressed?.Invoke();
    }

    /// <summary>
    /// Funzione che gestisce il bottone di Quit del pannello
    /// </summary>
    public void QuitButton()
    {
        Application.Quit();
    }
}
