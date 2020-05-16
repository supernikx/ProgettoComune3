using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce le fasi del Boss 2
/// </summary>
public class Boss2PhaseController : MonoBehaviour
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
    /// <summary>
    /// Evento che notifica l'inizio della quarta fase
    /// </summary>
    public Action OnFourthPhaseStart;
    #endregion

    [Header("Phases Settings")]
    //Percentuale di vita per attivare la seconda fase
    [SerializeField]
    private float secondPhaseLifePercentage;
    //Percentuale di vita per attivare la terza fase
    [SerializeField]
    private float thirdPhaseLifePercentage;
    //Percentuale di vita per attivare la quarta fase
    [SerializeField]
    private float fourthPhaseLifePercentage;

    /// <summary>
    /// Riferimento al BossController
    /// </summary>
    private Boss2Controller bossCtrl;
    /// <summary>
    /// Riferimento al BossLifeController
    /// </summary>
    private BossLifeController bossLifeCtrl;
    /// <summary>
    /// Quantità di vita massima del Boss 
    /// </summary>
    private int maxBossLife;
    /// <summary>
    /// identifica la fase del boss
    /// </summary>
    private int currentFase;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_bossCtrl"></param>
    public void Setup(Boss2Controller _bossCtrl)
    {
        bossCtrl = _bossCtrl;
        bossLifeCtrl = bossCtrl.GetBossLifeController();
        maxBossLife = bossLifeCtrl.GetMaxBossLife();
        currentFase = 1;

        bossLifeCtrl.OnBossTakeDamage += HandleOnBossTakeDamage;
    }

    /// <summary>
    /// Funzione che gestisce l'evento di danno del BossLifeController
    /// </summary>
    /// <param name="_currentLife"></param>
    private void HandleOnBossTakeDamage(int _currentLife)
    {
        float currentBossLifePercentage = (_currentLife * 100) / maxBossLife;

        if (currentFase == 1 && secondPhaseLifePercentage > currentBossLifePercentage)
        {
            currentFase = 2;
            OnSecondPhaseStart?.Invoke();
        }
        else if (currentFase == 2 && thirdPhaseLifePercentage > currentBossLifePercentage)
        {
            currentFase = 3;
            OnThirdPhaseStart?.Invoke();
        }
        else if (currentFase == 3 && fourthPhaseLifePercentage > currentBossLifePercentage)
        {
            currentFase = 4;
            OnFourthPhaseStart?.Invoke();
        }
    }

    private void OnDisable()
    {
        if (bossLifeCtrl != null)
            bossLifeCtrl.OnBossTakeDamage -= HandleOnBossTakeDamage;
    }
}
