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

    public override void Enter()
    {
        groupCtrl = context.GetLevelManager().GetGroupController();
        groupMovementCtrl = groupCtrl.GetGroupMovementController();
        bossCtrl = context.GetBossController();
        lifeCtrl = bossCtrl.GetBossLifeController();

        lifeCtrl.OnBossDead += HandleOnBossDead;
    }

    public override void Tick()
    {
        groupMovementCtrl.MoveAgentsToPoint(bossCtrl.transform.position, suckForce);
    }

    #region Handles
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
    }
}
