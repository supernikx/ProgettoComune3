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
    /// Riferimento al BossTrailController
    /// </summary>
    private Boss1TrailController trailCtrl;
    /// <summary>
    /// Riferimento al BossGraphicController
    /// </summary>
    private Boss1GraphicController graphicCtrl;
    /// <summary>
    /// Riferimento al sound controller
    /// </summary>
    private SoundController soundCtrl;

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
        trailCtrl = GetComponent<Boss1TrailController>();
        soundCtrl = GetComponent<SoundController>();
        graphicCtrl = GetComponentInChildren<Boss1GraphicController>();

        int bossDefeated = UserData.GetBossDefeated();
        if (bossDefeated == 1)
            gameObject.SetActive(false);
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
        trailCtrl.Setup(this);
        shootCtrl.Setup(this);
        graphicCtrl.Setup(this, sm);
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
        OnBossDead?.Invoke(this);
    }

    /// <summary>
    /// Funzione che disabilita il boss
    /// </summary>
    public void DisableBoss()
    {
        gameObject.SetActive(false);
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

    /// <summary>
    /// Funzione che ritorna il BossTrailController
    /// </summary>
    /// <returns></returns>
    public Boss1TrailController GetBossTrailController()
    {
        return trailCtrl;
    }

    /// <summary>
    /// Funzione che ritorna il sound controller
    /// </summary>
    /// <returns></returns>
    public SoundController GetSoundController()
    {
        return soundCtrl;
    }
    #endregion
    #endregion
}
