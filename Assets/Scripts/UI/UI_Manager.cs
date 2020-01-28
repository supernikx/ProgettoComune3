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
    [SerializeField]
    private UI_Controller defaultUICtrl;

    private GameManager gm;
    private UI_Controller currentUICtrl;

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

    public void Setup(GameManager _gm)
    {
        gm = _gm;
        currentUICtrl = defaultUICtrl;
        currentUICtrl.Setup(gm);

        Init();
    }

    #region API
    public void SetDefaultController()
    {
        currentUICtrl = defaultUICtrl;
    }

    #region Getter
    public UI_Controller GetCurrentUIController()
    {
        return currentUICtrl;
    }
    #endregion
    #endregion
}
