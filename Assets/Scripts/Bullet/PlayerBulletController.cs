using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il bullet del Player
/// </summary>
public class PlayerBulletController : BulletControllerBase
{
    [Header("Player Bullet Settings")]
    //Danno del proiettile
    [SerializeField]
    protected int bulletDamage;

    private void OnTriggerEnter(Collider other)
    {
        BossLifeController bossLifeCtrl = other.GetComponent<BossLifeController>();
        if (bossLifeCtrl != null)
        {
            bossLifeCtrl.TakeDamage(bulletDamage);
            BulletDestroy();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            BulletDestroy();
    }
}
