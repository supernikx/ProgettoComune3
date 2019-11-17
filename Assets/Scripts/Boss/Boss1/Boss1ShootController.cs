using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo sparo del Boss
/// </summary>
public class Boss1ShootController : MonoBehaviour
{
    [Header("Shoot Settings")]
    [SerializeField]
    private List<Transform> shootPoints;

    /// <summary>
    /// Riferimento al BossController
    /// </summary>
    private Boss1Controller bossCtrl;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_bossCtrl"></param>
    public void Setup(Boss1Controller _bossCtrl)
    {
        bossCtrl = _bossCtrl;
    }

    #region API
    /// <summary>
    /// Funzione che spara un proiettile per ogni shoot point
    /// </summary>
    public void Shoot()
    {
        if (bossCtrl.IsSetuppedAndEnabled())
        {
            foreach (Transform point in shootPoints)
            {
                Boss1BulletController newBullet = PoolManager.instance.GetPooledObject(ObjectTypes.Boss1Bullet, gameObject).GetComponent<Boss1BulletController>();
                if (newBullet != null)
                {
                    newBullet.transform.SetPositionAndRotation(point.position, Quaternion.LookRotation(point.forward.normalized));
                    newBullet.Setup();
                }
            }
        }
    }
    #endregion
}
