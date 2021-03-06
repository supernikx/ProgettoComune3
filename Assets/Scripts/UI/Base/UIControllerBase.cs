﻿using UnityEngine;

/// <summary>
/// Classe che fa da base per tutti i panneli principali di menù
/// </summary>
public abstract class UIControllerBase : MonoBehaviour
{
    /// <summary>
    /// Stato di attivo o disattivo del menù
    /// </summary>
    protected bool isActive;

    /// <summary>
    /// Setup del menu
    /// </summary>
    public void Setup(UIManagerBase _manager)
    {
        CustomSetup(_manager);
    }

    /// <summary>
    /// Funzione chimata al setup della classe base
    /// </summary>
    public virtual void CustomSetup(UIManagerBase _manager) { }

    /// <summary>
    /// Funzione che ritorna true se il menù è attivo, false altrimenti
    /// </summary>
    /// <returns></returns>
    public bool IsActive()
    {
        return isActive;
    }

    /// <summary>
    /// Funzion che attiva o disattiva il GameObject del menù
    /// </summary>
    /// <param name="_value"></param>
    public virtual void ToggleMenu(bool _value)
    {
        isActive = _value;
        gameObject.SetActive(isActive);
    }
}