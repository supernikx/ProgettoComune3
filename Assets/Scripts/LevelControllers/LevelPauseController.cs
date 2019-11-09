using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce la pausa
/// </summary>
public class LevelPauseController : MonoBehaviour
{
    #region Actions
    /// <summary>
    /// Evento che notifica l'inizio della pausa
    /// </summary>
    public Action OnGamePause;
    /// <summary>
    /// Evento che notifica la fine della pausa
    /// </summary>
    public Action OnGameUnpause;
    #endregion

    /// <summary>
    /// Bool che identifica se lo script è setuppato
    /// </summary>
    private bool isSetupped = true;
    /// <summary>
    /// Bool che identifica se il gioco è in pausa
    /// </summary>
    private bool isPaused;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    public void Setup()
    {
        isPaused = false;
        isSetupped = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
                OnGamePause?.Invoke();
            else
                OnGameUnpause?.Invoke();
        }
    }

    #region API
    #region Getter
    /// <summary>
    /// Funzione che ritorna se il gioco in pausa
    /// </summary>
    /// <returns></returns>
    public bool IsPaused()
    {
        return isPaused;
    }
    #endregion
    #endregion
}
