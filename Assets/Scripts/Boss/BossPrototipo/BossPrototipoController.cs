using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce tutte le referenze del boss
/// </summary>
public class BossPrototipoController : BossControllerBase
{
    /// <summary>
    /// Riferimento al Level Manager
    /// </summary>
    private LevelManager lvlMng;
    /// <summary>
    /// Riferimento alla StateMachine
    /// </summary>
    private BossPrototipoSMController sm;
    /// <summary>
    /// Riferimento al Collision Controller
    /// </summary>
    private BossPrototipoCollisionController collisionCtrl;
    /// <summary>
    /// Riferimentoi al Life Controller
    /// </summary>
    private BossLifeController lifeCtrl;
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
        lvlMng = _lvlMng;
        sm = GetComponent<BossPrototipoSMController>();
        collisionCtrl = GetComponent<BossPrototipoCollisionController>();
        lifeCtrl = GetComponent<BossLifeController>();
        cubeExplosion = GetComponent<CubeExplosion>();
    }

    #region API
    /// <summary>
    /// Funzione che attiva il boss
    /// </summary>
    public override void StartBoss()
    {
        BossPrototipoSMController.Context context = new BossPrototipoSMController.Context(this, sm, lvlMng);
        sm.Setup(context);
        lifeCtrl.Setup();
    }

    /// <summary>
    /// Funzione che ferma il Boss
    /// </summary>
    public override void StopBoss()
    {
        sm.GoToState("Empty");
    }

    /// <summary>
    /// Funzione che uccide il Boss
    /// </summary>
    public void KillBoss()
    {
        cubeExplosion.Explode();
    }

    #region Getter
    /// <summary>
    /// Funzione che ritorna il CollisionController
    /// </summary>
    /// <returns></returns>
    public BossPrototipoCollisionController GetCollisionController()
    {
        return collisionCtrl;
    }

    /// <summary>
    /// Funzione che ritorna il LifeController
    /// </summary>
    /// <returns></returns>
    public BossLifeController GetBossLifeController()
    {
        return lifeCtrl;
    }
    #endregion
    #endregion
}
