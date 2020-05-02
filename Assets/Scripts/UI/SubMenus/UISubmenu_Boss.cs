﻿using System;
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
    private Image bossLifeImageTest1;
    //Slider che mostra la vita del Boss
    [SerializeField]
    private Image bossLifeImageTest2;

    /// <summary>
    /// Riferimento al BossLife Controller
    /// </summary>
    private BossLifeController bossLifeCtrl;
    /// <summary>
    /// Vita massima del boss
    /// </summary>
    private int bossMaxLife;

    private void OnEnable()
    {
        if (bossLifeCtrl != null)
            bossLifeCtrl.OnBossTakeDamage += HandleOnBossTakeDamage;
    }

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_bossCtrl"></param>
    public void Setup(BossControllerBase _bossCtrl)
    {
        bossLifeCtrl = _bossCtrl.GetBossLifeController();
        bossMaxLife = bossLifeCtrl.GetMaxBossLife();
        bossLifeImageTest1.fillAmount = 1;
        bossLifeImageTest2.fillAmount = 1;

        bossLifeCtrl.OnBossTakeDamage += HandleOnBossTakeDamage;
    }

    /// <summary>
    /// Funzione che gestisce l'evento di danno del boss
    /// </summary>
    /// <param name="_currentLife"></param>
    private void HandleOnBossTakeDamage(int _currentLife)
    {
        bossLifeImageTest1.fillAmount = (float)_currentLife / bossMaxLife;
        bossLifeImageTest2.fillAmount = (float)_currentLife / bossMaxLife;
    }

    private void OnDisable()
    {
        if (bossLifeCtrl != null)
            bossLifeCtrl.OnBossTakeDamage -= HandleOnBossTakeDamage;
    }
}
