using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di attesa del Boss 2
/// </summary>
public class Boss2WaitingState : Boss2StateBase
{
    [Header("State Settings")]
    //Range di tempo che il boss deve aspettare
    [SerializeField]
    private Vector2 waitTimeRange;

    /// <summary>
    /// Riferimento al GroupController
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al BossController
    /// </summary>
    private Boss2Controller bossCtrl;
    /// <summary>
    /// Riferimento al BossCollisionController
    /// </summary>
    private BossCollisionController collisionCtrl;
    /// <summary>
    /// Riferimento al LifeController
    /// </summary>
    private BossLifeController lifeCtrl;
    /// <summary>
    /// Riferimento al Tentacles Controller
    /// </summary>
    private Boss2TentaclesController tentaclesCtrl;
    /// <summary>
    /// Tempo che il boss deve aspettare
    /// </summary>
    private float waitTime;
    /// <summary>
    /// Timer che conta il tempo passato
    /// </summary>
    private float timer;

    public override void Enter()
    {
        groupCtrl = context.GetLevelManager().GetGroupController();
        bossCtrl = context.GetBossController();
        lifeCtrl = bossCtrl.GetBossLifeController();
        collisionCtrl = bossCtrl.GetBossCollisionController();
        tentaclesCtrl = bossCtrl.GetTentaclesController();

        timer = 0;
        waitTime = Random.Range(waitTimeRange.x, waitTimeRange.y);

        lifeCtrl.OnBossDead += HandleOnBossDead;
        collisionCtrl.OnAgentHit += HandleOnAgentHit;
        tentaclesCtrl.OnTentacleDead += HandleOnTentacleDead;
        tentaclesCtrl.OnAllTentaclesDead += HandleOnAllTentaclesDead;
    }

    public override void Tick()
    {
        timer += Time.deltaTime;
        if (timer >= waitTime)
            Complete();
    }

    #region Handlers
    /// <summary>
    /// Funzione che gestisce l'evento di morte di un tentacolo
    /// </summary>
    private void HandleOnTentacleDead(int _damage)
    {
        bool canTakeDamage = lifeCtrl.GetCanTakeDamage();
        lifeCtrl.SetCanTakeDamage(true);
        lifeCtrl.TakeDamage(_damage);
        lifeCtrl.SetCanTakeDamage(canTakeDamage);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di morte dei tantacoli
    /// </summary>
    private void HandleOnAllTentaclesDead()
    {
        Complete(1);
    }

    /// <summary>
    /// Funzione che gestisce l'evento collisionCtrl.OnAgentHit
    /// <param name="obj"></param>
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
        if (tentaclesCtrl != null)
        {
            tentaclesCtrl.OnTentacleDead -= HandleOnTentacleDead;
            tentaclesCtrl.OnAllTentaclesDead -= HandleOnAllTentaclesDead;
        }

        if (lifeCtrl != null)
            lifeCtrl.OnBossDead -= HandleOnBossDead;

        if(collisionCtrl != null)
            collisionCtrl.OnAgentHit -= HandleOnAgentHit;
    }
}
