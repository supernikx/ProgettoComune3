using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il laser del Boss2
/// </summary>
public class Boss2LaserController : MonoBehaviour
{
	[Header("Reference Laser Settings")]
	//Larghezza del laser
	[SerializeField]
	private float laserRadius;
	//Range massimo del laser
	[SerializeField]
	private float maxLaserRange;
	//Riferimento al laser controller
	[SerializeField]
	private LaserController laserCtrl1;
	//Riferimento al laser controller
	[SerializeField]
	private LaserController laserCtrl2;

	/// <summary>
	/// Riferimento al Boss Controller
	/// </summary>
	private Boss2Controller bossCtrl;
	/// <summary>
	/// Riferimento al collision Controller
	/// </summary>
	private BossCollisionController collisionCtrl;

	/// <summary>
	/// Funzione di Setup
	/// </summary>
	/// <param name="_bossCtrl"></param>
	public void Setup(Boss2Controller _bossCtrl)
	{
		bossCtrl = _bossCtrl;
		collisionCtrl = bossCtrl.GetBossCollisionController();
		laserCtrl1.Setup(maxLaserRange, laserRadius);
		laserCtrl2.Setup(maxLaserRange, laserRadius);
	}

	/// <summary>
	/// Funzione che ritorna il laser controller in base all'id
	/// </summary>
	/// <param name="_id"></param>
	/// <returns></returns>
	private LaserController GetLaserControllerByID(int _id)
	{
		if (_id == 1)
			return laserCtrl1;
		else
			return laserCtrl2;
	}

	#region Handlers
	/// <summary>
	/// Funzione che gestisce l'evento di agent colpito
	/// </summary>
	/// <param name="obj"></param>
	private void HandleOnAgentHit(AgentController _agent)
	{
		collisionCtrl.OnAgentHit?.Invoke(_agent);
	}
	#endregion

	#region API
	/// <summary>
	/// Funzione che spawna il laser
	/// </summary>
	/// <param name="_spawnTime"></param>
	/// <param name="_onSpawnCallback"></param>
	public void SpawnLaser(float _spawnTime, float _spawnAngle, int _laserID, Action _onSpawnCallback)
	{
		GetLaserControllerByID(_laserID).SpawnLaser(_spawnTime, _spawnAngle, _onSpawnCallback);
	}

	/// <summary>
	/// Funzione che spawna il laser
	/// </summary>
	/// <param name="_spawnTime"></param>
	/// <param name="_onSpawnCallback"></param>
	public void SpawnLaser(float _spawnTime, float _spawnDelay, Vector3 _targetPosition, int _laserID, Action _onSpawnCallback)
	{
		GetLaserControllerByID(_laserID).SpawnLaser(_spawnTime, _spawnDelay, _targetPosition, _onSpawnCallback);
	}

	/// <summary>
	/// Funzione che fa partire il Laser
	/// </summary>
	public void StartLaser(int _laserID, float _laserEndAngle, float _laserSpeed, Action _onRotateCallback)
	{
		GetLaserControllerByID(_laserID).OnAgentHit += HandleOnAgentHit;
		GetLaserControllerByID(_laserID).StartLaser(_laserEndAngle, _laserSpeed, _onRotateCallback);
	}

	/// <summary>
	/// Funzione che fa partire il Laser
	/// </summary>
	public void StartLaser(int _laserID, Transform _targetTransform, float _laserDuration, float _laserSpeed, Action _onRotateCallback)
	{
		GetLaserControllerByID(_laserID).OnAgentHit += HandleOnAgentHit;
		GetLaserControllerByID(_laserID).StartLaser(_targetTransform, _laserDuration, _laserSpeed, _onRotateCallback);
	}

	/// <summary>
	/// Funzione che stoppa il laser
	/// </summary>
	public void StopLaser(int _laserID)
	{
		GetLaserControllerByID(_laserID).OnAgentHit -= HandleOnAgentHit;
		GetLaserControllerByID(_laserID).StopLaser();
	}

	#region Getter
	/// <summary>
	/// Funzione che ritorna se il laser è attivo
	/// </summary>
	/// <returns></returns>
	public bool IsEnable(int _laserID)
	{
		return GetLaserControllerByID(_laserID).IsEnable();
	}
	#endregion
	#endregion

	private void OnDisable()
	{
		if (laserCtrl1 != null)
			laserCtrl1.OnAgentHit -= HandleOnAgentHit;

		if (laserCtrl2 != null)
			laserCtrl2.OnAgentHit -= HandleOnAgentHit;
	}
}
