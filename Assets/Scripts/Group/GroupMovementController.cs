using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// bool che indentifica se la classe è setuppata
    /// </summary>
    private bool isSetupped = false;
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
        canMove = isSetupped = true;
    }

    private void Update()
    {
        if (!isSetupped || !canMove)
            return;

        ReadInput();
    }

    private void FixedUpdate()
    {
        if (!isSetupped || !canMove)
            return;

        MoveAgents();
    }

    /// <summary>
    /// Funzione che si occupa di leggere gl'input
    /// </summary>
    private void ReadInput()
    {
        movementVector.x = Input.GetAxis("Horizontal");
        movementVector.z = Input.GetAxis("Vertical");
        movementVector.y = Input.GetButton("Jump") ? 1 : 0;
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
