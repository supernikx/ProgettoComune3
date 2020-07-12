using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Classe che controlla il pannello di fine demo
/// </summary>
public class UISubmenu_EndDemo : MonoBehaviour
{
    [Header("Subanel Settings")]
    //Durata del Pannello
    [SerializeField]
    private Button button;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

    /// <summary>
    /// Funzione chiamata al click del panel
    /// </summary>
    public void OnPanelClick()
    {
        gameObject.SetActive(false);
    }
}
