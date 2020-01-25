using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Binding Settings")]
    //Mappatura dei tasti per la pausa
    [SerializeField]
    private InputAction inputPauseMapping;

    /// <summary>
    /// Bool che identifica se lo script è setuppato
    /// </summary>
    private bool isSetupped = true;
    /// <summary>
    /// Bool che identifica se il gioco è in pausa
    /// </summary>
    private bool isPaused;

    private void OnEnable()
    {
        inputPauseMapping.Enable();
    }

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    public void Setup()
    {
        inputPauseMapping.performed += OnPause;
        isPaused = false;
        isSetupped = true;
    }

    /// <summary>
    /// Funzione chiamata alla pressione del tasto di pausa
    /// </summary>
    public void OnPause(InputAction.CallbackContext _context)
    {
        SetPause(!isPaused);
    }

    #region API
    #region Setter
    /// <summary>
    /// Funzione che imposta la pausa
    /// </summary>
    /// <param name="_pause"></param>
    public void SetPause(bool _pause)
    {
        isPaused = _pause;
        if (isPaused)
            OnGamePause?.Invoke();
        else
            OnGameUnpause?.Invoke();
    }
    #endregion

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

    private void OnDisable()
    {
        inputPauseMapping.performed -= OnPause;
        inputPauseMapping.Disable();
    }
}
