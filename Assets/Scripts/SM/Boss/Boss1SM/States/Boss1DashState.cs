using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di Dash del Boss 1
/// </summary>
public class Boss1DashState : Boss1StateBase
{
    [Header("State Settings")]
    //Velocità di movimento del Boss
    [SerializeField]
    private float movementSpeed;
    //Distanza massima percorribile dal movimento
    [SerializeField]
    private float maxMoveDistance;

    public TrailController trail;

    /// <summary>
    /// Riferimento al GroupController
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al BossController
    /// </summary>
    private Boss1Controller bossCtrl;
    /// <summary>
    /// Riferimento al BossCollisionController
    /// </summary>
    private BossCollisionController collisionCtrl;
    /// <summary>
    /// Riferimento al LifeController
    /// </summary>
    private BossLifeController lifeCtrl;
    /// <summary>
    /// Riferimento al BossPhaseController
    /// </summary>
    private Boss1PhaseController bossPhaseCtrl;
    /// <summary>
    /// Riferimento al TrailController
    /// </summary>
    private Boss1TrailController trailController;
    /// <summary>
    /// Distanza percorsa
    /// </summary>
    private float distanceTraveled;

    public override void Enter()
    {
        groupCtrl = context.GetLevelManager().GetGroupController();
        bossCtrl = context.GetBossController();
        lifeCtrl = bossCtrl.GetBossLifeController();
        collisionCtrl = bossCtrl.GetBossCollisionController();
        bossPhaseCtrl = bossCtrl.GetBossPhaseController();
        trailController = bossCtrl.GetBossTrailController();

        distanceTraveled = 0;
        LookAtPosition(groupCtrl.GetGroupCenterPoint());
        trailController.InstantiateNewTrail();

        bossPhaseCtrl.OnSecondPhaseStart += HandleOnSecondPhaseStart;
        bossPhaseCtrl.OnThirdPhaseStart += HandleOnThirdPhaseStart;
        lifeCtrl.OnBossDead += HandleOnBossDead;
        collisionCtrl.OnObstacleHit += HandleOnObstacleHit;
        collisionCtrl.OnAgentHit += HandleOnAgentHit;
    }

    public override void Tick()
    {
        distanceTraveled += movementSpeed * Time.deltaTime;
        if (distanceTraveled >= maxMoveDistance)
            Complete();

        bossCtrl.transform.position = Vector3.MoveTowards(bossCtrl.transform.position, bossCtrl.transform.position + bossCtrl.transform.forward, movementSpeed * Time.deltaTime);
        trailController.UpdateLastTrail();
    }


    /// <summary>
    /// Funzione che fa guardare al Boss la posizione passata come parametro
    /// </summary>
    /// <param name="_lookPos"></param>
    private void LookAtPosition(Vector3 _lookPos)
    {
        _lookPos.y = bossCtrl.transform.position.y;
        bossCtrl.transform.LookAt(_lookPos, Vector3.up);
    }

    #region Handlers
    /// <summary>
    /// Funzione che gestisce l'evento collisionCtrl.OnAgentHit
    /// <param name="obj"></param>
    private void HandleOnAgentHit(AgentController _agent)
    {
        groupCtrl.RemoveAgent(_agent);
    }

    /// <summary>
    /// Funzione che gestisce l'evento ollisionCtrl.OnObstacleHit
    /// <param name="obj"></param>
    private void HandleOnObstacleHit(GameObject _obstacle)
    {
        distanceTraveled = maxMoveDistance;
    }

    /// <summary>
    /// Funzione che gestisce l'evento di morte del Boss
    /// </summary>
    private void HandleOnBossDead()
    {
        Complete(1);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di inizio secondo fase del Boss
    /// </summary>
    private void HandleOnSecondPhaseStart()
    {
        Complete(2);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di inizio terza fase del Boss
    /// </summary>
    private void HandleOnThirdPhaseStart()
    {
        Complete(3);
    }
    #endregion

    public override void Exit()
    {
        if (collisionCtrl != null)
        {
            collisionCtrl.OnObstacleHit -= HandleOnObstacleHit;
            collisionCtrl.OnAgentHit -= HandleOnAgentHit;
        }

        if (lifeCtrl != null)
            lifeCtrl.OnBossDead -= HandleOnBossDead;

        if (bossPhaseCtrl != null)
        {
            bossPhaseCtrl.OnSecondPhaseStart -= HandleOnSecondPhaseStart;
            bossPhaseCtrl.OnThirdPhaseStart -= HandleOnThirdPhaseStart;
        }

        if (trailController != null)
            trailController.EndTrail();

        bossCtrl = null;
        collisionCtrl = null;
        bossPhaseCtrl = null;
        lifeCtrl = null;
        distanceTraveled = 0;
    }
}
