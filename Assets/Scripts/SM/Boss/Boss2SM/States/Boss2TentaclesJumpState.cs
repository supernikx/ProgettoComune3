﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CLasse che gestisce lo stato di salto dei tentacoli
/// </summary>
public class Boss2TentaclesJumpState : Boss2StateBase
{
    [Header("Jump Settings")]
    //Zone in cui verrà eseguito il salto
    [SerializeField]
    private List<int> jumpZones;
    //Forza del salto
    [SerializeField]
    private float jumpForce;
    //Durata del salto
    [SerializeField]
    private float jumpTime;

    /// <summary>
    /// Riferimento al GroupController
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferiemtno al Boss Controller
    /// </summary>
    private Boss2Controller bossCtrl;
    /// <summary>
    /// Riferimento al Tentacles Controller
    /// </summary>
    private Boss2TentaclesController tentaclesCtrl;
    /// <summary>
    /// Riferimento al Collision Controller
    /// </summary>
    private BossCollisionController collisionCtrl;
    /// <summary>
    /// Riferimento al Life Controller
    /// </summary>
    private BossLifeController lifeCtrl;
    /// <summary>
    /// Riferimento al phase controller
    /// </summary>
    private Boss2PhaseController phaseCtrl;

    public override void Enter()
    {
        groupCtrl = context.GetLevelManager().GetGroupController();
        bossCtrl = context.GetBossController();
        tentaclesCtrl = bossCtrl.GetTentaclesController();
        collisionCtrl = bossCtrl.GetBossCollisionController();
        lifeCtrl = bossCtrl.GetBossLifeController();
        phaseCtrl = bossCtrl.GetPhaseController();

        tentaclesCtrl.OnTentacleDead += HandleOnTentacleDead;
        tentaclesCtrl.OnAllTentaclesDead += HandleOnAllTentaclesDead;
        collisionCtrl.OnAgentHit += HandleOnAgentHit;
        lifeCtrl.OnBossDead += HandleOnBossDead;
        phaseCtrl.OnSecondPhaseStart += HandleOnSecondPhaseStart;

        TentaclesJump();
    }

    private void TentaclesJump()
    {
        for (int i = 0; i < jumpZones.Count; i++)
        {
            //HACK: Così i designer possono partire a contare da 1
            int jumpZone = jumpZones[i] - 1;
            tentaclesCtrl.Jump(jumpZone, jumpForce, jumpTime);
        }

        Complete();
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
    /// Funzione che gestisce l'evento di hit di un agent
    /// </summary>
    /// <param name="_agent"></param>
    private void HandleOnAgentHit(AgentController _agent)
    {
        groupCtrl.RemoveAgent(_agent);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di morte di un tentacolo
    /// </summary>
    private void HandleOnTentacleDead(int _damage)
    {
        bool canTakeDamage = lifeCtrl.GetCanTakeDamage();
        lifeCtrl.SetCanTakeDamage(true);
        lifeCtrl.TakeDamage(_damage);
        lifeCtrl.SetCanTakeDamage(canTakeDamage);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di morte dei tantacoli
    /// </summary>
    private void HandleOnAllTentaclesDead()
    {
        Complete(3);
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
        if (tentaclesCtrl != null)
        {
            tentaclesCtrl.OnTentacleDead -= HandleOnTentacleDead;
            tentaclesCtrl.OnAllTentaclesDead -= HandleOnAllTentaclesDead;
        }

        if (collisionCtrl != null)
            collisionCtrl.OnAgentHit -= HandleOnAgentHit;

        if (lifeCtrl != null)
            lifeCtrl.OnBossDead -= HandleOnBossDead;

        if (phaseCtrl != null)
            phaseCtrl.OnSecondPhaseStart -= HandleOnSecondPhaseStart;
    }
}
