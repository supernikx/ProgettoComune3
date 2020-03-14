using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di riscucchio del Boss 2
/// </summary>
public class Boss2SuckState : Boss2StateBase
{
    [Header("Suck Settings")]
    //Forza di risucchio
    [SerializeField]
    private float suckForce;
    //Forza di risucchio
    [SerializeField]
    private float suckDuration;

    /// <summary>
    /// Riferimento al GroupController
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al GroupMovementController
    /// </summary>
    private GroupMovementController groupMovementCtrl;
    /// <summary>
    /// Riferiemtno al Boss Controller
    /// </summary>
    private Boss2Controller bossCtrl;
    /// <summary>
    /// Riferimento al Life Controller
    /// </summary>
    private BossLifeController lifeCtrl;
    /// <summary>
    /// Riferimento al Collision Controller
    /// </summary>
    private BossCollisionController collisionCtrl;
    /// <summary>
    /// Riferimento al phase controller
    /// </summary>
    private Boss2PhaseController phaseCtrl;
    /// <summary>
    /// Timer che conta quanto tempo dura lo stato
    /// </summary>
    private float suckTimer;

    public override void Enter()
    {
        groupCtrl = context.GetLevelManager().GetGroupController();
        groupMovementCtrl = groupCtrl.GetGroupMovementController();
        bossCtrl = context.GetBossController();
        lifeCtrl = bossCtrl.GetBossLifeController();
        collisionCtrl = bossCtrl.GetBossCollisionController();
        phaseCtrl = bossCtrl.GetPhaseController();

        lifeCtrl.OnBossDead += HandleOnBossDead;
        collisionCtrl.OnAgentHit += HandleOnAgentHit;
        phaseCtrl.OnThirdPhaseStart += HandleOnThirdPhaseStart;

        suckTimer = suckDuration;
    }

    public override void Tick()
    {
        groupMovementCtrl.MoveAgentsToPointDirection(bossCtrl.transform.position, suckForce);

        suckTimer -= Time.deltaTime;
        if (suckTimer <= 0)
            Complete(3);
    }

    #region Handles
    /// <summary>
    /// Funzione che gestisce l'evento di inizio della seconda fase
    /// </summary>
    private void HandleOnThirdPhaseStart()
    {
        Complete(3);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di hit di un agent
    /// </summary>
    /// <param name="_agent"></param>
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
            phaseCtrl.OnThirdPhaseStart -= HandleOnThirdPhaseStart;
    }
}
