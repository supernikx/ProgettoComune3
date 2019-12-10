using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CLasse che gestisce lo stato di stomp dei tentacoli
/// </summary>
public class Boss2TentaclesStompState : Boss2StateBase
{
    [Header("Jump Settings")]
    //Tentacolo che deve cadere
    [SerializeField]
    private List<int> tentaclesStompIndex;
    //Velocità del tentacolo
    [SerializeField]
    private float stompDuration;
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
    /// Riferimento alla coroutine che esegue il tentacles stomp
    /// </summary>
    private IEnumerator tentaclesStompRoutine;

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

        tentaclesStompRoutine = TentaclesStompCoroutine();
        tentaclesCtrl.StartCoroutine(tentaclesStompRoutine);
    }

    /// <summary>
    /// Coroutine che esegue lo stomp dei tentacoli
    /// </summary>
    /// <returns></returns>
    private IEnumerator TentaclesStompCoroutine()
    {
        for (int i = 0; i < tentaclesStompIndex.Count; i++)
        {
            yield return new WaitForSeconds(delayBetweenTentacles);

            //HACK: Così i designer possono partire a contare da 1
            int tentacleIndex = tentaclesStompIndex[i] - 1;
            Vector3 stompPosition = groupCtrl.GetGroupCenterPoint();

            tentaclesCtrl.Stomp(tentacleIndex, stompPosition, stompDuration);
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
        Complete(1);
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

            if (tentaclesStompRoutine != null)
                tentaclesCtrl.StopCoroutine(tentaclesStompRoutine);
        }

        if (collisionCtrl != null)
            collisionCtrl.OnAgentHit -= HandleOnAgentHit;

        if (lifeCtrl != null)
            lifeCtrl.OnBossDead -= HandleOnBossDead;
    }
}
