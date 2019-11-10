using System;
using UnityEngine;

/// <summary>
/// Classe astratta base di tutti i boss
/// </summary>
public abstract class BossControllerBase : MonoBehaviour
{
    #region Action
    /// <summary>
    /// Evento che notifica la morte del Boss
    /// </summary>
    public Action<BossControllerBase> OnBossDead;
    #endregion

    /// <summary>
    /// Riferimento al BossLife Controller
    /// </summary>
    protected BossLifeController lifeCtrl;
    /// <summary>
    /// Riferimento al level manager
    /// </summary>
    protected LevelManager lvlMng;
    /// <summary>
    /// Bool che identifica se lo script è setuppato
    /// </summary>
    protected bool isSetupped;
    /// <summary>
    /// Bool che identifica se lo script è attivo
    /// </summary>
    protected bool isEnabled;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    public virtual void Setup(LevelManager _lvlMng)
    {
        lvlMng = _lvlMng;
        lifeCtrl = GetComponent<BossLifeController>();
        isSetupped = true;
    }

    #region API
    /// <summary>
    /// Funzione che attiva il boss
    /// </summary>
    public virtual void StartBoss()
    {
        isEnabled = true;
    }

    /// <summary>
    /// Funzione che stoppa il boss
    /// </summary>
    public virtual void StopBoss()
    {
        isEnabled = false;
    }

    /// <summary>
    /// FUnzione che ritorna se lo script è setuppato
    /// </summary>
    /// <returns></returns>
    public virtual bool IsSetupped()
    {
        return isSetupped;
    }

    /// <summary>
    /// Funzione che ritorna se lo script è attivo
    /// </summary>
    /// <returns></returns>
    public virtual bool IsEnabled()
    {
        return isEnabled;
    }

    /// <summary>
    /// Funzione che ritorna se lo script è setuppato e attivo
    /// </summary>
    /// <returns></returns>
    public virtual bool IsSetuppedAndEnabled()
    {
        return isSetupped && isEnabled;
    }

    #region Getter
    /// <summary>
    /// Funzione che ritorna il LifeController
    /// </summary>
    /// <returns></returns>
    public virtual BossLifeController GetBossLifeController()
    {
        return lifeCtrl;
    }
    #endregion
    #endregion
}
