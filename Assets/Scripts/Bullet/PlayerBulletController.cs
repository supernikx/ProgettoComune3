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
    protected override void BulletDestroy()
    {
        SpawnOrb();
        base.BulletDestroy();
    }

    /// <summary>
    /// Funzione che spawna gli orb
    /// </summary>
    private void SpawnOrb() 
    {
        OrbController pooledOrb = PoolManager.instance.GetPooledObject(ObjectTypes.PlayerOrb, ownerObject).GetComponent<OrbController>();
        if (pooledOrb != null)
        {
            pooledOrb.transform.position = transform.position;
            pooledOrb.Setup();
        }
    }
}
