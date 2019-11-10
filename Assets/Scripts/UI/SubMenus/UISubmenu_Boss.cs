using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe che gestisce il pannello del boss
/// </summary>
public class UISubmenu_Boss : MonoBehaviour
{
    [Header("Reference Settings")]
    //Slider che mostra la vita del Boss
    [SerializeField]
    private Slider bossLifeBar;

    /// <summary>
    /// Riferimento al BossLife Controller
    /// </summary>
    private BossLifeController bossLifeCtrl;
    /// <summary>
    /// Vita massima del boss
    /// </summary>
    private int bossMaxLife;

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_bossCtrl"></param>
    public void Setup(BossControllerBase _bossCtrl)
    {
        bossLifeCtrl = _bossCtrl.GetBossLifeController();
        bossMaxLife = bossLifeCtrl.GetMaxBossLife();
        bossLifeBar.value = 1;

        bossLifeCtrl.OnBossTakeDamage += HandleOnBossTakeDamage;
    }

    /// <summary>
    /// Funzione che gestisce l'evento di danno del boss
    /// </summary>
    /// <param name="_currentLife"></param>
    private void HandleOnBossTakeDamage(int _currentLife)
    {
        bossLifeBar.value = (float)_currentLife / bossMaxLife;
    }

    private void OnDisable()
    {
        bossLifeCtrl.OnBossTakeDamage -= HandleOnBossTakeDamage;
    }
}
