using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di sparo del Boss
/// </summary>
public class Boss1ShootState : Boss1StateBase
{

    public override void Enter()
    {
        Boss1ShootController shootCtrl = context.GetBossController().GetBossShootController();
        shootCtrl.Shoot();
        Complete();
    }
}
