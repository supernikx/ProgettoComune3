using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Funzione che gestisce l'orb
/// </summary>
public class OrbController : MonoBehaviour, IPoolObject
{
    #region Actions
    /// <summary>
    /// Evento lanciato allo spawn dell'orb
    /// </summary>
    public static Action<OrbController> OnOrbSpawn;
    /// <summary>
    /// Evento lanciato al destroy dell'orb
    /// </summary>
    public static Action<OrbController> OnOrbDestroy;
	#endregion

	#region Pool Interface
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
    /// Variabile che identifica il tipo dell'oggetto
    /// </summary>
    private ObjectTypes _objectType;
    public ObjectTypes objectType
    {
        get
        {
            return _objectType;
        }
        set
        {
            _objectType = value;
        }
    }

    /// <summary>
    /// Funzione chiamata allo spawn in Pool dell'oggetto
    /// </summary>
    public void PoolInit()
    {
        if (orbVFX != null)
            orbVFX.Stop();

        if (trailVFX != null)
            EnableTrail(false);

        if (enableDelayRoutine != null)
            StopCoroutine(enableDelayRoutine);
    }

    /// <summary>
    /// Funzione chiamata al reset forzato nella Pool dell'oggetto
    /// </summary>
    public void ResetPool()
    {
        if (orbVFX != null)
            orbVFX.Stop();

        if (trailVFX != null)
            EnableTrail(false);

        if (enableDelayRoutine != null)
            StopCoroutine(enableDelayRoutine);

        OnOrbDestroy?.Invoke(this);
    }
    #endregion

    [Header("Orb References")]
    //Offset della posizione dell'orb
    [SerializeField]
    private Vector3 orbOffset;
    //Delay dell'attivazione
    [SerializeField]
    private float enableDelay;

    [Header("Orb Graphic Settings")]
    //VFX dell'orb
    [SerializeField]
    private ParticleSystem orbVFX;
    //VFX dell'orb
    [SerializeField]
    private ParticleSystem trailVFX;

    /// <summary>
    /// Riferimento alla coroutine di delay activation
    /// </summary>
    private IEnumerator enableDelayRoutine;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    public void Setup()
    {
        PoolManager.OnObjectSpawnEvent?.Invoke(this);

        transform.position += orbOffset;
        if (orbVFX != null)
            orbVFX.Play();

        enableDelayRoutine = EnableDelayCoroutine();
        StartCoroutine(enableDelayRoutine);
    }

    /// <summary>
    /// Coroutine che abilita l'orb con un delay
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableDelayCoroutine()
    {
        yield return new WaitForSeconds(enableDelay);
        if (trailVFX != null)
            EnableTrail(true);

        OnOrbSpawn?.Invoke(this);
    }

    /// <summary>
    /// Funzione cha abilitaa/disabilita il trail
    /// </summary>
    /// <param name="_enable"></param>
    private void EnableTrail(bool _enable)
    {
        var trails = trailVFX.trails;
        trails.enabled = _enable;
    }

    /// <summary>
    /// Funzionne di Destroy
    /// </summary>
    public void Destroy()
    {
        if (orbVFX != null)
            orbVFX.Stop();

        if (trailVFX != null)
            EnableTrail(false);

        if (enableDelayRoutine != null)
            StopCoroutine(enableDelayRoutine);

        OnOrbDestroy?.Invoke(this);
        PoolManager.OnObjectDestroyEvent?.Invoke(this);
    }
}
