using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Classe che risolve il problema di Unity i pulsanti vengono deselezionati se c'è un click esterno
/// </summary>
public class UIDeselectFix : MonoBehaviour
{
    /// <summary>
    /// Riferimento all'event system
    /// </summary>
    private EventSystem eventSystem;
    /// <summary>
    /// Riferimento all'oggetto precedentemente selezionato dall'event system
    /// </summary>
    private GameObject oldSelected;
    /// <summary>
    /// Bool che identifica se lo script è setuppato
    /// </summary>
    private bool isSetupped = false;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_uiMng"></param>
    public void Setup(EventSystem _eventSystem)
    {
        eventSystem = _eventSystem;
        isSetupped = true;
    }


    private void Update()
    {
        if (!isSetupped || eventSystem == null)
            return;

        if (eventSystem.currentSelectedGameObject != null && eventSystem.currentSelectedGameObject != oldSelected)
            oldSelected = eventSystem.currentSelectedGameObject;
        else if (oldSelected != null && eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(oldSelected);
    }

    #region API
    /// <summary>
    /// Funzione che imposta l'event system
    /// </summary>
    /// <param name="_eventSystem"></param>
    public void SetEventSystem(EventSystem _eventSystem)
    {
        eventSystem = _eventSystem;
    }
    #endregion
}
