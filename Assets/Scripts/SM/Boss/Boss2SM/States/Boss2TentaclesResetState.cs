using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CLasse che gestisce lo stato di reset dei tentacoli
/// </summary>
public class Boss2TentaclesResetState : Boss2StateBase
{
    [Header("Jump Settings")]
    //Tentacolo che deve resettare
    [SerializeField]
    private List<int> tentaclesResetIndex;
    //Velocità del tentacolo
    [SerializeField]
    private float resetDuration;
    //Delay tra i tentacoli
    [SerializeField]
    private float delayBetweenTentacles;

    /// <summary>
    /// Riferimento al GroupController
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferiemtno al Boss Controller
    /// </summary>
    private Boss2Controller bossCtrl;
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
    /// <summary>
    /// Riferimento alla coroutine che esegue il tentacles reset
    /// </summary>
    private IEnumerator tentaclesResetRoutine;

    public override void Enter()
    {
        groupCtrl = context.GetLevelManager().GetGroupController();
        bossCtrl = context.GetBossController();
        tentaclesCtrl = bossCtrl.GetTentaclesController();
        collisionCtrl = bossCtrl.GetBossCollisionController();
        lifeCtrl = bossCtrl.GetBossLifeController();

        tentaclesCtrl.OnTentacleDead += HandleOnTentacleDead;
        tentaclesCtrl.OnAllTentaclesDead += HandleOnAllTentaclesDead;
        collisionCtrl.OnAgentHit += HandleOnAgentHit;
        lifeCtrl.OnBossDead += HandleOnBossDead;

        tentaclesResetRoutine = TentaclesResetCoroutine();
        tentaclesCtrl.StartCoroutine(tentaclesResetRoutine);
    }

    /// <summary>
    /// Coroutine che esegue il reset dei tentacoli
    /// </summary>
    /// <returns></returns>
    private IEnumerator TentaclesResetCoroutine()
    {
        for (int i = 0; i < tentaclesResetIndex.Count; i++)
        {
            yield return new WaitForSeconds(delayBetweenTentacles);

            //HACK: Così i designer possono partire a contare da 1
            int tentacleIndex = tentaclesResetIndex[i] - 1;
            tentaclesCtrl.Reset(tentacleIndex, resetDuration);
        }

        Complete();
    }

    #region Handlers
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
        Complete(2);
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

            if (tentaclesResetRoutine != null)
                tentaclesCtrl.StopCoroutine(tentaclesResetRoutine);
        }

        if (collisionCtrl != null)
            collisionCtrl.OnAgentHit -= HandleOnAgentHit;

        if (lifeCtrl != null)
            lifeCtrl.OnBossDead -= HandleOnBossDead;
    }
}
