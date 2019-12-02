using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Classe che gestisce gl'input del gruppo
/// </summary>
public class GroupMovementController : MonoBehaviour
{
    #region Actions
    /// <summary>
    /// Evento che notifica che il gruppo si sta muovendo
    /// </summary>
    public Action OnGroupMove;
    #endregion

    [Header("Group Movement Settings")]
    //Variabile che identifica il moltiplicatore della velocità se il gruppo è in raggruppamento
    [SerializeField]
    private float groupSpeedMultiplier;

    /// <summary>
    /// Rifeirmento al Group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Rifeirmento al Group Size Controller
    /// </summary>
    private GroupSizeController sizeCtrl;
    /// <summary>
    /// Riferimento al vettore di movimento
    /// </summary>
    private Vector3 movementVector;
    /// <summary>
    /// bool che identifica se il gruppo può muoversi
    /// </summary>
    private bool canMove = false;
    /// <summary>
    /// bool che identifica se il gruppo si sta raggruppando
    /// </summary>
    private bool grouping = false;

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    public void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;
        sizeCtrl = groupCtrl.GetGroupSizeController();

        sizeCtrl.OnGroupPressed += HandleOnGroupPressed;
        canMove = true;
    }

    private void Update()
    {
        if (!groupCtrl.IsSetuppedAndEnabled() || !canMove)
            return;

        MoveAgents();
    }

    /// <summary>
    /// Funzione chiamata al movimento dal PlayerInput
    /// </summary>
    public void OnMove(InputValue _value)
    {
        Vector2 newMove = _value.Get<Vector2>();
        movementVector.x = newMove.x;
        movementVector.z = newMove.y;
    }

    /// <summary>
    /// Funzione che si occupa di muovere tutti gli agent
    /// </summary>
    private void MoveAgents()
    {
        List<AgentController> agents = groupCtrl.GetAgents();
        if (agents == null || agents.Count == 0)
            return;

        //Movimento
        if (movementVector == Vector3.zero)
            return;

        float speedMultiplier = grouping ? groupSpeedMultiplier : 1;
        foreach (AgentController agent in agents)
            agent.GetAgentMovementController().Move(movementVector.normalized, true, speedMultiplier);

        OnGroupMove?.Invoke();
    }

    /// <summary>
    /// Funzione che gestisce l'evento di raggruppamento
    /// </summary>
    /// <param name="_value"></param>
    private void HandleOnGroupPressed(bool _value)
    {
        grouping = _value;
    }

    #region API
    #region Setter
    /// <summary>
    /// Funzione che imposta la variabile can move con il valore passato come parametro
    /// </summary>
    /// <param name="_canMove"></param>
    public void SetCanMove(bool _canMove)
    {
        canMove = _canMove;
    }
    #endregion

    /// <summary>
    /// Funzione che resetta il vettore di moviemento a 0
    /// </summary>
    public void ResetMovementVelocity()
    {
        movementVector = Vector3.zero;
    }
    #endregion

    private void OnDisable()
    {
        if (sizeCtrl != null)
            sizeCtrl.OnGroupPressed += HandleOnGroupPressed;
    }
}
