using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Classe che gestisce le UI
/// </summary>
public class UI_Manager : MonoBehaviour
{
    [Header("General Settings")]
    //Riferimento all'ui controller di default
    [SerializeField]
    private UI_Controller defaultUICtrl;

    /// <summary>
    /// Riferimento al GameManager
    /// </summary>
    private GameManager gm;
    /// <summary>
    /// Riferimento all'ui controller attivo
    /// </summary>
    private UI_Controller currentUICtrl;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_gm"></param>
    public void Setup(GameManager _gm)
    {
        gm = _gm;
        currentUICtrl = defaultUICtrl;
        currentUICtrl.Setup(gm);

        Init();
    }

    /// <summary>
    /// Funzione di inizializzione
    /// </summary>
    public void Init()
    {
        currentUICtrl.ClearCurrentMenu();

        UI_Controller[] uiCtrls = FindObjectsOfType<UI_Controller>();
        for (int i = 0; i < uiCtrls.Length; i++)
        {
            if (uiCtrls[i] != defaultUICtrl)
            {
                currentUICtrl = uiCtrls[i];
                break;
            }
        }

        if (currentUICtrl != null)
            currentUICtrl.Setup(gm);
        else
            currentUICtrl = defaultUICtrl;
    }

    #region API
    /// <summary>
    /// Funzione che imposta il default controller come attivo
    /// </summary>
    public void SetDefaultController()
    {
        currentUICtrl = defaultUICtrl;
    }

    #region Getter
    /// <summary>
    /// Funzione che ritorna l'attuale ui controller attivo
    /// </summary>
    /// <returns></returns>
    public UI_Controller GetCurrentUIController()
    {
        return currentUICtrl;
    }
    #endregion
    #endregion
}
