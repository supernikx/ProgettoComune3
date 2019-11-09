using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enumeratore che identifica i tipi di oggetti presenti in Pool
/// </summary>
public enum ObjectTypes
{
    Agent,
    Bullet,
}

/// <summary>
/// Classe che contiene i dati da impostare in inspector per configurare la Pool
/// </summary>
[System.Serializable]
public class PoolObjects
{
    public GameObject prefab;
    public int ammount;
    public ObjectTypes objectType;
}

/// <summary>
/// Classe che gestisce la Pool
/// </summary>
public class PoolManager : MonoBehaviour
{
    #region Events
    /// <summary>
    /// Evento chiamato quando un oggetto viene Poolato
    /// </summary>
    /// <param name="pooledObject"></param>
    public delegate void PoolManagerEvent(IPoolObject pooledObject);
    public PoolManagerEvent OnObjectPooled;
    #endregion

    #region Singleton
    /// <summary>
    /// Riferimento al Singleton
    /// </summary>
    public static PoolManager instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("Pool Settings")]
    /// <summary>
    /// Riferimento alla lista di oggetti da Poolare
    /// </summary>
    [SerializeField]
    private List<PoolObjects> poolObjects = new List<PoolObjects>();

    /// <summary>
    /// Posizione della Pool
    /// </summary>
    private Vector3 poolPosition = new Vector3(1000, 1000, 1000);
    /// <summary>
    /// Dictionary che contiene tutti gli oggetti in Pool in base al tipo
    /// </summary>
    private Dictionary<ObjectTypes, List<IPoolObject>> poolDictionary;

    /// <summary>
    /// Funzione che di Setup
    /// </summary>
    public void Setup()
    {
        poolDictionary = new Dictionary<ObjectTypes, List<IPoolObject>>();
        foreach (PoolObjects obj in poolObjects)
        {
            List<IPoolObject> objectsToAdd = new List<IPoolObject>();
            for (int i = 0; i < obj.ammount; i++)
            {
                GameObject instantiateObject = Instantiate(obj.prefab, transform);
                IPoolObject instantiateObjectInterface = instantiateObject.GetComponent<IPoolObject>();
                if (instantiateObjectInterface != null)
                {
                    instantiateObjectInterface.OnObjectDestroy += OnObjectDestroy;
                    instantiateObjectInterface.OnObjectSpawn += OnObjectSpawn;
                    OnObjectDestroy(instantiateObjectInterface);
                    objectsToAdd.Add(instantiateObjectInterface);
                    instantiateObjectInterface.PoolInit();
                }
                else
                {
                    Debug.Log("il prefab: " + instantiateObject.ToString() + "     type:" + obj.objectType.ToString() + " non implementa l'interfaccia IPoolObject");
                    break;
                }
            }
            poolDictionary.Add(obj.objectType, objectsToAdd);
        }
    }

    /// <summary>
    /// Funzione che gestisce l'evento di distruzione di un oggetto per farlo tornare i Pool
    /// </summary>
    /// <param name="objectToDestroy"></param>
    private void OnObjectDestroy(IPoolObject objectToDestroy)
    {
        objectToDestroy.CurrentState = State.InPool;
        objectToDestroy.gameObject.transform.position = poolPosition;
        objectToDestroy.gameObject.transform.SetParent(transform);
        objectToDestroy.ownerObject = null;
    }

    /// <summary>
    /// Funzioe che gestisce l'evento di spawn di un oggetto per toglierlo dalla Pool
    /// </summary>
    /// <param name="objectToSpawn"></param>
    private void OnObjectSpawn(IPoolObject objectToSpawn)
    {
        objectToSpawn.CurrentState = State.InUse;
    }

    #region API
    /// <summary>
    /// Funzione che fa prendere un oggetto dalla Pool in base al tipo passato come parametro
    /// </summary>
    /// <param name="type"></param>
    /// <param name="_ownerObject"></param>
    /// <returns></returns>
    public GameObject GetPooledObject(ObjectTypes type, GameObject _ownerObject)
    {
        foreach (IPoolObject _object in poolDictionary[type])
        {
            if (_object.CurrentState == State.InPool)
            {
                _object.ownerObject = _ownerObject;
                OnObjectPooled?.Invoke(_object);
                return _object.gameObject;
            }
        }
        Debug.Log("Nessun " + type + " disponibile");
        return null;
    }
    #endregion

    private void OnDisable()
    {
        foreach (ObjectTypes type in poolDictionary.Keys)
        {
            foreach (IPoolObject pooledObject in poolDictionary[type])
            {
                pooledObject.OnObjectDestroy -= OnObjectDestroy;
                pooledObject.OnObjectSpawn -= OnObjectSpawn;
            }
        }
    }
}
