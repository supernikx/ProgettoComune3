using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che controlla il boss nel livello
/// </summary>
public class LevelBossController : MonoBehaviour
{
    #region Actions
    /// <summary>
    /// Evento che notifica l'inizio della BossFight
    /// </summary>
    public static Action<BossControllerBase> OnBossFightStart;
    /// <summary>
    /// Evento che notifica la fine della BossFight
    /// </summary>
    public static Action<BossControllerBase> OnBossFightEnd;
    #endregion

    /// <summary>
    /// Riferimento al level manager
    /// </summary>
    private LevelManager lvlMng;
    /// <summary>
    /// Riferimento al group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al boss attuale
    /// </summary>
    private BossControllerBase currentBoss;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_lvlMng"></param>
    public void Setup(LevelManager _lvlMng)
    {
        lvlMng = _lvlMng;
        groupCtrl = lvlMng.GetGroupController();


        foreach (BossControllerBase boss in FindObjectsOfType<BossControllerBase>())
        {
            if (boss != null)
                boss.Setup(lvlMng);
        }

        groupCtrl.OnGroupDead += HandleOnGroupDead;
        ActiveBossTrigger.OnBossTriggered += HandleOnBossTriggered;
    }

    #region Handles
    /// <summary>
    /// Funzione che gestisce l'evento di trigger del Boss
    /// </summary>
    /// <param name="_bossToEnable"></param>
    private void HandleOnBossTriggered(BossControllerBase _bossToEnable)
    {
        if (_bossToEnable != null)
        {
            currentBoss = _bossToEnable;

            currentBoss.OnBossDead += HandleOnBossDead;
            currentBoss.StartBoss();
            OnBossFightStart?.Invoke(currentBoss);
        }
    }

    /// <summary>
    /// Funzione che gestisce l'evento di morte del Boss
    /// </summary>
    /// <param name="_deadBoss"></param>
    private void HandleOnBossDead(BossControllerBase _deadBoss)
    {
        if (currentBoss != null && _deadBoss == currentBoss)
        {
            currentBoss.OnBossDead -= HandleOnBossDead;
            OnBossFightEnd?.Invoke(currentBoss);
            currentBoss = null;
        }
    }

    /// <summary>
    /// Funzione che gestisce l'evento di morte del gruppo
    /// </summary>
    private void HandleOnGroupDead()
    {
        if (currentBoss != null)
            currentBoss.StopBoss();

        if (groupCtrl != null)
            groupCtrl.OnGroupDead -= HandleOnGroupDead;

        OnBossFightEnd?.Invoke(currentBoss);
        currentBoss = null;
    }
    #endregion

    private void OnDisable()
    {
        ActiveBossTrigger.OnBossTriggered -= HandleOnBossTriggered;

        if (currentBoss != null)
            currentBoss.OnBossDead -= HandleOnBossDead;

        if (groupCtrl != null)
            groupCtrl.OnGroupDead -= HandleOnGroupDead;
    }
}
