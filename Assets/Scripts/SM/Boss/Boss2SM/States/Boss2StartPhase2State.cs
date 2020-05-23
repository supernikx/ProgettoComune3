using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce l'inizio della seconda fase del Boss 2
/// </summary>
public class Boss2StartPhase2State : Boss2StateBase
{
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
        bossCtrl = context.GetBossController();
        lifeCtrl = bossCtrl.GetBossLifeController();        

        lifeCtrl.SetCanTakeDamage(canTakeDirectDamage);
        bossCtrl.ChangeColor(Color.white);

        Debug.Log("Phase 2 Iniziata");
        Complete();
    }
}
