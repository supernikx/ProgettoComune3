using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di morte del Boss
/// </summary>
public class Boss1DeathState : Boss1StateBase
{
    /// <summary>
    /// Riferimento al boss controller
    /// </summary>
    private Boss1Controller bossCtrl;

    public override void Enter()
    {
        bossCtrl = context.GetBossController();
        bossCtrl.KillBoss();
    }
}
