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
    /// Riferimento al LaserController
    /// </summary>
    private Boss2LaserController laserCtrl;
    /// <summary>
    /// Riferimento al CoverBlockController
    /// </summary>
    private Boss2CoverBlocksController coverBlockCtrl;
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
        laserCtrl = GetComponent<Boss2LaserController>();
        coverBlockCtrl = GetComponent<Boss2CoverBlocksController>();
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
        laserCtrl.Setup(this);
        lifeCtrl.Setup(this);
        collisionCtrl.Setup(this);
        phaseCtrl.Setup(this);
        coverBlockCtrl.Setup();
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
    /// Funzione di Debug che cambia il colore del boss
    /// </summary>
    /// <param name="_color"></param>
    public void ChangeColor(Color _color)
    {
        GetComponentInChildren<MeshRenderer>().material.color = _color;
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
    /// Funzione che ritorna il LaserController
    /// </summary>
    /// <returns></returns>
    public Boss2LaserController GetLaserController()
    {
        return laserCtrl;
    }

    /// <summary>
    /// Funzione che ritorna il CoverBlockController
    /// </summary>
    /// <returns></returns>
    public Boss2CoverBlocksController GetCoverBlocksController()
    {
        return coverBlockCtrl;
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
