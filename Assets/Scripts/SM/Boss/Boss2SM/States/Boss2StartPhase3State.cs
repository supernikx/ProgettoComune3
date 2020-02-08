using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce l'inizio della seconda fase del Boss 3
/// </summary>
public class Boss2StartPhase3State : Boss2StateBase
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
    /// Riferiemtno al Boss Tourrets Controller
    /// </summary>
    private Boss2TourretsController bossTourretsCtrl;

    public override void Enter()
    {
        bossCtrl = context.GetBossController();
        bossTourretsCtrl = bossCtrl.GetTourretsController();
        lifeCtrl = bossCtrl.GetBossLifeController();

        lifeCtrl.SetCanTakeDamage(canTakeDirectDamage);
        bossTourretsCtrl.SetCanAim(false);

        Debug.Log("Phase 2 Iniziata");
        Complete();
    }
}
