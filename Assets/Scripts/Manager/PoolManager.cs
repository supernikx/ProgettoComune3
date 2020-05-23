using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enumeratore che identifica i tipi di oggetti presenti in Pool
/// </summary>
public enum ObjectTypes
{
	None,
	Agent,
	PlayerBullet,
	Boss1Bullet,
	PlayerBulletHitVFX,
	PlayerDeathVFX,
	PlayerOrb,
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

	public static PoolManagerEvent OnObjectSpawnEvent;
	public static PoolManagerEvent OnObjectDestroyEvent;
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
	/// Lista dei parent
	/// </summary>
	private List<Transform> parentList = new List<Transform>();

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
		OnObjectSpawnEvent += OnObjectSpawn;
		OnObjectDestroyEvent += OnObjectDestroy;

		poolDictionary = new Dictionary<ObjectTypes, List<IPoolObject>>();
		foreach (PoolObjects obj in poolObjects)
		{
			List<IPoolObject> objectsToAdd = new List<IPoolObject>();
			Transform newParent = new GameObject(obj.objectType.ToString()).transform;
			newParent.SetParent(transform);
			parentList.Add(newParent);

			for (int i = 0; i < obj.ammount; i++)
			{
				GameObject instantiateObject = Instantiate(obj.prefab, transform);
				IPoolObject instantiateObjectInterface = instantiateObject.GetComponent<IPoolObject>();
				if (instantiateObjectInterface != null)
				{
					instantiateObjectInterface.objectType = obj.objectType;
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
		objectToDestroy.currentState = State.InPool;
		objectToDestroy.gameObject.transform.position = poolPosition;
		objectToDestroy.gameObject.transform.SetParent(GetParentByOjectType(objectToDestroy.objectType));
		objectToDestroy.ownerObject = null;
	}

	/// <summary>
	/// Funzioe che gestisce l'evento di spawn di un oggetto per toglierlo dalla Pool
	/// </summary>
	/// <param name="objectToSpawn"></param>
	private void OnObjectSpawn(IPoolObject objectToSpawn)
	{
		objectToSpawn.currentState = State.InUse;
	}

	/// <summary>
	/// Funzione che ritorna il parent di una object type
	/// </summary>
	/// <param name="_objType"></param>
	/// <returns></returns>
	private Transform GetParentByOjectType(ObjectTypes _objType)
	{
		for (int i = 0; i < parentList.Count; i++)
		{
			if (parentList[i].name == _objType.ToString())
				return parentList[i];
		}

		return null;
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
			if (_object.currentState == State.InPool)
			{
				_object.ownerObject = _ownerObject;
				OnObjectPooled?.Invoke(_object);
				return _object.gameObject;
			}
		}

		Debug.Log("Nessun " + type + " disponibile");
		return null;
	}

	/// <summary>
	/// Funzione che fa tornare in Pool tutti gli oggetti del tipo passato come parametro
	/// </summary>
	/// <param name="type"></param>
	public void ResetPoolObjects(ObjectTypes type)
	{
		foreach (IPoolObject _object in poolDictionary[type])
		{
			if (_object.currentState == State.InUse)
			{
				_object.ResetPool();
				OnObjectDestroy(_object);
			}
		}
	}
	#endregion

	private void OnDisable()
	{
		OnObjectSpawnEvent -= OnObjectSpawn;
		OnObjectDestroyEvent -= OnObjectDestroy;
	}
}
