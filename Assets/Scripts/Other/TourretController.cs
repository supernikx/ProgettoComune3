using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Classee che controlla una torretta
/// </summary>
public class TourretController : MonoBehaviour, IBossDamageable
{
    #region Actions
    /// <summary>
    /// Evento che notifica la morte della torretta
    /// </summary>
    public Action<TourretController> OnTourretDead;
    /// <summary>
    /// Evento che notifica che un agent è stato colpito
    /// </summary>
    public Action<AgentController> OnAgentHit;
    #endregion

    [Header("Tourret Settings")]
    //Vita del tentacolo
    [SerializeField]
    private int tourretLife;
    //Danno del tentacolo al boss quando muore
    [SerializeField]
    private int deadTourretDamage;
    //Punto di sparo
    [SerializeField]
    private Transform shootPoint;
    //Oggetto che mira
    [SerializeField]
    private Transform aimObject;

    /// <summary>
    /// Riferimento al tourret controller
    /// </summary>
    private Boss2TourretsController tourretsCtrl;
    /// <summary>
    /// Vita attuale della torretta
    /// </summary>
    private int currentTourretLife;
    /// <summary>
    /// Rotazione iniziale
    /// </summary>
    private Quaternion startRotation;
    /// <summary>
    /// Bool che identifica se la torretta può mirare
    /// </summary>
    private bool canAim;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    public void Setup(Boss2TourretsController _tourretsCtrl)
    {
        currentTourretLife = tourretLife;
        tourretsCtrl = _tourretsCtrl;
        startRotation = transform.rotation;
        canAim = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AgentController agent = collision.gameObject.GetComponent<AgentController>();
        if (agent != null)
            OnAgentHit?.Invoke(agent);
    }

    #region API
    /// <summary>
    /// Funzione che fa prendere danno alla torretta
    /// </summary>
    /// <param name="_damage"></param>
    public void TakeDamage(int _damage)
    {
        currentTourretLife = Mathf.Clamp(currentTourretLife - _damage, 0, tourretLife);
        if (currentTourretLife == 0)
        {
            DOTween.Kill(transform);

            gameObject.SetActive(false);
            OnTourretDead?.Invoke(this);
        }
    }

    /// <summary>
    /// Funzione che mira nella direzione passata come parmetro
    /// </summary>
    /// <param name="_aimDir"></param>
    public void Aim(Vector3 _aimDir)
    {
        if (canAim)
            aimObject.LookAt(_aimDir);
    }

    /// <summary>
    /// Funzione che spara
    /// </summary>
    public void Shoot()
    {
        Boss2BulletController newBullet = PoolManager.instance.GetPooledObject(ObjectTypes.Boss2Bullet, tourretsCtrl.gameObject).GetComponent<Boss2BulletController>();
        if (newBullet != null)
        {
            newBullet.transform.SetPositionAndRotation(shootPoint.position, Quaternion.LookRotation(shootPoint.forward.normalized));
            newBullet.Setup();
        }
    }

    #region Setter
    /// <summary>
    /// Funzione che imposta se la torretta può mirare
    /// </summary>
    /// <param name="_canAim"></param>
    public void SetCanAim(bool _canAim)
    {
        canAim = _canAim;
    }
    #endregion

    #region Getter
    /// <summary>
    /// Funzione che ritorna il danno che fa il tentacolo quando muore
    /// </summary>
    /// <returns></returns>
    public int GetDeadTentacleDamage()
    {
        return deadTourretDamage;
    }
    #endregion
    #endregion

    private void OnDisable()
    {
        DOTween.Kill(transform);
    }
}
