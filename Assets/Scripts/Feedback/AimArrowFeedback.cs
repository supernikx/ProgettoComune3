using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che si occupa di controllare il feedback della freccia di mira
/// </summary>
public class AimArrowFeedback : MonoBehaviour
{
    /// <summary>
    /// Classe che combina sprite e colori della freccia
    /// </summary>
    [System.Serializable]
    private class ArrowGraphic
    {
        /// <summary>
        /// Sprite della freccia
        /// </summary>
        public Sprite arrowSprite;
        /// <summary>
        ///Colore della freccia 
        /// </summary>
        public Color arrowColor;
    }

    [Header("Arrow Settings")]
    //Dati della freccia col gruppo pieno
    [SerializeField]
    private ArrowGraphic fullGrupArrow;
    //Dati della freccia col gruppo nè pieno nè vuoto
    [SerializeField]
    private ArrowGraphic normalGroupArrow;
    //Dati della freccia col gruppo vuoto
    [SerializeField]
    private ArrowGraphic emptyGroupArrow;

    /// <summary>
    /// Riferimento al Group Controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento allo sprite renderer
    /// </summary>
    private SpriteRenderer spriteRenderer;
    /// <summary>
    /// Membri massimi del gruppo
    /// </summary>
    private int groupMaxAgents;
    /// <summary>
    /// Membri minimi del gruppo
    /// </summary>
    private int groupMinAgents;

    /// <summary>
    /// Funzione di  Setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    internal void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        groupMaxAgents = groupCtrl.GetGroupMaxAgentCont();
        groupMinAgents = groupCtrl.GetGroupMinAgentCont();

        groupCtrl.OnAgentSpawn += HandleOnGroupSizeChange;
        groupCtrl.OnAgentDead += HandleOnGroupSizeChange;
    }

    #region Handlers
    /// <summary>
    /// Funzione che gestisce l'evento di spawn e morte di un nuovo agent
    /// </summary>
    /// <param name="_agentCtrl"></param>
    private void HandleOnGroupSizeChange(AgentController _agentCtrl)
    {
        UpdateArrowColor(groupCtrl.GetGroupCont());
    }
    #endregion

    #region API
    /// <summary>
    /// Funzione che aggiorna la posizione e l'orientamento della freccia
    /// </summary>
    /// <param name="_groupCenterPosition"></param>
    /// <param name="_aimDirection"></param>
    public void UpdateArrow(Vector3 _groupCenterPosition, Vector3 _aimDirection)
    {
        //Prima lo sposto
        transform.position = _groupCenterPosition;
        Vector3 directionToLook = _aimDirection;
        directionToLook.y = 0f;
        if (directionToLook != Vector3.zero)
            transform.rotation = Quaternion.AngleAxis(-90f, Vector3.up) * Quaternion.LookRotation(directionToLook);
    }

    /// <summary>
    /// Funzione che controlla e aggiorna il colore della freccia
    /// </summary>
    /// <param name="_currentGroupCount"></param>
    public void UpdateArrowColor(int _currentGroupCount)
    {
        if (_currentGroupCount <= groupMinAgents)
        {
            spriteRenderer.sprite = emptyGroupArrow.arrowSprite;
            spriteRenderer.color = emptyGroupArrow.arrowColor;
            return;
        }

        if (_currentGroupCount > groupMinAgents && _currentGroupCount < groupMaxAgents)
        {
            spriteRenderer.sprite = normalGroupArrow.arrowSprite;
            spriteRenderer.color = normalGroupArrow.arrowColor;
            return;
        }

        if (_currentGroupCount >= groupMaxAgents)
        {
            spriteRenderer.sprite = fullGrupArrow.arrowSprite;
            spriteRenderer.color = fullGrupArrow.arrowColor;
            return;
        }
    }
    #endregion

    private void OnDisable()
    {
        if (groupCtrl != null)
        {
            groupCtrl.OnAgentSpawn -= HandleOnGroupSizeChange;
            groupCtrl.OnAgentDead -= HandleOnGroupSizeChange;
        }
    }
}
