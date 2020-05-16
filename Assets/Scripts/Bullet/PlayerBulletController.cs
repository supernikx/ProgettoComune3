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

    private GroupController groupCtrl;
    private GroupOrbController groupOrbCtrl;

    public override void Setup()
    {
        base.Setup();
        groupCtrl = GameManager.instance.GetLevelManager().GetGroupController();
        groupOrbCtrl = groupCtrl.GetGroupOrbController();
    }

    private void OnTriggerEnter(Collider other)
    {
        IBossDamageable bossDamageable = other.GetComponentInParent<IBossDamageable>();
        if (bossDamageable != null)
        {
            bossDamageable.TakeDamage(bulletDamage);
            BulletDestroy();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle") || other.gameObject.layer == LayerMask.NameToLayer("ShootInteractable"))        
            BulletDestroy();       
    }

    /// <summary>
    /// Override funzione di destroy
    /// </summary>
    public override void BulletDestroy()
    {
        groupOrbCtrl.InstantiatedOrb(transform.position);
        base.BulletDestroy();
    }
}
