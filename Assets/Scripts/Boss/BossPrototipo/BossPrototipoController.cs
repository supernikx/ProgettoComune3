using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce tutte le referenze del boss
/// </summary>
public class BossPrototipoController : MonoBehaviour
{
    /// <summary>
    /// Riferimento al game manager
    /// </summary>
    private GameManager gm;
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
    /// <param name="_gm"></param>
    public void Setup(GameManager _gm)
    {
        gm = _gm;
        sm = GetComponent<BossPrototipoSMController>();
        collisionCtrl = GetComponent<BossPrototipoCollisionController>();
        lifeCtrl = GetComponent<BossLifeController>();
        cubeExplosion = GetComponent<CubeExplosion>();

        BossPrototipoSMController.Context context = new BossPrototipoSMController.Context(this, sm, _gm);
        sm.Setup(context);
        lifeCtrl.Setup();
    }

    #region API
    /// <summary>
    /// Funzione che uccide il Boss
    /// </summary>
    public void KillBoss()
    {
        cubeExplosion.Explode();
    }

    /// <summary>
    /// Funzione che ferma il Boss
    /// </summary>
    public void StopBoss()
    {
        sm.GoToState("Empty");
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
