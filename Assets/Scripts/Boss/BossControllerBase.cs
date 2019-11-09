using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe astratta base di tutti i boss
/// </summary>
public abstract class BossControllerBase : MonoBehaviour
{
    /// <summary>
    /// Funzione di Setup
    /// </summary>
    public abstract void Setup(LevelManager _lvlMng);

    /// <summary>
    /// Funzione che attiva il boss
    /// </summary>
    public abstract void StartBoss();

    /// <summary>
    /// Funzione che stoppa il boss
    /// </summary>
    public abstract void StopBoss();
}
