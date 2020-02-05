using System.Collections;
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
    /// Riferimento al Life Controller
    /// </summary>
    private BossLifeController lifeCtrl;
    /// <summary>
    /// Riferimento al CoverBlockController
    /// </summary>
    private Boss2CoverBlocksController coverBlockCtrl;

    public override void Enter()
    {
        bossCtrl = context.GetBossController();
        lifeCtrl = bossCtrl.GetBossLifeController();
        coverBlockCtrl = bossCtrl.GetCoverBlocksController(); 

        lifeCtrl.SetCanTakeDamage(canTakeDirectDamage);
        coverBlockCtrl.EnableCoverBlocks(true);

        Debug.Log("Phase 2 Iniziata");
        Complete();
    }
}
