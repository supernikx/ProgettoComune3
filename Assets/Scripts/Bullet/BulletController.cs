using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il bullet
/// </summary>
public class BulletController : MonoBehaviour
{
    [Header("Bullet Settings")]
    //Velocità del proiettile
    [SerializeField]
    private float bulletSpeed;
    //Danno del proiettile
    [SerializeField]
    private int bulletDamage;

    /// <summary>
    /// Punto di spawn del proiettile
    /// </summary>
    private Vector3 spawnPosition;
    /// <summary>
    /// bool che idetifica se il bullet è setuppato o no
    /// </summary>
    private bool isSetupped = false;

    /// <summary>
    /// Funzione che esegue il setup
    /// </summary>
    public void Setup()
    {
        spawnPosition = transform.position;
        isSetupped = true;
    }

    private void FixedUpdate()
    {
        if (!isSetupped)
            return;

        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, bulletSpeed);

        if (Vector3.Distance(transform.position, spawnPosition) > 100)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        BossLifeController bossLifeCtrl = other.GetComponent<BossLifeController>();
        if (bossLifeCtrl != null)
        {
            bossLifeCtrl.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
}
