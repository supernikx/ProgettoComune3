using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di mira del BossPrototipo
/// </summary>
public class BossPrototipoAimState : BossPrototipoStateBase
{
    [Header("State Settings")]
    //Range di tempo che il boss deve passare a mirare
    [SerializeField]
    private Vector2 aimTimeRange;

    /// <summary>
    /// Riferimento al group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al BossController
    /// </summary>
    private BossPrototipoController bossCtrl;
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
        groupCtrl = context.GetLevelManager().GetGroupController();
        bossCtrl = context.GetBossController();
        lifeCtrl = bossCtrl.GetBossLifeController();

        timer = 0;
        aimTime = Random.Range(aimTimeRange.x, aimTimeRange.y);

        lifeCtrl.OnBossDead += HandleOnBossDead;
    }

    public override void Tick()
    {
        timer += Time.deltaTime;
        if (timer >= aimTime)
            Complete();

        LookAtPosition(groupCtrl.GetGroupCenterPoint());
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

        groupCtrl = null;
        bossCtrl = null;
        aimTime = 0;
        timer = 0;
    }
}
