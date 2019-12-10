﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce l'inizio della seconda fase del Boss 2
/// </summary>
public class Boss2StartPhase2State : Boss2StateBase
{
    [Header("Phase Settings")]
    //Se il boss può prendere danno diretto
    [SerializeField]
    private bool canTakeDirectDamage;

    /// <summary>
    /// Riferiemtno al Boss Controller
    /// </summary>
    private Boss2Controller bossCtrl;
    /// <summary>
    /// Riferiemtno al Boss Tentacle Controller
    /// </summary>
    private Boss2TentaclesController bossTentacleCtrl;
    /// <summary>
    /// Riferimento al Life Controller
    /// </summary>
    private BossLifeController lifeCtrl;

    public override void Enter()
    {
        bossCtrl = context.GetBossController();
        bossTentacleCtrl = bossCtrl.GetTentaclesController();
        lifeCtrl = bossCtrl.GetBossLifeController();

        lifeCtrl.SetCanTakeDamage(canTakeDirectDamage);
        bossTentacleCtrl.Phase2TentaclesSetup();

        Debug.Log("Phase 2 Iniziata");
        Complete();
    }
}
