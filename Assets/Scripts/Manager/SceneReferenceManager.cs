using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce le referenze delle scene principali
/// </summary>
public class SceneReferenceManager : MonoBehaviour
{
    [Header("Scene References")]
    //Nome della scena di MainMenu
    [SerializeField]
    private string mainMenuSceneName;
    //Nome della scena di Swarm
    [SerializeField]
    private string swarmSceneName;
    //Nome della scena di Hub
    [SerializeField]
    private string hubSceneName;

    #region API
    #region Getter
    /// <summary>
    /// Funzione che ritorna il nome della scena di MainMenu
    /// </summary>
    /// <returns></returns>
    public string GetMainMenuSceneName()
    {
        return mainMenuSceneName;
    }

    /// <summary>
    /// Funzione che ritorna il nome della scena Swarm
    /// </summary>
    /// <returns></returns>
    public string GetSwarmSceneName()
    {
        return swarmSceneName;
    }

    /// <summary>
    /// Funzione che ritorna il nome della scena Hub
    /// </summary>
    /// <returns></returns>
    public string GetHubSceneName()
    {
        return hubSceneName;
    }
    #endregion
    #endregion
}
