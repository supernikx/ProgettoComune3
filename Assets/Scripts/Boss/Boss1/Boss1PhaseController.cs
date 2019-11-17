using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce le fasi del Boss 1
/// </summary>
public class Boss1PhaseController : MonoBehaviour
{
    #region Actions
    /// <summary>
    /// Evento che notifica l'inizio della seconda fase
    /// </summary>
    public Action OnSecondPhaseStart;
    /// <summary>
    /// Evento che notifica l'inizio della terza fase
    /// </summary>
    public Action OnThirdPhaseStart;
    #endregion

    [Header("Phases Settings")]
    //Percentuale di vita per attivare la seconda fase
    [SerializeField]
    private float secondPhaseLifePercentage;
    //Percentuale di vita per attivare la terza fase
    [SerializeField]
    private float thirdPhaseLifePercentage;

    /// <summary>
    /// Riferimento al BossController
    /// </summary>
    private Boss1Controller bossCtrl;
    /// <summary>
    /// Riferimento al BossLifeController
    /// </summary>
    private BossLifeController bossLifeCtrl;
    /// <summary>
    /// Quantità di vita massima del Boss 
    /// </summary>
    private int maxBossLife;
    /// <summary>
    /// Fase attuale del Boss
    /// </summary>
    private int currentPhase;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_bossCtrl"></param>
    public void Setup(Boss1Controller _bossCtrl)
    {
        bossCtrl = _bossCtrl;
        bossLifeCtrl = bossCtrl.GetBossLifeController();
        maxBossLife = bossLifeCtrl.GetMaxBossLife();
        currentPhase = 1;

        bossLifeCtrl.OnBossTakeDamage += HandleOnBossTakeDamage;
    }

    /// <summary>
    /// Funzione che gestisce l'evento di danno del BossLifeController
    /// </summary>
    /// <param name="_currentLife"></param>
    private void HandleOnBossTakeDamage(int _currentLife)
    {
        float currentBossLifePercentage = (_currentLife * 100) / maxBossLife;

        if (currentPhase == 1 && secondPhaseLifePercentage >= currentBossLifePercentage)
        {
            currentPhase = 2;
            OnSecondPhaseStart?.Invoke();
        }
        else if (currentPhase == 2 && thirdPhaseLifePercentage >= currentBossLifePercentage)
        {
            currentPhase = 3;
            OnThirdPhaseStart?.Invoke();
        }
    }

    private void OnDisable()
    {
        if (bossLifeCtrl != null)
            bossLifeCtrl.OnBossTakeDamage -= HandleOnBossTakeDamage;
    }
}
