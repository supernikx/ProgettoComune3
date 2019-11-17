using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di morte del boss
/// </summary>
public class BossPrototipoDeathState : BossPrototipoStateBase
{
    /// <summary>
    /// Riferimento al boss controller
    /// </summary>
    private BossPrototipoController bossCtrl;

    public override void Enter()
    {
        bossCtrl = context.GetBossController();
        bossCtrl.KillBoss();
    }
}
