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
    /// Evento che notifica l'inizio della terza fase
    /// </summary>
    public Action OnThirdPhaseStart;
    #endregion

    [Header("Phases Settings")]
    //Percentuale di vita per attivare la terza fase
    [SerializeField]
    private float thirdPhaseLifePercentage;

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
    /// identifica se il Boss si trova nella terza fase
    /// </summary>
    private bool thirdPhase;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_bossCtrl"></param>
    public void Setup(Boss2Controller _bossCtrl)
    {
        bossCtrl = _bossCtrl;
        bossLifeCtrl = bossCtrl.GetBossLifeController();
        maxBossLife = bossLifeCtrl.GetMaxBossLife();
        thirdPhase = false;

        bossLifeCtrl.OnBossTakeDamage += HandleOnBossTakeDamage;
    }

    /// <summary>
    /// Funzione che gestisce l'evento di danno del BossLifeController
    /// </summary>
    /// <param name="_currentLife"></param>
    private void HandleOnBossTakeDamage(int _currentLife)
    {
        float currentBossLifePercentage = (_currentLife * 100) / maxBossLife;

        if (!thirdPhase && thirdPhaseLifePercentage > currentBossLifePercentage)
        {
            thirdPhase = true;
            OnThirdPhaseStart?.Invoke();
        }
    }

    private void OnDisable()
    {
        if (bossLifeCtrl != null)
            bossLifeCtrl.OnBossTakeDamage -= HandleOnBossTakeDamage;
    }
}
