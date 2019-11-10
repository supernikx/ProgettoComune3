using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Classe che gestisce gl'input del gruppo
/// </summary>
public class GroupMovementController : MonoBehaviour
{
    /// <summary>
    /// Rifeirmento al Group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al vettore di movimento
    /// </summary>
    private Vector3 movementVector;
    /// <summary>
    /// bool che identifica se il gruppo può muoversi
    /// </summary>
    private bool canMove = false;

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    public void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;
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
    /// Funzione chiamata alla pressione del tasto jump dal PlayerInput
    /// </summary>
    public void OnJump()
    {
        movementVector.y = 1;
    }

    /// <summary>
    /// Funzione che si occupa di muovere tutti gli agent
    /// </summary>
    private void MoveAgents()
    {
        List<AgentController> agents = groupCtrl.GetAgents();
        if (agents == null || agents.Count == 0)
            return;

        //Salto
        if (movementVector.y == 1)
        {
            foreach (AgentController agent in agents)
                agent.GetAgentJumpController().Jump();
        }
        movementVector.y = 0;

        //Movimento
        if (movementVector == Vector3.zero)
            return;

        foreach (AgentController agent in agents)
            agent.GetAgentMovementController().Move(movementVector.normalized, true);
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
    #endregion
}
