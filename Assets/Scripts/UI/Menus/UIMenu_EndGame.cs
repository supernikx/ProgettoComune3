using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il pannello di EndGame
/// </summary>
public class UIMenu_EndGame : UIMenu_Base
{
    #region Actions
    /// <summary>
    /// Evento che notifica la pressione del pulsante di Retry
    /// </summary>
    public Action RetryButtonPressed;
    #endregion

    /// <summary>
    /// Funzione che gestisce il pulsante di Retry del pannello
    /// </summary>
    public void RetryButton()
    {
        RetryButtonPressed?.Invoke();
    }
}
