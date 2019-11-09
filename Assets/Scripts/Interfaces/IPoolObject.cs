using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che contiene gli eventi della Pool
/// </summary>
public class PoolManagerEvets{
    public delegate void Events(IPoolObject _gameObject);
}

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
    State CurrentState
    {
        get;
        set;
    }

    /// <summary>
    /// Funzione chiamata all'istanziazione dell'oggetto in Pool
    /// </summary>
    void PoolInit();

    /// <summary>
    /// Evento che gestisce l'uscita dell'oggetto dalla Pool
    /// </summary>
    event PoolManagerEvets.Events OnObjectSpawn;
    /// <summary>
    /// Evento che gestisce il ritorno dell'oggetto dalla Pool
    /// </summary>
    event PoolManagerEvets.Events OnObjectDestroy;
}
