﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce l'inizio della terza fase del Boss
/// </summary>
public class Boss1StartPhase3State : Boss1StateBase
{
    [Header("Phase Settings")]
    //Se il boss può prendere danno diretto
    [SerializeField]
    private bool canTakeDirectDamage;

    /// <summary>
    /// Riferiemtno al Boss Controller
    /// </summary>
    private Boss1Controller bossCltr;
    /// <summary>
    /// Riferimento al Life Controller
    /// </summary>
    private BossLifeController lifeCtrl;

    public override void Enter()
    {
        bossCltr = context.GetBossController();
        lifeCtrl = bossCltr.GetBossLifeController();

        lifeCtrl.SetCanTakeDamage(canTakeDirectDamage);

        Debug.Log("Phase 3 Iniziata");
        Complete();
    }
}
