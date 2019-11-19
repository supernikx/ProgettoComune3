using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il Pannello di Loading
/// </summary>
public class UIMenu_Loading : UIMenu_Base
{
    [Header("Pannel Settings")]
    //Riferimento alla camera di backup attiva durante i caricamenti
    [SerializeField]
    private Camera backupCam;

    /// <summary>
    /// Ovverride della funzione di Toggle del UIMenu_Base
    /// </summary>
    /// <param name="_value"></param>
    public override void ToggleMenu(bool _value)
    {
        base.ToggleMenu(_value);
        backupCam.gameObject.SetActive(_value);
    }
}
