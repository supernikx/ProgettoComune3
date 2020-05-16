using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di laser del Boss 2
/// </summary>
public class Boss2LaserState : Boss2StateBase
{
	[Header("First Laser Settings")]
	//Tempo di spawn del laser 1
	[SerializeField]
	private float laser1SpawnTime;
	//Velocità di moviemento del laser 1
	[SerializeField]
	private float laser1Speed;
	//Angolo di partenza del laser 1
	[SerializeField]
	private float laser1StartAngle;
	//Angolo di arrivo del laser 1
	[SerializeField]
	private float laser1EndAngle;

	[Header("Second Laser Settings (can be empty)")]
	//Se c'è il laser 2
	[SerializeField]
	private bool useSecondLaser;
	//Tempo di spawn del laser 2
	[SerializeField]
	private float laser2SpawnTime;
	//Velocità di moviemento del laser 2
	[SerializeField]
	private float laser2Speed;
	//Angolo di partenza del laser 2
	[SerializeField]
	private float laser2StartAngle;
	//Angolo di arrivo del laser 2
	[SerializeField]
	private float laser2EndAngle;

	[Header("Tracker Laser Settings (this override other settings)")]
	//Se il laser deve trackare il player
	[SerializeField]
	private bool trackPlayer;
	//Tempo di delay dello spawn del laser
	[SerializeField]
	private float trackLaserSpawnDelay;
	//Tempo di spawn del laser track
	[SerializeField]
	private float trackLaserSpawnTime;
	//Duarata del track laser
	[SerializeField]
	private float trackLaserDuration;
	//Velocità del track laser
	[SerializeField]
	private float trackLaserSpeed;

	/// <summary>
	/// Riferimento al GroupController
	/// </summary>
	private GroupController groupCtrl;
	/// <summary>
	/// Riferiemtno al Boss Controller
	/// </summary>
	private Boss2Controller bossCtrl;
	/// <summary>
	/// Riferimento al LaserController del Boss2
	/// </summary>
	private Boss2LaserController laserCtrl;
	/// <summary>
	/// Riferimento al Collision Controller
	/// </summary>
	private BossCollisionController collisionCtrl;
	/// <summary>
	/// Bool che identifica se bisogna aspettare altri laser
	/// </summary>
	private bool waitForOtherLaser;

	public override void Enter()
	{
		groupCtrl = context.GetLevelManager().GetGroupController();
		bossCtrl = context.GetBossController();
		laserCtrl = bossCtrl.GetLaserController();
		collisionCtrl = bossCtrl.GetBossCollisionController();

		collisionCtrl.OnAgentHit += HandleOnAgentHit;

		waitForOtherLaser = true;

		if (trackPlayer)
		{
			laserCtrl.SpawnLaser(trackLaserSpawnTime, trackLaserSpawnDelay, groupCtrl.GetGroupCenterPoint(), 1, HandleOnLaser1Spawn);
		}
		else
		{
			laserCtrl.SpawnLaser(laser1SpawnTime, laser1StartAngle, 1, HandleOnLaser1Spawn);

			if (useSecondLaser)
				laserCtrl.SpawnLaser(laser2SpawnTime, laser2StartAngle, 2, HandleOnLaser2Spawn);
		}
	}

	#region Handles
	/// <summary>
	/// Funzione che gestisce la callback di spawn del laser 1
	/// </summary>
	private void HandleOnLaser1Spawn()
	{
		if (trackPlayer)
		{
			laserCtrl.StartLaser(1, groupCtrl.GetGroupCenterTransform(), trackLaserDuration, trackLaserSpeed, HandleOnLaser1Rotate);
		}
		else
		{
			laserCtrl.StartLaser(1, laser1EndAngle, laser1Speed, HandleOnLaser1Rotate);
		}
	}

	/// <summary>
	/// Funzione che gestisce la callback di spawn del laser 2
	/// </summary>
	private void HandleOnLaser2Spawn()
	{
		if (useSecondLaser && !trackPlayer)
			laserCtrl.StartLaser(2, laser2EndAngle, laser2Speed, HandleOnLaser2Rotate);
	}

	/// <summary>
	/// Funzione che gestisce la callback di spawn del laser 1
	/// </summary>
	private void HandleOnLaser1Rotate()
	{
		if (!trackPlayer && waitForOtherLaser && useSecondLaser)
			waitForOtherLaser = false;
		else
			Complete();
	}

	/// <summary>
	/// Funzione che gestisce la callback di spawn del laser 2
	/// </summary>
	private void HandleOnLaser2Rotate()
	{
		if (!trackPlayer && waitForOtherLaser && useSecondLaser)
			waitForOtherLaser = false;
		else
			Complete();
	}

	/// <summary>
	/// Funzione che gestisce l'evento di hit di un agent
	/// </summary>
	/// <param name="_agent"></param>
	private void HandleOnAgentHit(AgentController _agent)
	{
		groupCtrl.RemoveAgent(_agent, true);
	}
	#endregion

	public override void Exit()
	{
		if (collisionCtrl != null)
			collisionCtrl.OnAgentHit -= HandleOnAgentHit;

		if (laserCtrl != null)
			laserCtrl.StopLaser(1);

		if (laserCtrl != null)
			laserCtrl.StopLaser(2);
	}
}
