using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Classe che gestisce lo stato di super attack del Boss 2
/// </summary>
public class Boss2SuperAttackState : Boss2StateBase
{
	[Header("State Settings")]
	[SerializeField]
	private float chargeTime;
	[SerializeField]
	private float attackDuration;


	[Header("Feedback")]
	//suono di charge del boss
	[SerializeField]
	private string attackChargeSoundID = "attackCharge";
	//suono di attack del boss
	[SerializeField]
	private string attackSoundID = "attack";

	/// <summary>
	/// Riferimento al GroupController
	/// </summary>
	private GroupController groupCtrl;
	/// <summary>
	/// Riferiemento al boss controller
	/// </summary>
	private Boss2Controller bossCtrl;
	/// <summary>
	/// Riferiemento al graphic controller
	/// </summary>
	private Boss2GraphicController graphicCtrl;
	/// <summary>
	/// Riferimento al LifeController
	/// </summary>
	private BossLifeController lifeCtrl;
	/// <summary>
	/// Riferimento al Blocks Controller
	/// </summary>
	private Boss2CoverBlocksController coverBlockCtrl;
	/// <summary>
	/// Riferimento al Collision Controller
	/// </summary>
	private BossCollisionController collisionCtrl;
	/// <summary>
	/// Riferimento al phase controller
	/// </summary>
	private Boss2PhaseController phaseCtrl;
	/// <summary>
	/// Riferimento al sound controller
	/// </summary>
	private SoundController soundCtrl;
	/// <summary>
	/// Riferiemento al coverblock da disabilitare
	/// </summary>
	private CoverBlockController coverBlockToDisable;
	/// <summary>
	/// Int che identifica la next phase
	/// </summary>
	private int nextPhase;
	/// <summary>
	/// Riferimento alla coroutine dell'attacco
	/// </summary>
	private IEnumerator attackRoutine;

	public override void Enter()
	{
		groupCtrl = context.GetLevelManager().GetGroupController();
		bossCtrl = context.GetBossController();
		graphicCtrl = bossCtrl.GetGraphicController();
		lifeCtrl = bossCtrl.GetBossLifeController();
		coverBlockCtrl = bossCtrl.GetCoverBlocksController();
		collisionCtrl = bossCtrl.GetBossCollisionController();
		phaseCtrl = bossCtrl.GetPhaseController();
		soundCtrl = bossCtrl.GetSoundController();

		lifeCtrl.SetCanTakeDamage(canTakeDirectDamage);

		lifeCtrl.OnBossDead += HandleOnBossDead;
		collisionCtrl.OnAgentHit += HandleOnAgentHit;
		phaseCtrl.OnSecondPhaseStart += HandleOnSecondPhaseStart;
		phaseCtrl.OnThirdPhaseStart += HandleOnThirdPhaseStart;
		phaseCtrl.OnFourthPhaseStart += HandleOnFourthPhaseStart;

		nextPhase = -1;

		attackRoutine = AttackCoroutine();
		bossCtrl.StartCoroutine(attackRoutine);
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
	}

	/// <summary>
	/// Funzione che gestisce l'evento di inizio della seconda fase
	/// </summary>
	private void HandleOnThirdPhaseStart()
	{
		nextPhase = 3;
		lifeCtrl.SetCanTakeDamage(false);
	}

	/// <summary>
	/// Funzione che gestisce l'evento di inizio della seconda fase
	/// </summary>
	private void HandleOnFourthPhaseStart()
	{
		nextPhase = 4;
		lifeCtrl.SetCanTakeDamage(false);
	}
	#endregion

	/// <summary>
	/// Funzione che gestisce l'evento di morte del Boss
	/// </summary>
	private void HandleOnBossDead()
	{
		Complete(1);
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

	/// <summary>
	/// Coroutine che esgue l'attacco
	/// </summary>
	/// <returns></returns>
	private IEnumerator AttackCoroutine()
	{
		soundCtrl.PlayClipLoop(attackChargeSoundID);
		bossCtrl.canvasDebug.SetActive(true);
		Color startColor = bossCtrl.GetComponentInChildren<MeshRenderer>().material.GetColor("Color_A9F326B");
		graphicCtrl.ChangeColor(Color.yellow);
		float timer = 0f;
		while (timer < chargeTime)
		{
			bossCtrl.boss2TimerDebug.text = (chargeTime - timer).ToString("0.00");
			timer += Time.deltaTime;
			yield return null;
		}
		soundCtrl.StopClipLoop(attackChargeSoundID);
		soundCtrl.PlayClipLoop(attackSoundID);
		bossCtrl.canvasDebug.SetActive(false);
		graphicCtrl.SuperAttackVFX(true);
		graphicCtrl.ChangeColor(Color.red);
		float waitTime = attackDuration / 10f;
		for (int k = 0; k < 10; k++)
		{
			List<AgentController> coverAgents = new List<AgentController>();
			List<CoverBlockController> coverBlocks = coverBlockCtrl.GetCoverBlocks();
			int mostAgents = 0;

			for (int i = 0; i < coverBlocks.Count; i++)
			{
				List<AgentController> coverBlockAgents = coverBlocks[i].GetAgentList();
				if (coverBlockAgents.Count >= mostAgents)
				{
					mostAgents = coverBlockAgents.Count;
					coverBlockToDisable = coverBlocks[i];
				}

				if (!coverBlocks[i].IsCovering())
					continue;

				for (int j = 0; j < coverBlockAgents.Count; j++)
				{
					if (!coverAgents.Contains(coverBlockAgents[j]))
						coverAgents.Add(coverBlockAgents[j]);
				}
			}

			List<AgentController> allAgents = groupCtrl.GetAgents();
			for (int i = 0; i < allAgents.Count; i++)
			{
				if (!coverAgents.Contains(allAgents[i]))
				{
					groupCtrl.RemoveAgent(allAgents[i], true);
				}
			}

			yield return new WaitForSeconds(waitTime);
		}

		soundCtrl.StopClipLoop(attackSoundID);
		graphicCtrl.SuperAttackVFX(false);
		graphicCtrl.ResetColor();

		if (coverBlockToDisable != null)
			coverBlockToDisable.Enable(false);

		if (nextPhase != -1)
			Complete(nextPhase);
		else
			Complete();
	}

	public override void Exit()
	{
		if (lifeCtrl != null)
			lifeCtrl.OnBossDead -= HandleOnBossDead;

		if (collisionCtrl != null)
			collisionCtrl.OnAgentHit -= HandleOnAgentHit;

		if (attackRoutine != null && bossCtrl != null)
			bossCtrl.StopCoroutine(attackRoutine);

		if (phaseCtrl != null)
		{
			phaseCtrl.OnSecondPhaseStart -= HandleOnSecondPhaseStart;
			phaseCtrl.OnThirdPhaseStart -= HandleOnThirdPhaseStart;
			phaseCtrl.OnFourthPhaseStart -= HandleOnFourthPhaseStart;
		}
	}
}
