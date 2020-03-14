using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CLasse che gestisce lo stato di sparo della torretta
/// </summary>
public class Boss2TourretShootState : Boss2StateBase
{
    [Header("Jump Settings")]
    //Torrette che devono sparare
    [SerializeField]
    private List<int> tourretShootIndex;
    //Delay di sparo della torretta
    [SerializeField]
    private float tourretShootDelay;
    //Delay tra le torrette
    [SerializeField]
    private float delayBetweenTourrets;

    /// <summary>
    /// Riferimento al GroupController
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferiemtno al Boss Controller
    /// </summary>
    private Boss2Controller bossCtrl;
    /// <summary>
    /// Riferimento al Tourrets Controller
    /// </summary>
    private Boss2TourretsController tourretsCtrl;
    /// <summary>
    /// Riferimento al Collision Controller
    /// </summary>
    private BossCollisionController collisionCtrl;
    /// <summary>
    /// Riferimento al Life Controller
    /// </summary>
    private BossLifeController lifeCtrl;
    /// <summary>
    /// Riferimento alla coroutine che esegue il shoot delle torrette
    /// </summary>
    private IEnumerator tourretsShootRoutine;

    public override void Enter()
    {
        groupCtrl = context.GetLevelManager().GetGroupController();
        bossCtrl = context.GetBossController();
        tourretsCtrl = bossCtrl.GetTourretsController();
        collisionCtrl = bossCtrl.GetBossCollisionController();
        lifeCtrl = bossCtrl.GetBossLifeController();

        tourretsCtrl.OnTourretDead += HandleOnTourretDead;
        tourretsCtrl.OnAllTourretsDead += HandleOnAllTourretsDead;
        collisionCtrl.OnAgentHit += HandleOnAgentHit;
        lifeCtrl.OnBossDead += HandleOnBossDead;

        tourretsShootRoutine = TourretsShootCoroutine();
        tourretsCtrl.StartCoroutine(tourretsShootRoutine);
    }

    /// <summary>
    /// Coroutine che esegue lo shoot delle torrette
    /// </summary>
    /// <returns></returns>
    private IEnumerator TourretsShootCoroutine()
    {
        for (int i = 0; i < tourretShootIndex.Count; i++)
        {
            //HACK: Così i designer possono partire a contare da 1
            int tourretIndex = tourretShootIndex[i] - 1;
            if (!tourretsCtrl.CheckTourretIndex(tourretIndex))
                continue;

            tourretsCtrl.LockAim(tourretIndex, true);
            yield return new WaitForSeconds(tourretShootDelay);
            tourretsCtrl.Shoot(tourretIndex);
            yield return new WaitForSeconds(delayBetweenTourrets);
        }

        for (int i = 0; i < tourretShootIndex.Count; i++)
        {
            //HACK: Così i designer possono partire a contare da 1
            int tourretIndex = tourretShootIndex[i] - 1;

            tourretsCtrl.LockAim(tourretIndex, false);
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
        groupCtrl.RemoveAgent(_agent, true);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di morte di una torretta
    /// </summary>
    private void HandleOnTourretDead(int _damage)
    {
        bool canTakeDamage = lifeCtrl.GetCanTakeDamage();
        lifeCtrl.SetCanTakeDamage(true);
        lifeCtrl.TakeDamage(_damage);
        lifeCtrl.SetCanTakeDamage(canTakeDamage);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di morte delle torrette
    /// </summary>
    private void HandleOnAllTourretsDead()
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
        if (tourretsCtrl != null)
        {
            tourretsCtrl.OnTourretDead -= HandleOnTourretDead;
            tourretsCtrl.OnAllTourretsDead -= HandleOnAllTourretsDead;

            if (tourretsShootRoutine != null)
                tourretsCtrl.StopCoroutine(tourretsShootRoutine);
        }

        if (collisionCtrl != null)
            collisionCtrl.OnAgentHit -= HandleOnAgentHit;

        if (lifeCtrl != null)
            lifeCtrl.OnBossDead -= HandleOnBossDead;
    }
}
