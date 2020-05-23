using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enumeratore che identifica gli sati degli oggetti in Pool
/// </summary>
public enum State
{
    InPool,
    InUse,
    Destroying,
}

/// <summary>
/// Interfaccia che identifica gli oggetti che possono essere messi in Pool
/// </summary>
public interface IPoolObject 
{
    /// <summary>
    /// Proprietaro dell'oggetto
    /// </summary>
    GameObject ownerObject { get; set; }

    /// <summary>
    /// L'oggetto
    /// </summary>
    GameObject gameObject { get; }

    /// <summary>
    /// Stato attuale dell'oggetto in relazione alla Pool
    /// </summary>
    State currentState
    {
        get;
        set;
    }

    /// <summary>
    /// Tipo di oggetto
    /// </summary>
    ObjectTypes objectType
    {
        get;
        set;
    }

    /// <summary>
    /// Funzione chiamata all'istanziazione dell'oggetto in Pool
    /// </summary>
    void PoolInit();

    /// <summary>
    /// Funzione chiamata al ritorno forzato in Pool dell'oggetto
    /// </summary>
    void ResetPool();
}
