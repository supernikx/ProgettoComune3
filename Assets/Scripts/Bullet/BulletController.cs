using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il bullet
/// </summary>
public class BulletController : MonoBehaviour, IPoolObject
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
    private State _CurrentState;
    public State CurrentState
    {
        get
        {
            return _CurrentState;
        }
        set
        {
            _CurrentState = value;
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

    [Header("Bullet Settings")]
    //Velocità del proiettile
    [SerializeField]
    private float bulletSpeed;
    //Danno del proiettile
    [SerializeField]
    private int bulletDamage;

    /// <summary>
    /// Punto di spawn del proiettile
    /// </summary>
    private Vector3 spawnPosition;
    /// <summary>
    /// bool che idetifica se il bullet è setuppato o no
    /// </summary>
    private bool isSetupped = false;

    /// <summary>
    /// Funzione che esegue il setup
    /// </summary>
    public void Setup()
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

        if (Vector3.Distance(transform.position, spawnPosition) > 100)
            BulletDestroy();
    }

    private void OnTriggerEnter(Collider other)
    {
        BossLifeController bossLifeCtrl = other.GetComponent<BossLifeController>();
        if (bossLifeCtrl != null)
        {
            bossLifeCtrl.TakeDamage(bulletDamage);
            BulletDestroy();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            BulletDestroy();
    }

    /// <summary>
    /// Funzione che rimanda in Pool il Bullet
    /// </summary>
    private void BulletDestroy()
    {
        isSetupped = false;
        OnObjectDestroy?.Invoke(this);
    }
}
