using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CLasse che gestisce lo stato di rotazione dei tentacoli
/// </summary>
public class Boss2TentaclesRotateState : Boss2StateBase
{
    [Header("Rotation Settings")]
    //Velocità di rotazione
    [SerializeField]
    private float rotationTime;

    /// <summary>
    /// Riferimento al GroupController
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferiemtno al Boss Controller
    /// </summary>
    private Boss2Controller bossCltr;
    /// <summary>
    /// Riferimento al Tentacles Controller
    /// </summary>
    private Boss2TentaclesController tentaclesCtrl;
    /// <summary>
    /// Riferimento al Collision Controller
    /// </summary>
    private BossCollisionController collisionCtrl;
    /// <summary>
    /// Riferimento al Life Controller
    /// </summary>
    private BossLifeController lifeCtrl;

    public override void Enter()
    {
        groupCtrl = context.GetLevelManager().GetGroupController();
        bossCltr = context.GetBossController();
        tentaclesCtrl = bossCltr.GetTentaclesController();
        collisionCtrl = bossCltr.GetBossCollisionController();
        lifeCtrl = bossCltr.GetBossLifeController();

        tentaclesCtrl.OnTentaclesRotated += HandleOnTentaclesRotated;
        tentaclesCtrl.OnTentacleDead += HandleOnTentacleDead;
        tentaclesCtrl.OnAllTentaclesDead += HandleOnAllTentaclesDead;
        collisionCtrl.OnAgentHit += HandleOnAgentHit;
        lifeCtrl.OnBossDead += HandleOnAllTentaclesDead;

        tentaclesCtrl.Rotate(rotationTime);
    }

    #region Handlers
    /// <summary>
    /// Funzione che gestisce l'evento di rotazione completata
    /// </summary>
    private void HandleOnTentaclesRotated()
    {
        Complete();
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
    /// Funzione che gestisce l'evento di morte di un tentacolo
    /// </summary>
    private void HandleOnTentacleDead()
    {
        bool canTakeDamage = lifeCtrl.GetCanTakeDamage();
        lifeCtrl.SetCanTakeDamage(true);
        lifeCtrl.TakeDamage(tentaclesCtrl.GetDeadTentacleDamage());
        lifeCtrl.SetCanTakeDamage(canTakeDamage);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di morte di tutti i tantacoli
    /// </summary>
    private void HandleOnAllTentaclesDead()
    {
        Complete(1);
    }
    #endregion

    public override void Exit()
    {
        if (tentaclesCtrl != null)
        {
            tentaclesCtrl.OnTentaclesRotated -= HandleOnTentaclesRotated;
            tentaclesCtrl.OnTentacleDead -= HandleOnTentacleDead;
            tentaclesCtrl.OnAllTentaclesDead -= HandleOnAllTentaclesDead;
        }

        if (collisionCtrl != null)
            collisionCtrl.OnAgentHit -= HandleOnAgentHit;

        if (lifeCtrl != null)
            lifeCtrl.OnBossDead -= HandleOnAllTentaclesDead;
    }
}
