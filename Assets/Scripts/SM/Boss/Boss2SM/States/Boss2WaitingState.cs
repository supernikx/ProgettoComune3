using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di attesa del Boss 2
/// </summary>
public class Boss2WaitingState : Boss2StateBase
{
	[Header("State Settings")]
	//Range di tempo che il boss deve aspettare
	[SerializeField]
	private Vector2 waitTimeRange;

	/// <summary>
	/// Riferimento al GroupController
	/// </summary>
	private GroupController groupCtrl;
	/// <summary>
	/// Riferimento al BossController
	/// </summary>
	private Boss2Controller bossCtrl;
	/// <summary>
	/// Riferimento al BossCollisionController
	/// </summary>
	private BossCollisionController collisionCtrl;
	/// <summary>
	/// Riferimento al LifeController
	/// </summary>
	private BossLifeController lifeCtrl;
	/// <summary>
	/// Riferimento al phase controller
	/// </summary>
	private Boss2PhaseController phaseCtrl;
	/// <summary>
	/// Tempo che il boss deve aspettare
	/// </summary>
	private float waitTime;
	/// <summary>
	/// Timer che conta il tempo passato
	/// </summary>
	private float timer;
	/// <summary>
	/// Int che identifica la next phase
	/// </summary>
	private int nextPhase;

	public override void Enter()
	{
		groupCtrl = context.GetLevelManager().GetGroupController();
		bossCtrl = context.GetBossController();
		lifeCtrl = bossCtrl.GetBossLifeController();
		collisionCtrl = bossCtrl.GetBossCollisionController();
		phaseCtrl = bossCtrl.GetPhaseController();

		nextPhase = -1;
		timer = 0;
		waitTime = Random.Range(waitTimeRange.x, waitTimeRange.y);
		lifeCtrl.SetCanTakeDamage(canTakeDirectDamage);

		lifeCtrl.OnBossDead += HandleOnBossDead;
		collisionCtrl.OnAgentHit += HandleOnAgentHit;
		phaseCtrl.OnSecondPhaseStart += HandleOnSecondPhaseStart;
		phaseCtrl.OnThirdPhaseStart += HandleOnThirdPhaseStart;
		phaseCtrl.OnFourthPhaseStart += HandleOnFourthPhaseStart;
	}

	public override void Tick()
	{
		timer += Time.deltaTime;
		if (timer >= waitTime)
		{
			if (nextPhase != -1)
				Complete(nextPhase);
			else
				Complete();
		}
	}

	#region Handlers
	#region Phase
	/// <summary>
	/// Funzione che gestisce l'evento di inizio della seconda fase
	/// </summary>
	private void HandleOnSecondPhaseStart()
	{
		nextPhase = 2;
		lifeCtrl.SetCanTakeDamage(false);
		bossCtrl.ChangeColor(Color.cyan);
	}

	/// <summary>
	/// Funzione che gestisce l'evento di inizio della seconda fase
	/// </summary>
	private void HandleOnThirdPhaseStart()
	{
		nextPhase = 3;
		lifeCtrl.SetCanTakeDamage(false);
		bossCtrl.ChangeColor(Color.cyan);
	}

	/// <summary>
	/// Funzione che gestisce l'evento di inizio della seconda fase
	/// </summary>
	private void HandleOnFourthPhaseStart()
	{
		nextPhase = 4;
		lifeCtrl.SetCanTakeDamage(false);
		bossCtrl.ChangeColor(Color.cyan);
	}
	#endregion

	/// <summary>
	/// Funzione che gestisce l'evento collisionCtrl.OnAgentHit
	/// <param name="obj"></param>
	private void HandleOnAgentHit(AgentController _agent)
	{
		groupCtrl.RemoveAgent(_agent, true);
	}

	/// <summary>
	/// Funzione che gestisce l'evento di morte del Boss
	/// </summary>
	private void HandleOnBossDead()
	{
		Complete(1);
	}
	#endregion

	public override void Exit()
	{
		if (lifeCtrl != null)
			lifeCtrl.OnBossDead -= HandleOnBossDead;

		if (collisionCtrl != null)
			collisionCtrl.OnAgentHit -= HandleOnAgentHit;

		if (phaseCtrl != null)
		{
			phaseCtrl.OnSecondPhaseStart -= HandleOnSecondPhaseStart;
			phaseCtrl.OnThirdPhaseStart -= HandleOnThirdPhaseStart;
			phaseCtrl.OnFourthPhaseStart -= HandleOnFourthPhaseStart;
		}
	}
}
