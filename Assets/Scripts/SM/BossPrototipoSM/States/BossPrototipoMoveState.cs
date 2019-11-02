using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di moviemento del boss
/// </summary>
public class BossPrototipoMoveState : BossPrototipoStateBase
{
    [Header("State Settings")]
    //Velocità di movimento del Boss
    [SerializeField]
    private float movementSpeed;
    //Distanza massima percorribile dal movimento
    [SerializeField]
    private float maxMoveDistance;

    /// <summary>
    /// Riferimento al GroupController
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al BossController
    /// </summary>
    private BossPrototipoController bossCtrl;
    /// <summary>
    /// Riferimento al BossCollisionController
    /// </summary>
    private BossPrototipoCollisionController collisionCtrl;
    /// <summary>
    /// Riferimento al LifeController
    /// </summary>
    private BossLifeController lifeCtrl;
    /// <summary>
    /// Distanza percorsa
    /// </summary>
    private float distanceTraveled;

    public override void Enter()
    {
        groupCtrl = context.GetGameManager().GetGroupController();
        bossCtrl = context.GetBossController();
        lifeCtrl = bossCtrl.GetBossLifeController();
        collisionCtrl = bossCtrl.GetCollisionController();
        distanceTraveled = 0;

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

        bossCtrl = null;
        collisionCtrl = null;
        distanceTraveled = 0;
    }
}
