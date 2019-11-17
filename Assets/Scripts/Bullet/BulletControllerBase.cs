using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe base del bullet
/// </summary>
public abstract class BulletControllerBase : MonoBehaviour, IPoolObject
{
    #region Pool Interface
    /// <summary>
    /// Evento che toglie dalla Pool il bullet
    /// </summary>
    public event PoolManagerEvets.Events OnObjectSpawn;
    /// <summary>
    /// Evento che rimette in Pool il bullet
    /// </summary>
    public event PoolManagerEvets.Events OnObjectDestroy;

    /// <summary>
    /// Variabile che identifica l'owner del bullet
    /// </summary>
    private GameObject _ownerObject;
    public GameObject ownerObject
    {
        get
        {
            return _ownerObject;
        }
        set
        {
            _ownerObject = value;
        }
    }

    /// <summary>
    /// Variabile che identifica lo stato della Pool del bullet
    /// </summary>
    private State _currentState;
    public State currentState
    {
        get
        {
            return _currentState;
        }
        set
        {
            _currentState = value;
        }
    }

    /// <summary>
    /// Funzione chiamata allo spawn in Pool dell'oggetto
    /// </summary>
    public void PoolInit()
    {
        return;
    }
    #endregion

    [Header("Base Bullet Settings")]
    //Velocità del proiettile
    [SerializeField]
    protected float bulletSpeed;
    //Range del proiettile
    [SerializeField]
    protected float bulletRange;

    /// <summary>
    /// Punto di spawn del proiettile
    /// </summary>
    protected Vector3 spawnPosition;
    /// <summary>
    /// bool che idetifica se il bullet è setuppato o no
    /// </summary>
    protected bool isSetupped = false;

    /// <summary>
    /// Funzione che esegue il setup
    /// </summary>
    public virtual void Setup()
    {
        spawnPosition = transform.position;
        isSetupped = true;
        OnObjectSpawn?.Invoke(this);
    }

    private void FixedUpdate()
    {
        if (!isSetupped)
            return;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, bulletSpeed);

        if (Vector3.Distance(transform.position, spawnPosition) > bulletRange)
            BulletDestroy();
    }

    /// <summary>
    /// Funzione che rimanda in Pool il Bullet
    /// </summary>
    protected virtual void BulletDestroy()
    {
        isSetupped = false;
        OnObjectDestroy?.Invoke(this);
    }
}
