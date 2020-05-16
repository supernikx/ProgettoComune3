using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di vulnerabile del boss 2
/// </summary>
public class Boss2VulnerableState : Boss2StateBase
{
	/// <summary>
	/// Riferiemtno al Boss Controller
	/// </summary>
	private Boss2Controller bossCtrl;
	/// <summary>
	/// Riferimento al Life Controller
	/// </summary>
	private BossLifeController lifeCtrl;
	/// <summary>
	/// Riferimento al Phase Controller
	/// </summary>
	private Boss2PhaseController phaseCtrl;

	public override void Enter()
	{
		bossCtrl = context.GetBossController();
		lifeCtrl = bossCtrl.GetBossLifeController();
		phaseCtrl = bossCtrl.GetPhaseController();

		phaseCtrl.OnSecondPhaseStart += HandleOnSecondPhaseStart;
		phaseCtrl.OnThirdPhaseStart += HandleOnThirdPhaseStart;
		phaseCtrl.OnFourthPhaseStart += HandleOnFourthPhaseStart;

		lifeCtrl.SetCanTakeDamage(true);
	}

	#region Handlers
	/// <summary>
	/// Funzione che gestisce l'evento di inizio della seconda fase
	/// </summary>
	private void HandleOnSecondPhaseStart()
	{
		Complete(2);
	}

	/// <summary>
	/// Funzione che gestisce l'evento di inizio della terza fase
	/// </summary>
	private void HandleOnThirdPhaseStart()
	{
		Complete(3);
	}

	/// <summary>
	/// Funzione che gestisce l'evento di inizio della quarta fase
	/// </summary>
	private void HandleOnFourthPhaseStart()
	{
		Complete(4);
	}

	#endregion

	public override void Exit()
	{
		if (phaseCtrl != null)
		{
			phaseCtrl.OnSecondPhaseStart -= HandleOnSecondPhaseStart;
			phaseCtrl.OnThirdPhaseStart -= HandleOnThirdPhaseStart;
			phaseCtrl.OnFourthPhaseStart -= HandleOnFourthPhaseStart;
		}
	}
}
