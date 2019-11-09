using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che controlla il boss nel livello
/// </summary>
public class LevelBossController : MonoBehaviour
{
    /// <summary>
    /// Riferimento al level manager
    /// </summary>
    LevelManager lvlMng;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_lvlMng"></param>
    public void Setup(LevelManager _lvlMng)
    {
        lvlMng = _lvlMng;

        BossControllerBase boss = FindObjectOfType<BossControllerBase>();
        if (boss != null)
            boss.Setup(lvlMng);

        ActiveBossTrigger.OnBossTriggered += HandleOnBosstriggered;
    }

    /// <summary>
    /// Funzione che gestisce l'evento di trigger del Boss
    /// </summary>
    /// <param name="_bossToEnable"></param>
    private void HandleOnBosstriggered(BossControllerBase _bossToEnable)
    {
        if (_bossToEnable != null)
            _bossToEnable.StartBoss();
    }

    private void OnDisable()
    {
        ActiveBossTrigger.OnBossTriggered -= HandleOnBosstriggered;
    }
}
