using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CLasse che gestisce lo stato di salto dei tentacoli
/// </summary>
public class Boss2TentaclesJumpState : Boss2StateBase
{
    [Header("Jump Settings")]
    //Zone in cui verrà eseguito il salto
    [SerializeField]
    private List<int> jumpZones;
    //Forza del salto
    [SerializeField]
    private float jumpForce;
    //Durata del salto
    [SerializeField]
    private float jumpTime;

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
        tentaclesCtrl.OnAllTentaclesDead += HandleOnTentaclesDead;
        collisionCtrl.OnAgentHit += HandleOnAgentHit;
        lifeCtrl.OnBossDead += HandleOnTentaclesDead;

        TentaclesJump();
    }

    private void TentaclesJump()
    {
        for (int i = 0; i < jumpZones.Count; i++)
        {
            //HACK: Così i designer possono partire a contare da 1
            int jumpZone = jumpZones[i] - 1;
            tentaclesCtrl.Jump(jumpZone, jumpForce, jumpTime);
        }

        Complete();
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
    /// Funzione che gestisce l'evento di morte dei tantacoli
    /// </summary>
    private void HandleOnTentaclesDead()
    {
        Complete(1);
    }
    #endregion

    public override void Exit()
    {
        if (tentaclesCtrl != null)
        {
            tentaclesCtrl.OnTentaclesRotated -= HandleOnTentaclesRotated;
            tentaclesCtrl.OnAllTentaclesDead -= HandleOnTentaclesDead;
        }

        if (collisionCtrl != null)
            collisionCtrl.OnAgentHit -= HandleOnAgentHit;

        if (lifeCtrl != null)
            lifeCtrl.OnBossDead -= HandleOnTentaclesDead;
    }
}
