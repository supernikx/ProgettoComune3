using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Funzione che gestisce gli orb del gruppo
/// </summary>
public class GroupOrbController : MonoBehaviour
{
	[Header("Orbs Settings")]
	//Velocità minima degli orb
	[SerializeField]
	private float orbMinDistance;
	//Distanza minima degli orb
	[SerializeField]
	private float orbMinSpeed;
	//Distanza massima degli orb
	[SerializeField]
	private float orbMaxDistance;
	//Velocità masssima degli orb
	[SerializeField]
	private float orbMaxSpeed;

	/// <summary>
	/// Riferimento al group controller
	/// </summary>
	private GroupController groupCtrl;
	/// <summary>
	/// Lista che identifica gli orb in scena
	/// </summary>
	private List<OrbController> droppedOrbs = new List<OrbController>();

	/// <summary>
	/// Funzione di Setup
	/// </summary>
	public void Setup(GroupController _groupCtrl)
	{
		groupCtrl = _groupCtrl;
		OrbController.OnOrbSpawn += HandleOnOrbSpawn;
		OrbController.OnOrbDestroy += HandleOnOrbDestroy;
		groupCtrl.OnGroupDead += HandleOnGroupDead;

		droppedOrbs.Clear();
	}

	#region Handlers
	/// <summary>
	/// Funzione che gestisce l'evento di spawn di un Orb
	/// </summary>
	/// <param name="_dropppedOrb"></param>
	private void HandleOnOrbSpawn(OrbController _dropppedOrb)
	{
		droppedOrbs.Add(_dropppedOrb);
	}

	/// <summary>
	/// Funzione che gestisce l'evento di destroy di un Orb
	/// </summary>
	/// <param name="_destroyedOrb"></param>
	private void HandleOnOrbDestroy(OrbController _destroyedOrb)
	{
		droppedOrbs.Remove(_destroyedOrb);
	}

	/// <summary>
	/// Funzione che gestisce l'evento di morte del gruppo
	/// </summary>
	private void HandleOnGroupDead()
	{
		droppedOrbs.Clear();
	}
	#endregion

	#region API
	/// <summary>
	/// Funzione che muove gli orb verso una posizone
	/// </summary>
	/// <param name="_targetPosition"></param>
	public void MoveOrbs(Vector3 _targetPosition)
	{
		Vector3 orbPos = Vector3.zero;
		Vector3 orbDirection = Vector3.zero;
		float orbDisatnce = 0f;
		float orbSpeed = 0f;

		for (int i = 0; i < droppedOrbs.Count; i++)
		{
			orbDisatnce = Vector3.Distance(_targetPosition, orbPos);

			if (orbDisatnce > orbMaxDistance)
				orbSpeed = orbMaxSpeed;
			else if (orbDisatnce < orbMinDistance)
				orbSpeed = orbMinSpeed;
			else
			{
				float disatnceOffset = (orbDisatnce - orbMinDistance) / (orbMaxDistance - orbMinDistance);
				float speedOffset = orbMaxDistance - orbMinSpeed;
				orbSpeed = (disatnceOffset * speedOffset) + orbMinSpeed;
			}

			orbPos = droppedOrbs[i].transform.position;
			orbDirection = (_targetPosition - orbPos).normalized;

			droppedOrbs[i].transform.Translate(orbDirection * orbSpeed * Time.deltaTime, Space.World);
		}
	}

	/// <summary>
	/// Funzione che ritorna se si può ricaricare
	/// </summary>
	/// <returns></returns>
	public bool CanReload()
	{
		return droppedOrbs.Count > 0;
	}

	/// <summary>
	/// Funzione che istanza un Orb
	/// </summary>
	/// <param name="_spawnPosition"></param>
	public void InstantiatedOrb(Vector3 _spawnPosition)
	{
		if (droppedOrbs.Count < groupCtrl.GetGroupMaxAgentCont())
		{
			GameObject pooledObj = PoolManager.instance.GetPooledObject(ObjectTypes.PlayerOrb, gameObject);
			if (pooledObj == null)
				return;

			OrbController pooledOrb = pooledObj.GetComponent<OrbController>();
			if (pooledOrb != null)
			{
				pooledOrb.transform.position = _spawnPosition;
				pooledOrb.Setup();
			}
		}
	}
	#endregion

	private void OnDestroy()
	{
		OrbController.OnOrbSpawn -= HandleOnOrbSpawn;
		OrbController.OnOrbDestroy -= HandleOnOrbDestroy;

		if (groupCtrl != null)
			groupCtrl.OnGroupDead -= HandleOnGroupDead;
	}
}
