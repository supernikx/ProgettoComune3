using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il laser del Boss2
/// </summary>
public class Boss2LaserController : MonoBehaviour
{
    [Header("Reference Laser Settings")]
    //Larghezza del laser
    [SerializeField]
    private float laserRadius;
    //Range massimo del laser
    [SerializeField]
    private float maxLaserRange;
    //Riferimento al laser controller
    [SerializeField]
    private LaserController laserCtrl;

    /// <summary>
    /// Riferimento al Boss Controller
    /// </summary>
    private Boss2Controller bossCtrl;
    /// <summary>
    /// Riferimento al collision Controller
    /// </summary>
    private BossCollisionController collisionCtrl;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_bossCtrl"></param>
    public void Setup(Boss2Controller _bossCtrl)
    {
        bossCtrl = _bossCtrl;
        collisionCtrl = bossCtrl.GetBossCollisionController();
        laserCtrl.Setup(maxLaserRange, laserRadius);
    }

    #region Handlers
    /// <summary>
    /// Funzione che gestisce l'evento di agent colpito
    /// </summary>
    /// <param name="obj"></param>
    private void HandleOnAgentHit(AgentController _agent)
    {
        collisionCtrl.OnAgentHit?.Invoke(_agent);
    }
    #endregion

    #region API
    /// <summary>
    /// Funzione che fa partire il Laser
    /// </summary>
    public void StartLaser()
    {
        laserCtrl.OnAgentHit += HandleOnAgentHit;
        laserCtrl.StartLaser();
    }

    /// <summary>
    /// Funzione che stoppa il laser
    /// </summary>
    public void StopLaser()
    {
        laserCtrl.OnAgentHit -= HandleOnAgentHit;
        laserCtrl.StopLaser();
    }

    /// <summary>
    /// Funzionne che ruota il laser verso la posizione passata come parametro
    /// </summary>
    /// <param name="_targetPosition"></param>
    /// <param name="_speed"></param>
    public void RotateLaser(Vector3 _targetPosition, float _speed)
    {
        _targetPosition.y = laserCtrl.transform.position.y;
        Quaternion targetRotation = Quaternion.LookRotation((_targetPosition - laserCtrl.transform.position).normalized, Vector3.up);
        laserCtrl.transform.rotation = Quaternion.RotateTowards(laserCtrl.transform.rotation, targetRotation, _speed * Time.deltaTime);
    }
    #endregion

    private void OnDisable()
    {
        if (laserCtrl != null)
            laserCtrl.OnAgentHit -= HandleOnAgentHit;
    }
}
