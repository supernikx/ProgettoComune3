using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Classe base dei Menu di UI
/// </summary>
public class UIMenu_Base : UIControllerBase
{
    [Header("Gamepad Settings")]
    //Bottone selezionato di base quando questo pannello è attivo
    [SerializeField]
    protected GameObject defaultSelecedButton;

    /// <summary>
    /// Riferimento all'UI Manager
    /// </summary>
    protected UI_Controller controller;
    /// <summary>
    /// Riferimento all'event system
    /// </summary>
    protected EventSystem eventSystem;

    /// <summary>
    /// Override della funzione di Setup dell'UIControllerBase
    /// </summary>
    public override void CustomSetup(UIManagerBase _controller)
    {
        controller = _controller as UI_Controller;
        eventSystem = controller.GetEventSystem();
    }

    /// <summary>
    /// Override della funzione di toggle dell'UIControllerBase
    /// </summary>
    /// <param name="_value"></param>
    public override void ToggleMenu(bool _value)
    {
        base.ToggleMenu(_value);

        if (_value)
        {
            eventSystem.SetSelectedGameObject(defaultSelecedButton);
        }
    }

    /// <summary>
    /// Funzione che ritorna il pulsante selezionato di default in questo pannello
    /// </summary>
    /// <returns></returns>
    public GameObject GetDefaultSelectedButton()
    {
        return defaultSelecedButton;
    }

    private void OnDestroy()
    {
        ToggleMenu(false);
    }
}
