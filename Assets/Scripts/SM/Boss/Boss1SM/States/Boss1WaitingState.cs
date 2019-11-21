using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di attesa del Boss 1
/// </summary>
public class Boss1WaitingState : Boss1StateBase
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
        bossPhaseCtrl = bossCtrl.GetBossPhaseController();

        timer = 0;
        waitTime = Random.Range(waitTimeRange.x, waitTimeRange.y);

        bossPhaseCtrl.OnSecondPhaseStart += HandleOnSecondPhaseStart;
        bossPhaseCtrl.OnThirdPhaseStart += HandleOnThirdPhaseStart;
        lifeCtrl.OnBossDead += HandleOnBossDead;
        collisionCtrl.OnAgentHit += HandleOnAgentHit;
    }

    public override void Tick()
    {
        timer += Time.deltaTime;
        if (timer >= waitTime)
            Complete();
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
        if (bossPhaseCtrl != null)
        {
            bossPhaseCtrl.OnSecondPhaseStart -= HandleOnSecondPhaseStart;
            bossPhaseCtrl.OnThirdPhaseStart -= HandleOnThirdPhaseStart;
        }

        if (lifeCtrl != null)
            lifeCtrl.OnBossDead -= HandleOnBossDead;

        if(collisionCtrl != null)
            collisionCtrl.OnAgentHit -= HandleOnAgentHit;

        bossPhaseCtrl = null;
        lifeCtrl = null;
        bossCtrl = null;
        waitTime = 0;
        timer = 0;
    }
}
