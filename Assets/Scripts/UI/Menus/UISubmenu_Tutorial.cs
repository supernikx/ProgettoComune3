using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Classe che gestisce il pannello del tutorial della UI
/// </summary>
public class UISubmenu_Tutorial : MonoBehaviour
{
    [Header("Subpanel Settings")]
    //Riferimento al TextMeshPro che conterrà il testo del tutorial
    [SerializeField]
    private TextMeshProUGUI tutorialText;
    //Riferimento all'immagine che conterrà l'immagine del tutorial
    [SerializeField]
    private Image tutorialImage;

    /// <summary>
    /// Funzione di Setup del Pannello
    /// </summary>
    /// <param name="_tutorialImage"></param>
    /// <param name="_tutorialText"></param>
    public void SetupTutorialPanel(Sprite _tutorialImage, string _tutorialText)
    {
        tutorialText.text = _tutorialText;
        tutorialImage.sprite = _tutorialImage;
    }
}
