using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce l'agent e fornisce il punto di accesso agli altri controller dell'agent
/// </summary>
public class AgentController : MonoBehaviour
{
    /// <summary>
    /// Riferimento al Group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento all'agent movement controller
    /// </summary>
    private AgentMovementController movementCtrl;
    /// <summary>
    /// Riferimento al graphic controller
    /// </summary>
    private AgentGraphicController graphicCtrl;
    /// <summary>
    /// Riferimento all'agent collision controller
    /// </summary>
    private AgentCollisionController agentCollisionCtrl;
    /// <summary>
    /// Riferimento all'agent jump controller
    /// </summary>
    private AgentJumpController agentJumpCtrl;
    /// <summary>
    /// Riferimento all'agent gravity controller
    /// </summary>
    private AgentGravityController agentGravityCtrl;
    /// <summary>
    /// Riferimento all'agent distance controller
    /// </summary>
    private AgentDistanceController agentDistanceCtrl;

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    public void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;
        movementCtrl = GetComponent<AgentMovementController>();
        agentJumpCtrl = GetComponent<AgentJumpController>();
        agentGravityCtrl = GetComponent<AgentGravityController>();
        agentCollisionCtrl = GetComponent<AgentCollisionController>();
        agentDistanceCtrl = GetComponent<AgentDistanceController>();
        graphicCtrl = GetComponentInChildren<AgentGraphicController>();

        movementCtrl.Setup(this);
        agentCollisionCtrl.Setup(this);
        agentJumpCtrl.Setup(this);
        agentGravityCtrl.Setup(this);
        agentDistanceCtrl.Setup();
        graphicCtrl.Setup();
    }

    #region API
    #region Getter
    /// <summary>
    /// Funzioe che ritorna il movement controller dell'agent
    /// </summary>
    /// <returns></returns>
    public AgentMovementController GetAgentMovementController()
    {
        return movementCtrl;
    }

    /// <summary>
    /// Funzione che ritorna l'agent collision controller
    /// </summary>
    /// <returns></returns>
    public AgentCollisionController GetAgentCollisionController()
    {
        return agentCollisionCtrl;
    }

    /// <summary>
    /// Funzione che ritorna l'agent jump controller
    /// </summary>
    /// <returns></returns>
    public AgentJumpController GetAgentJumpController()
    {
        return agentJumpCtrl;
    }

    /// <summary>
    /// Funzione che ritorna l'agent distance controller
    /// </summary>
    /// <returns></returns>
    public AgentDistanceController GetAgentDistanceController()
    {
        return agentDistanceCtrl;
    }
    #endregion
    #endregion
}
