using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce tutte le referenze del Boss 1
/// </summary>
public class Boss1Controller : BossControllerBase
{
    /// <summary>
    /// Riferimento alla StateMachine
    /// </summary>
    private Boss1SMController sm;
    /// <summary>
    /// Riferimento al ShootController
    /// </summary>
    private Boss1ShootController shootCtrl;
    /// <summary>
    /// Riferimento al PhaseController
    /// </summary>
    private Boss1PhaseController phaseCtrl;
    /// <summary>
    /// Riferimento al CubeExplosion
    /// </summary>
    private CubeExplosion cubeExplosion;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_lvlMng"></param>
    public override void Setup(LevelManager _lvlMng)
    {
        base.Setup(_lvlMng);

        sm = GetComponent<Boss1SMController>();
        shootCtrl = GetComponent<Boss1ShootController>();
        phaseCtrl = GetComponent<Boss1PhaseController>();
        cubeExplosion = GetComponent<CubeExplosion>();
    }

    #region API
    /// <summary>
    /// Funzione che attiva il boss
    /// </summary>
    public override void StartBoss()
    {
        base.StartBoss();
        Boss1SMController.Context context = new Boss1SMController.Context(this, sm, lvlMng);
        sm.Setup(context);
        lifeCtrl.Setup(this);
        collisionCtrl.Setup(this);
        phaseCtrl.Setup(this);
        shootCtrl.Setup(this);
    }

    /// <summary>
    /// Funzione che ferma il Boss
    /// </summary>
    public override void StopBoss()
    {
        sm.GoToState("Empty");
        base.StopBoss();
    }

    /// <summary>
    /// Funzione che uccide il Boss
    /// </summary>
    public void KillBoss()
    {
        cubeExplosion.Explode();
        OnBossDead?.Invoke(this);
    }

    #region Getter
    /// <summary>
    /// Funzione che ritorna il BossPhaseController
    /// </summary>
    /// <returns></returns>
    public Boss1PhaseController GetBossPhaseController()
    {
        return phaseCtrl;
    }

    /// <summary>
    /// Funzione che ritorna il BossShootController
    /// </summary>
    /// <returns></returns>
    public Boss1ShootController GetBossShootController()
    {
        return shootCtrl;
    }
    #endregion
    #endregion
}
