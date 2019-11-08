using System;
using UnityEngine;

/// <summary>
/// Classe che gestisce il Pannello di MainMenu
/// </summary>
public class UIMenu_MainMenu : UIControllerBase
{
    /// <summary>
    /// Evento che notifica la pressione del bottone Start
    /// </summary>
    public Action StartButtonPressed;

    /// <summary>
    /// Funzione che gestisce il bottone di Start del pannello
    /// </summary>
    public void StartButton()
    {
        StartButtonPressed?.Invoke();
    }
}
