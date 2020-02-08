using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce l'inizio della prima fase del Boss 2
/// </summary>
public class Boss2StartPhase1State : Boss2StateBase
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
    /// Riferimento al CoverBlockController
    /// </summary>
    private Boss2CoverBlocksController coverBlockCtrl;
    /// <summary>
    /// Riferiemtno al Boss Tourrets Controller
    /// </summary>
    private Boss2TourretsController bossTourretsCtrl;
    /// <summary>
    /// Riferimento al Life Controller
    /// </summary>
    private BossLifeController lifeCtrl;

    public override void Enter()
    {
        bossCtrl = context.GetBossController();
        bossTourretsCtrl = bossCtrl.GetTourretsController();
        coverBlockCtrl = bossCtrl.GetCoverBlocksController();
        lifeCtrl = bossCtrl.GetBossLifeController();

        lifeCtrl.SetCanTakeDamage(canTakeDirectDamage);
        coverBlockCtrl.EnableCoverBlocks(true);
        bossTourretsCtrl.TourretsSetup();
        bossTourretsCtrl.SetCanAim(true);

        Debug.Log("Phase 1 Iniziata");
        Complete();
    }
}
