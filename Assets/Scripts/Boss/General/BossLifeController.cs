using System;
using UnityEngine;
using TMPro;

/// <summary>
/// Classe che gestisce la vita del Boss
/// </summary>
public class BossLifeController : MonoBehaviour
{
    #region Action
    public Action OnBossDead;
    #endregion

    [Header("Life Settings")]
    //Vita iniziale del Boss
    [SerializeField]
    private int bossStartLife;

    [Header("Debug")]
    [SerializeField]
    private TextMeshProUGUI lifeText;

    /// <summary>
    /// Vita attuale del boss
    /// </summary>
    private int _currentLife;
    public int currentLife
    {
        get
        {
            return _currentLife;
        }
        set
        {
            _currentLife = value;
            lifeText.text = "BossLife: " + _currentLife.ToString();
        }
    }

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    public void Setup()
    {
        currentLife = bossStartLife;
    }

    /// <summary>
    /// Funzione che toglie vita al boss
    /// </summary>
    /// <param name="_damage"></param>
    public void TakeDamage(int _damage)
    {
        currentLife = Mathf.Clamp(currentLife - _damage, 0, bossStartLife);
        if (currentLife == 0)
            OnBossDead?.Invoke();
    }
}
