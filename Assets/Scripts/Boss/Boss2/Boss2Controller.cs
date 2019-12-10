using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce tutte le referenze del Boss 2
/// </summary>
public class Boss2Controller : BossControllerBase
{
    /// <summary>
    /// Riferimento alla StateMachine
    /// </summary>
    private Boss2SMController sm;
    /// <summary>
    /// Riferimento al TentaclesController
    /// </summary>
    private Boss2TentaclesController tentaclesCtrl;
    /// <summary>
    /// Riferimento al phase controller
    /// </summary>
    private Boss2PhaseController phaseCtrl;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_lvlMng"></param>
    public override void Setup(LevelManager _lvlMng)
    {
        base.Setup(_lvlMng);

        sm = GetComponent<Boss2SMController>();
        tentaclesCtrl = GetComponent<Boss2TentaclesController>();
        phaseCtrl = GetComponent<Boss2PhaseController>();
    }

    #region API
    /// <summary>
    /// Funzione che attiva il boss
    /// </summary>
    public override void StartBoss()
    {
        base.StartBoss();
        Boss2SMController.Context context = new Boss2SMController.Context(this, sm, lvlMng);
        sm.Setup(context);
        tentaclesCtrl.Setup(this);
        lifeCtrl.Setup(this);
        collisionCtrl.Setup(this);
        phaseCtrl.Setup(this);
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
        gameObject.SetActive(false);
        OnBossDead?.Invoke(this);
    }

    #region Getter
    /// <summary>
    /// Funzione che ritorna il TentaclesController
    /// </summary>
    /// <returns></returns>
    public Boss2TentaclesController GetTentaclesController()
    {
        return tentaclesCtrl;
    }

    /// <summary>
    /// Funzione che ritorna il phase controlelr
    /// </summary>
    /// <returns></returns>
    public Boss2PhaseController GetPhaseController()
    {
        return phaseCtrl;
    }
    #endregion
    #endregion
}
