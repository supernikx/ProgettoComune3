using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script che gestisce un VFX
/// </summary>
public class GeneralVFXController : MonoBehaviour, IPoolObject
{
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
        var main = particles.main;
        main.playOnAwake = false;
        particles.Stop();
    }

    /// <summary>
    /// Funzione chiamata al reset forzato nella Pool dell'oggetto
    /// </summary>
    public void ResetPool()
    {
        particles.Stop();
    }
    #endregion

    [Header("References")]
    [SerializeField]
    private ParticleSystem particles;

    /// <summary>
    /// Funzione che esegue lo spawn del VFX
    /// </summary>
    /// <param name="_spawnPosition"></param>
    public void Spawn(Vector3 _spawnPosition)
    {
        transform.position = _spawnPosition;
        particles.Play();
        PoolManager.OnObjectSpawnEvent?.Invoke(this);
    }

    /// <summary>
    /// Callback della fine del VFX
    /// </summary>
    public void OnParticleSystemStopped()
    {
        particles.Stop();
        PoolManager.OnObjectDestroyEvent?.Invoke(this);
    }
}
