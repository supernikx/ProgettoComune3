using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe che gestisce il Pannello di Gameplay
/// </summary>
public class UIMenu_Gameplay : UIControllerBase
{
    [Header("References")]
    //Riferimento all'immagine che deve apparire nel momento della ricarica
    [SerializeField]
    private Image reloadingImage;

    /// <summary>
    /// Riferimento al group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al group shoot controller
    /// </summary>
    private GroupShootController groupShootCtrl;
    /// <summary>
    /// Tempo massimo di ricarica
    /// </summary>
    private float maxReloadingTime;

    /// <summary>
    /// Override funzione che esegue il setup del pannello
    /// </summary>
    public override void CustomSetup()
    {
        base.CustomSetup();
        reloadingImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// Override funzione che gestisce accensione/spegnimento menù
    /// </summary>
    /// <param name="_value"></param>
    public override void ToggleMenu(bool _value)
    {
        base.ToggleMenu(_value);

        if (_value)
        {
            groupCtrl = manager.GetGameManager().GetLevelManager().GetGroupController();
            groupShootCtrl = groupCtrl.GetGroupShootController();

            groupShootCtrl.OnReloadingStart += HandleOnReloadingStart;
            groupShootCtrl.OnReloading += HandleOnReloading;
            groupShootCtrl.OnReloadingEnd += HandleOnReloadingEnd;
        }
        else if (groupCtrl != null)
        {
            groupShootCtrl.OnReloadingStart -= HandleOnReloadingStart;
            groupShootCtrl.OnReloading -= HandleOnReloading;
            groupShootCtrl.OnReloadingEnd -= HandleOnReloadingEnd;
            groupCtrl = null;
        }
    }

    #region Handles
    /// <summary>
    /// Funzione che gestisce l'evento di inizio ricarica
    /// </summary>
    /// <param name="_reloadingTime"></param>
    private void HandleOnReloadingStart(float _reloadingTime)
    {
        maxReloadingTime = _reloadingTime;
        reloadingImage.fillAmount = 0;
        reloadingImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di ricarica in corso
    /// </summary>
    /// <param name="_passReloadingTime"></param>
    private void HandleOnReloading(float _passReloadingTime)
    {
        reloadingImage.fillAmount = _passReloadingTime / maxReloadingTime;
    }

    /// <summary>
    /// Funzione che gestisce l'evento di fine ricarica
    /// </summary>
    private void HandleOnReloadingEnd()
    {
        maxReloadingTime = 0;
        reloadingImage.gameObject.SetActive(false);
    }
    #endregion
}
