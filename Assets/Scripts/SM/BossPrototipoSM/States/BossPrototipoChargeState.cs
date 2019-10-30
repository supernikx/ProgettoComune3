using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il charge del boss
/// </summary>
public class BossPrototipoChargeState : BossPrototipoStateBase
{
    [Header("State Settings")]
    //Range di tempo che il boss deve passare a caricare
    [SerializeField]
    private Vector2 chargeTimeRange;

    /// <summary>
    /// Riferimento al LifeController
    /// </summary>
    private BossLifeController lifeCtrl;
    /// <summary>
    /// Tempo che il boss deve usare per mirare
    /// </summary>
    private float aimTime;
    /// <summary>
    /// Timer che conta il tempo passato
    /// </summary>
    private float timer;

    public override void Enter()
    {
        lifeCtrl = context.GetBossController().GetBossLifeController();
        timer = 0;
        aimTime = Random.Range(chargeTimeRange.x, chargeTimeRange.y);

        lifeCtrl.OnBossDead += HandleOnBossDead;
    }

    public override void Tick()
    {
        timer += Time.deltaTime;
        if (timer >= aimTime)
            Complete();
    }

    #region Handler
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

        aimTime = 0;
        timer = 0;
    }
}
