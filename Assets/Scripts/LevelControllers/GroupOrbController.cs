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
	//Velocità degli orb
	[SerializeField]
	private float orbSpeed;

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
		for (int i = 0; i < droppedOrbs.Count; i++)
		{
			Vector3 direction = (_targetPosition - droppedOrbs[i].transform.position).normalized;
			droppedOrbs[i].transform.Translate(direction * orbSpeed * Time.deltaTime, Space.World);
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
	#endregion

	private void OnDestroy()
	{
		OrbController.OnOrbSpawn -= HandleOnOrbSpawn;
		OrbController.OnOrbDestroy -= HandleOnOrbDestroy;

		if (groupCtrl != null)
			groupCtrl.OnGroupDead -= HandleOnGroupDead;
	}
}
