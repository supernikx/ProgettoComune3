﻿using System;
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
    /// Zona attuale
    /// </summary>
    private int currentZoneIndex;
    /// <summary>
    /// Rotazione iniziale
    /// </summary>
    private Quaternion startRotation;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    public void Setup(Boss2TentaclesController _tentaclesCtrl, int _startZoneIndex = -1)
    {
        currentTentacleLife = tentacleLife;
        tentaclesctrl = _tentaclesCtrl;
        currentZoneIndex = _startZoneIndex;
        startRotation = transform.rotation;

        if (_startZoneIndex != -1)
            transform.rotation = tentaclesctrl.GetZoneByIndex(currentZoneIndex).transform.rotation;
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
    /// <param name="_rotatingTime"></param>
    public void RotateToNextPoint(float _rotatingTime)
    {
        Transform nextZone = tentaclesctrl.GetZoneByIndex(currentZoneIndex + 1);
        transform.DORotateQuaternion(nextZone.transform.rotation, _rotatingTime).SetEase(Ease.Linear).OnComplete(() =>
         {
             transform.rotation = nextZone.rotation;
             currentZoneIndex = tentaclesctrl.GetIndexByZone(nextZone);
             OnTentacleRotated?.Invoke(this);
         });
    }

    /// <summary>
    /// Funzione che fa eseguire un salto al tentacolo in base ai parametri passati
    /// </summary>
    /// <param name="_jumpForce"></param>
    /// <param name="_jumpTime"></param>
    public void Jump(float _jumpForce, float _jumpTime)
    {
        transform.DOJump(transform.position, _jumpForce, 1, _jumpTime);
    }

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
    /// Funzione che ritorna la zona attuale del tentacolo
    /// </summary>
    /// <returns></returns>
    public int GetCurrentZone()
    {
        return currentZoneIndex;
    }

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