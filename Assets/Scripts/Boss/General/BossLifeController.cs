using System;
using UnityEngine;
using TMPro;

/// <summary>
/// Classe che gestisce la vita del Boss
/// </summary>
public class BossLifeController : MonoBehaviour
{
    #region Action
    /// <summary>
    /// Evento che notifica la morte del Boss
    /// </summary>
    public Action OnBossDead;
    /// <summary>
    /// Evento che notifica la vita rimanente del Boss
    /// </summary>
    public Action<int> OnBossTakeDamage;
    #endregion

    [Header("Life Settings")]
    //Vita iniziale del Boss
    [SerializeField]
    private int bossStartLife;

    /// <summary>
    /// Riferimento al boss controller
    /// </summary>
    private BossControllerBase bossCtrl;
    /// <summary>
    /// Vita attuale del boss
    /// </summary>
    private int currentLife;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    public void Setup(BossControllerBase _bossCtrl)
    {
        bossCtrl = _bossCtrl;
        currentLife = bossStartLife;
    }

    #region API
    /// <summary>
    /// Funzione che toglie vita al boss
    /// </summary>
    /// <param name="_damage"></param>
    public void TakeDamage(int _damage)
    {
        if (!bossCtrl.IsSetuppedAndEnabled())
            return;

        currentLife = Mathf.Clamp(currentLife - _damage, 0, bossStartLife);
        OnBossTakeDamage?.Invoke(currentLife);

        if (currentLife == 0)
            OnBossDead?.Invoke();
    }

    #region Getter
    /// <summary>
    /// Funzione che ritorna la vita massima del boss
    /// </summary>
    /// <returns></returns>
    public int GetMaxBossLife()
    {
        return bossStartLife;
    }

    /// <summary>
    /// Funzione che ritorna la vita attuale del boss
    /// </summary>
    /// <returns></returns>
    public int GetCurrentBossLife()
    {
        return currentLife;
    }
    #endregion
    #endregion
}
