using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce l'agent e fornisce il punto di accesso agli altri controller dell'agent
/// </summary>
public class AgentController : MonoBehaviour, IPoolObject
{
    #region Pool Interface
    /// <summary>
    /// Evento che toglie dalla Pool l'agent
    /// </summary>
    public event PoolManagerEvets.Events OnObjectSpawn;
    /// <summary>
    /// Evento che rimette in Pool l'agent
    /// </summary>
    public event PoolManagerEvets.Events OnObjectDestroy;

    /// <summary>
    /// Variabile che identifica l'owner dell'agent
    /// </summary>
    private GameObject _ownerObject;
    public GameObject ownerObject
    {
        get
        {
            return _ownerObject;
        }
        set
        {
            _ownerObject = value;
        }
    }

    /// <summary>
    /// Variabile che identifica lo stato della Pool dell'agent
    /// </summary>
    private State _CurrentState;
    public State CurrentState
    {
        get
        {
            return _CurrentState;
        }
        set
        {
            _CurrentState = value;
        }
    }
    #endregion

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

    #region Setup
    /// <summary>
    /// Funzione chiamata allo spawn in Pool dell'oggetto
    /// </summary>
    public void PoolInit()
    {
        movementCtrl = GetComponent<AgentMovementController>();
        agentJumpCtrl = GetComponent<AgentJumpController>();
        agentGravityCtrl = GetComponent<AgentGravityController>();
        agentCollisionCtrl = GetComponent<AgentCollisionController>();
        agentDistanceCtrl = GetComponent<AgentDistanceController>();
        graphicCtrl = GetComponentInChildren<AgentGraphicController>();

        graphicCtrl.Init(this);
        agentCollisionCtrl.Init(this);
        agentJumpCtrl.Init(this);
        agentGravityCtrl.Init(this);
    }

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    public void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;

        movementCtrl.Setup();
        agentCollisionCtrl.Setup();
        agentJumpCtrl.Setup();
        agentGravityCtrl.Setup();
        agentDistanceCtrl.Setup();
        graphicCtrl.Setup();

        OnObjectSpawn?.Invoke(this);
    }

    /// <summary>
    /// Funzione che esegue l'UnSetup dell'agent
    /// </summary>
    public void UnSetup()
    {
        groupCtrl = null;

        agentCollisionCtrl.UnSetup();
        agentJumpCtrl.UnSetup();
        agentGravityCtrl.UnSetup();
        graphicCtrl.UnSetup();

        OnObjectDestroy?.Invoke(this);
    }
    #endregion

    #region API
    #region Getter
    /// <summary>
    /// Funzione che ritorna il group controller
    /// </summary>
    /// <returns></returns>
    public GroupController GetGroupController()
    {
        return groupCtrl;
    }

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
