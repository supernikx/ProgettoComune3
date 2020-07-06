using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Classe che gestisce tutte le referenze del Boss 2
/// </summary>
public class Boss2Controller : BossControllerBase
{
	[Header("Debug")]
	public GameObject canvasDebug;
	public TextMeshProUGUI boss2TimerDebug;

	/// <summary>
	/// Riferimento alla StateMachine
	/// </summary>
	private Boss2SMController sm;
	/// <summary>
	/// Riferimento al LaserController
	/// </summary>
	private Boss2LaserController laserCtrl;
	/// <summary>
	/// Riferimento al CoverBlockController
	/// </summary>
	private Boss2CoverBlocksController coverBlockCtrl;
	/// <summary>
	/// Riferimento al phase controller
	/// </summary>
	private Boss2PhaseController phaseCtrl;
	/// <summary>
	/// Riferiemento al graphic controller
	/// </summary>
	private Boss2GraphicController graphicCtrl;
	/// <summary>
	/// Riferiemento al sound controller
	/// </summary>
	private SoundController soundCtrl;

	private void Start()
	{
		canvasDebug.SetActive(false);
	}

	/// <summary>
	/// Funzione di Setup
	/// </summary>
	/// <param name="_lvlMng"></param>
	public override void Setup(LevelManager _lvlMng)
	{
		base.Setup(_lvlMng);

		sm = GetComponent<Boss2SMController>();
		laserCtrl = GetComponent<Boss2LaserController>();
		coverBlockCtrl = GetComponent<Boss2CoverBlocksController>();
		phaseCtrl = GetComponent<Boss2PhaseController>();
		soundCtrl = GetComponent<SoundController>();
		graphicCtrl = GetComponentInChildren<Boss2GraphicController>();

		int bossDefeated = UserData.GetBossDefeated();
		if (bossDefeated > 0 && bossDefeated < 3 && bossDefeated != 1)
			gameObject.SetActive(false);
		else
			coverBlockCtrl.Setup();
	}

	#region API
	/// <summary>
	/// Funzione che attiva il boss
	/// </summary>
	public override void StartBoss()
	{
		base.StartBoss();
		Boss2SMController.Context context = new Boss2SMController.Context(this, sm, lvlMng);
		sm.Setup(context);
		laserCtrl.Setup(this);
		lifeCtrl.Setup(this);
		collisionCtrl.Setup(this);
		phaseCtrl.Setup(this);
		graphicCtrl.Setup(this);
	}

	/// <summary>
	/// Funzione che ferma il Boss
	/// </summary>
	public override void StopBoss()
	{
		sm.GoToState("Empty");
		base.StopBoss();
	}

	/// <summary>
	/// Funzione che uccide il Boss
	/// </summary>
	public void KillBoss()
	{
		gameObject.SetActive(false);
		OnBossDead?.Invoke(this);
	}

	#region Getter
	/// <summary>
	/// Funzione che ritorna il LaserController
	/// </summary>
	/// <returns></returns>
	public Boss2LaserController GetLaserController()
	{
		return laserCtrl;
	}

	/// <summary>
	/// Funzione che ritorna il CoverBlockController
	/// </summary>
	/// <returns></returns>
	public Boss2CoverBlocksController GetCoverBlocksController()
	{
		return coverBlockCtrl;
	}

	/// <summary>
	/// Funzione che ritorna il phase controlelr
	/// </summary>
	/// <returns></returns>
	public Boss2PhaseController GetPhaseController()
	{
		return phaseCtrl;
	}

	/// <summary>
	/// Funzione che ritorna il sound controlelr
	/// </summary>
	/// <returns></returns>
	public SoundController GetSoundController()
	{
		return soundCtrl;
	}

	/// <summary>
	/// Funzione che ritorna il graphic controller
	/// </summary>
	/// <returns></returns>
	public Boss2GraphicController GetGraphicController()
	{
		return graphicCtrl;
	}
	#endregion
	#endregion
}
