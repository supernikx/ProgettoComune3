using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di laser del Boss 2
/// </summary>
public class Boss2LaserState : Boss2StateBase
{
    [Header("Laser Settings")]
    //Velocità di moviemento del laser
    [SerializeField]
    private float laserTrackSpeed;
    //Durata del laser
    [SerializeField]
    private float laserDuration;
    //Tempo di spawn del laser
    [SerializeField]
    private float laserSpawnTime;

    /// <summary>
    /// Riferimento al GroupController
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferiemtno al Boss Controller
    /// </summary>
    private Boss2Controller bossCtrl;
    /// <summary>
    /// Riferimento al LaserController del Boss2
    /// </summary>
    private Boss2LaserController laserCtrl;
    /// <summary>
    /// Riferimento al Life Controller
    /// </summary>
    private BossLifeController lifeCtrl;
    /// <summary>
    /// Riferimento al Collision Controller
    /// </summary>
    private BossCollisionController collisionCtrl;
    /// <summary>
    /// Timer che conta quanto tempo dura lo stato
    /// </summary>
    private float laserTimer;

    public override void Enter()
    {
        groupCtrl = context.GetLevelManager().GetGroupController();
        bossCtrl = context.GetBossController();
        laserCtrl = bossCtrl.GetLaserController();
        lifeCtrl = bossCtrl.GetBossLifeController();
        collisionCtrl = bossCtrl.GetBossCollisionController();

        lifeCtrl.OnBossDead += HandleOnBossDead;
        collisionCtrl.OnAgentHit += HandleOnAgentHit;

        laserTimer = laserDuration;
        laserCtrl.SpawnLaser(laserSpawnTime, groupCtrl.GetGroupCenterPoint(), HandleOnLaserSpawn);
    }

    public override void Tick()
    {
        if (laserCtrl.IsEnable())
        {
            laserCtrl.RotateLaser(groupCtrl.GetGroupCenterPoint(), laserTrackSpeed);

            laserTimer -= Time.deltaTime;
            if (laserTimer <= 0)
                Complete(2);
        }
    }

    #region Handles
    /// <summary>
    /// Funzione che gestisce la callback di spawn del laser
    /// </summary>
    private void HandleOnLaserSpawn()
    {
        laserCtrl.StartLaser();
    }

    /// <summary>
    /// Funzione che gestisce l'evento di hit di un agent
    /// </summary>
    /// <param name="_agent"></param>
    private void HandleOnAgentHit(AgentController _agent)
    {
        groupCtrl.RemoveAgent(_agent);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di morte del Boss
    /// </summary>
    private void HandleOnBossDead()
    {
        Complete(1);
    }
    #endregion

    public override void Exit()
    {
        if (lifeCtrl != null)
            lifeCtrl.OnBossDead -= HandleOnBossDead;

        if (collisionCtrl != null)
            collisionCtrl.OnAgentHit -= HandleOnAgentHit;

        if (laserCtrl != null)
            laserCtrl.StopLaser();
    }
}
