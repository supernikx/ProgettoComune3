using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Classe che gestisce il trigger del tutorial
/// </summary>
public class TutorialTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    //Range del trigger
    [SerializeField]
    private float triggerRange;
    //Testo del tutorial
    [SerializeField][Multiline]
    private string tutorialText;
    //Immagine del tutorial
    [SerializeField]
    private Sprite tutorialImage;
    //Input per aprire il pannello di tutorial
    [SerializeField]
    private InputAction inputOpenTutorialPanel;
    //Input per chiudere il pannello di tutorial
    [SerializeField]
    private InputAction inputCloseTutorialPanel;

    /// <summary>
    /// Riferimento al LevelTutorialController
    /// </summary>
    private LevelTutorialController lvlTutorialCtrl;
    /// <summary>
    /// Indica se il gruppo è nel range del trigger
    /// </summary>
    private bool inRange;

    private void OnEnable()
    {
        inputOpenTutorialPanel.Enable();
        inputCloseTutorialPanel.Enable();
    }

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_lvlMng"></param>
    public void Setup(LevelTutorialController _tutorialCtrl)
    {
        lvlTutorialCtrl = _tutorialCtrl;
        inRange = false;
    }

    /// <summary>
    /// Funzione che controlla la distanza del gruppo
    /// </summary>
    /// <param name="_groupPosition"></param>
    public void CheckGroupDistance(Vector3 _groupPosition)
    {
        float groupDistance = Vector3.Distance(transform.position, _groupPosition);
        if (groupDistance <= triggerRange && !inRange)
        {
            inRange = true;
            inputOpenTutorialPanel.started += HandleOnGroupPanelOpen;
        }
        else if (groupDistance > triggerRange && inRange)
        {
            inRange = false;
            inputOpenTutorialPanel.started -= HandleOnGroupPanelOpen;
        }
    }

    #region Handlers
    /// <summary>
    /// Funzione che gestisce l'evento del tasto di apertura pannello premuto
    /// </summary>
    /// <param name="_context"></param>
    private void HandleOnGroupPanelOpen(InputAction.CallbackContext _context)
    {
        inputOpenTutorialPanel.started -= HandleOnGroupPanelOpen;
        inputOpenTutorialPanel.canceled += HandleOnGroupPanelOpenCancelled;

        lvlTutorialCtrl.TutorialTriggerOpen(this);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di tasto di apertura pannello rilasciato
    /// </summary>
    /// <param name="_context"></param>
    private void HandleOnGroupPanelOpenCancelled(InputAction.CallbackContext _context) 
    {
        inputOpenTutorialPanel.canceled -= HandleOnGroupPanelOpenCancelled;
        inputCloseTutorialPanel.started += HandleOnGroupPanelClosed;
    }

    /// <summary>
    /// Funzione che gestisce l'evento del tasto di chiusura pannello premuto
    /// </summary>
    /// <param name="_context"></param>
    private void HandleOnGroupPanelClosed(InputAction.CallbackContext _context)
    {
        inputCloseTutorialPanel.started -= HandleOnGroupPanelClosed;
        inputCloseTutorialPanel.canceled += HandleOnGroupPanelClosedCancelled;

        lvlTutorialCtrl.TutorialTriggerClose(this);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di tasto di chiusura pannello rilasciato
    /// </summary>
    /// <param name="_context"></param>
    private void HandleOnGroupPanelClosedCancelled(InputAction.CallbackContext _context)
    {
        inputCloseTutorialPanel.canceled -= HandleOnGroupPanelClosedCancelled;
        inputOpenTutorialPanel.started += HandleOnGroupPanelOpen;
    }
    #endregion


    #region API
    #region Getter
    /// <summary>
    /// Funzione che ritorna lo sprite del tutorial
    /// </summary>
    /// <returns></returns>
    public Sprite GetTutorialSprite()
    {
        return tutorialImage;
    }

    /// <summary>
    /// Funzione che ritorna il testo del tutorial
    /// </summary>
    /// <returns></returns>
    public string GetTutorialText()
    {
        return tutorialText;
    }
    #endregion
    #endregion

    #region Debug
    /// <summary>
    /// Gizmo
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, triggerRange);
    }
    #endregion

    private void OnDisable()
    {
        inputOpenTutorialPanel.Disable();
        inputCloseTutorialPanel.Disable();
    }

}
