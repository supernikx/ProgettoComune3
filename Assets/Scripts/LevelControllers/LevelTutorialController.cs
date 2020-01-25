using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce i trigger del tutorial
/// </summary>
public class LevelTutorialController : MonoBehaviour
{
    #region Actions
    /// <summary>
    /// Evento lanciato quando un pannello tutorial viene attivato
    /// </summary>
    public static Action<TutorialTrigger> OnTutorialOpen;
    /// <summary>
    /// Evento lanciato quando un pannello tutorial viene chiuso
    /// </summary>
    public static Action<TutorialTrigger> OnTutorialClose;
    #endregion

    [Header("Tutorial Settings")]
    //Lista di trigger
    [SerializeField]
    private List<TutorialTrigger> triggersList;

    /// <summary>
    /// Riferimento al level manager
    /// </summary>
    private LevelManager lvlMng;
    /// <summary>
    /// Riferimento al GroupController
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Bool che identifica se la classe deve controllare la distanza del gruppo rispetto ai trigger
    /// </summary>
    private bool checkTriggers;
    /// <summary>
    /// Bool che identifica se la classe è setuppata o no
    /// </summary>
    private bool isSetupped = false;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_lvlMng"></param>
    public void Setup(LevelManager _lvlMng)
    {
        lvlMng = _lvlMng;
        groupCtrl = lvlMng.GetGroupController();

        SetupTutorialTriggers();

        checkTriggers = true;
        isSetupped = true;
    }

    /// <summary>
    /// Funzione che esegue il Setup dei trigger
    /// </summary>
    private void SetupTutorialTriggers()
    {
        for (int i = 0; i < triggersList.Count; i++)
        {
            triggersList[i].Setup(this);
        }
    }

    private void Update()
    {
        if (isSetupped && checkTriggers)
        {
            CheckTriggers();    
        }
    }

    /// <summary>
    /// Funzione che controlla i trigger
    /// </summary>
    private void CheckTriggers()
    {
        Vector3 groupCenter = groupCtrl.GetGroupCenterPoint();
        for (int i = 0; i < triggersList.Count; i++)
            triggersList[i].CheckGroupDistance(groupCenter);
    }

    #region API
    /// <summary>
    /// Funzione che notifica che è stato triggerato un tutorial
    /// </summary>
    /// <param name="_trigger"></param>
    public void TutorialTriggerOpen(TutorialTrigger _trigger)
    {
        checkTriggers = false;
        groupCtrl.Enable(false);

        OnTutorialOpen?.Invoke(_trigger);
    }

    /// <summary>
    /// Funzione che notifica che è stato chiuso un tutorial
    /// </summary>
    /// <param name="_trigger"></param>
    public void TutorialTriggerClose(TutorialTrigger _trigger)
    {
        checkTriggers = true;
        groupCtrl.Enable(true);

        OnTutorialClose?.Invoke(_trigger);
    }
    #endregion
}
