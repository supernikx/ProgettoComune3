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
    /// Riferimento all'uiManager
    /// </summary>
    private UI_Manager uiMng;
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
    public void Setup(UI_Manager _uiMng)
    {
        uiMng = _uiMng;
        eventSystem = uiMng.GetEventSystem();
        isSetupped = true;
    }


    private void Update()
    {
        if (!isSetupped)
            return;

        if (eventSystem.currentSelectedGameObject != null && eventSystem.currentSelectedGameObject != oldSelected)
            oldSelected = eventSystem.currentSelectedGameObject;
        else if (oldSelected != null && eventSystem.currentSelectedGameObject == null)
            eventSystem.SetSelectedGameObject(oldSelected);
    }
}
