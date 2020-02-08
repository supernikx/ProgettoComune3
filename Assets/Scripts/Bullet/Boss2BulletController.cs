using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il bullet del Boss1
/// </summary>
public class Boss2BulletController : BulletControllerBase
{
    /// <summary>
    /// Riferimento al BossCollisionController
    /// </summary>
    private BossCollisionController bossCollisionCtrl;

    /// <summary>
    /// Override della funzione di Setup del BulletControllerBase
    /// </summary>
    public override void Setup()
    {
        if (ownerObject != null)
        {
            Boss2Controller bossCtrl = ownerObject.GetComponent<Boss2Controller>();
            if (bossCtrl != null)
            {
                bossCollisionCtrl = bossCtrl.GetBossCollisionController();
            }
        }

        base.Setup();
    }

    private void OnTriggerEnter(Collider other)
    {
        AgentController agent = other.GetComponent<AgentController>();
        if (agent != null)
        {
            if (bossCollisionCtrl != null)
                bossCollisionCtrl.OnAgentHit?.Invoke(agent);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            BulletDestroy();
    }
}
