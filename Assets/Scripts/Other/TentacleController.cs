using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Classee che controlla un tentacolo
/// </summary>
public class TentacleController : MonoBehaviour, IBossDamageable
{
    #region Actions
    /// <summary>
    /// Evento che notifica la morte del tentacolo
    /// </summary>
    public Action<TentacleController> OnTentacleDead;
    /// <summary>
    /// Evento che notifica il completamnento della rotazione del tentacolo
    /// </summary>
    public Action<TentacleController> OnTentacleRotated;
    /// <summary>
    /// Evento che notifica che un agent è stato colpito
    /// </summary>
    public Action<AgentController> OnAgentHit;
    #endregion

    [Header("Tentacle Settings")]
    //Vita del tentacolo
    [SerializeField]
    private int tentacleLife;

    /// <summary>
    /// Riferimento al tentacles controller
    /// </summary>
    private Boss2TentaclesController tentaclesctrl;
    /// <summary>
    /// Vita attuale del tentacolo
    /// </summary>
    private int currentTentacleLife;
    /// <summary>
    /// Zona attuale
    /// </summary>
    private int currentZoneIndex;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    public void Setup(Boss2TentaclesController _tentaclesCtrl, int _startZoneIndex)
    {
        currentTentacleLife = tentacleLife;
        tentaclesctrl = _tentaclesCtrl;
        currentZoneIndex = _startZoneIndex;
        transform.rotation = tentaclesctrl.GetZoneByIndex(_startZoneIndex).transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AgentController agent = collision.gameObject.GetComponent<AgentController>();
        if (agent != null)
            OnAgentHit?.Invoke(agent);
    }

    #region API
    /// <summary>
    /// Funzione che fa ruotare il tentacolo al punto successivo
    /// </summary>
    /// <param name="rotationSpeed"></param>
    public void RotateToNextPoint(float rotationSpeed)
    {
        Transform nextZone = tentaclesctrl.GetZoneByIndex(currentZoneIndex + 1);
        transform.DORotateQuaternion(nextZone.transform.rotation, 10f / rotationSpeed).SetEase(Ease.Linear).OnComplete(() =>
         {
             transform.rotation = nextZone.rotation;
             currentZoneIndex = tentaclesctrl.GetIndexByZone(nextZone);
             OnTentacleRotated?.Invoke(this);
         });
    }

    /// <summary>
    /// Funzione che fa prendere danno al tentacolo
    /// </summary>
    /// <param name="_damage"></param>
    public void TakeDamage(int _damage)
    {
        currentTentacleLife = Mathf.Clamp(currentTentacleLife - _damage, 0, tentacleLife);
        if (currentTentacleLife == 0)
        {
            DOTween.Kill(transform);

            gameObject.SetActive(false);
            OnTentacleDead?.Invoke(this);
        }
    }
    #endregion

    private void OnDisable()
    {
        DOTween.Kill(transform);
    }
}
