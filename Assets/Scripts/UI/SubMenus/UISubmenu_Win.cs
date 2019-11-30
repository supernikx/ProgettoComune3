using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Funzione che controlla il pannello di vittoria
/// </summary>
public class UISubmenu_Win : MonoBehaviour
{
    [Header("Subanel Settings")]
    //Durata del Pannello
    [SerializeField]
    private float panelDuration;

    /// <summary>
    /// Riferimento alla coroutine di despawn del pannello
    /// </summary>
    private IEnumerator panelDespawnRoutine;

    private void OnEnable()
    {
        panelDespawnRoutine = PanelDespawnCoroutine();
        StartCoroutine(panelDespawnRoutine);
    }

    /// <summary>
    /// Coroutine che spegne il pannello dopo il tempo passato come parametro
    /// </summary>
    /// <returns></returns>
    private IEnumerator PanelDespawnCoroutine()
    {
        yield return new WaitForSeconds(panelDuration);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (panelDespawnRoutine != null)
            StopCoroutine(panelDespawnRoutine);
    }
}
