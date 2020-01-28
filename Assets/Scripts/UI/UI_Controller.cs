using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Classe che gestisce la UI di questa scena
/// </summary>
public class UI_Controller : UIManagerBase
{
    // Riferimento all'event system
    [SerializeField]
    private EventSystem eventSystem;

    /// <summary>
    /// Riferimento all'UIDeselectFix
    /// </summary>
    private UIDeselectFix deselectFix;

    /// <summary>
    /// Override della funzione di Setup del UIManagerBase
    /// </summary>
    protected override void CustomSetup()
    {
        eventSystem.gameObject.SetActive(true);

        deselectFix = GetComponent<UIDeselectFix>();
        deselectFix.Setup(eventSystem);
    }

    #region API
    #region Getter
    /// <summary>
    /// Funzione che ritorna l'event system
    /// </summary>
    /// <returns></returns>
    public EventSystem GetEventSystem()
    {
        return eventSystem;
    }
    #endregion
    #endregion
}
