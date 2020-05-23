using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	/// <summary>
	/// Riferimento al GroupController
	/// </summary>
	private GroupController groupCtrl;
	/// <summary>
	/// Riferiemento al boss controller
	/// </summary>
	private Boss2Controller bossCtrl;
	/// <summary>
	/// Riferimento al Blocks Controller
	/// </summary>
	private Boss2CoverBlocksController coverBlockCtrl;
	/// <summary>
	/// Riferimento al Collision Controller
	/// </summary>
	private BossCollisionController collisionCtrl;
	/// <summary>
	/// Riferiemento al coverblock da disabilitare
	/// </summary>
	private CoverBlockController coverBlockToDisable;
	/// <summary>
	/// Timer che conta il tempo di attacco
	/// </summary>
	private float attackTimer;
	/// <summary>
	/// Riferimento alla coroutine dell'attacco
	/// </summary>
	private IEnumerator attackRoutine;

	public override void Enter()
	{
		groupCtrl = context.GetLevelManager().GetGroupController();
		bossCtrl = context.GetBossController();
		coverBlockCtrl = bossCtrl.GetCoverBlocksController();
		collisionCtrl = bossCtrl.GetBossCollisionController();
		collisionCtrl.OnAgentHit += HandleOnAgentHit;

		attackTimer = 0;

		attackRoutine = AttackCoroutine();
		bossCtrl.StartCoroutine(attackRoutine);
	}

	#region Handlers
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
		//bossCtrl.ChangeColor(new Color(255, 127, 80));
		bossCtrl.ChangeColor(Color.yellow);
		yield return new WaitForSeconds(chargeTime);
		bossCtrl.ChangeColor(Color.red);
		float waitTime = attackTimer / 10;
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

		bossCtrl.ChangeColor(Color.white);

		if (coverBlockToDisable != null)
			coverBlockToDisable.Enable(false);

		Complete();
	}

	public override void Exit()
	{
		if (collisionCtrl != null)
			collisionCtrl.OnAgentHit -= HandleOnAgentHit;

		if (attackRoutine != null && bossCtrl != null)
			bossCtrl.StopCoroutine(attackRoutine);
	}
}
