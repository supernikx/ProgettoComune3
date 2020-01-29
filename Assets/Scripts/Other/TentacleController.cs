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
    /// Evento che notifica che un agent è stato colpito
    /// </summary>
    public Action<AgentController> OnAgentHit;
    #endregion

    [Header("Tentacle Settings")]
    //Vita del tentacolo
    [SerializeField]
    private int tentacleLife;
    //Danno del tentacolo al boss quando muore
    [SerializeField]
    private int deadTentacleDamage;

    /// <summary>
    /// Riferimento al tentacles controller
    /// </summary>
    private Boss2TentaclesController tentaclesctrl;
    /// <summary>
    /// Vita attuale del tentacolo
    /// </summary>
    private int currentTentacleLife;
    /// <summary>
    /// Rotazione iniziale
    /// </summary>
    private Quaternion startRotation;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    public void Setup(Boss2TentaclesController _tentaclesCtrl)
    {
        currentTentacleLife = tentacleLife;
        tentaclesctrl = _tentaclesCtrl;
        startRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AgentController agent = collision.gameObject.GetComponent<AgentController>();
        if (agent != null)
            OnAgentHit?.Invoke(agent);
    }

    #region API
    /// <summary>
    /// Funzione che esegue lo Stomp del tentacolo nella posizione passata come paraemtro
    /// </summary>
    /// <param name="stompPosition"></param>
    /// <param name="tentacleSpeed"></param>
    public void Stomp(Vector3 _stompPosition, float _stompDuration)
    {
        Quaternion stompRotation = Quaternion.LookRotation(_stompPosition, Vector3.up);
        transform.DORotateQuaternion(stompRotation, _stompDuration);
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

    /// <summary>
    /// Funzione che esegue il reset della rotazione del tentacolo
    /// </summary>
    /// <param name="_resetDuration"></param>
    public void Reset(float _resetDuration)
    {
        transform.DORotateQuaternion(startRotation, _resetDuration);
    }

    #region Getter
    /// <summary>
    /// Funzione che ritorna il danno che fa il tentacolo quando muore
    /// </summary>
    /// <returns></returns>
    public int GetDeadTentacleDamage()
    {
        return deadTentacleDamage;
    }
    #endregion
    #endregion

    private void OnDisable()
    {
        DOTween.Kill(transform);
    }
}
