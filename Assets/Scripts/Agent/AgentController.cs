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
    public State currentState
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

    /// <summary>
    /// Variabile che identifica il tipo dell'oggetto
    /// </summary>
    private ObjectTypes _objectType;
    public ObjectTypes objectType
    {
        get
        {
            return _objectType;
        }
        set
        {
            _objectType = value;
        }
    }

    /// <summary>
    /// Funzione chiamata al reset forzato nella Pool dell'oggetto
    /// </summary>
    public void ResetPool()
    {
        groupCtrl = null;

        agentGroupCtrl.UnSetup();
        graphicCtrl.UnSetup();
    }
    #endregion

    /// <summary>
    /// Riferimento al Group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento all'agent group controller
    /// </summary>
    private AgentGroupController agentGroupCtrl;
    /// <summary>
    /// Riferimento all'agent movement controller
    /// </summary>
    private AgentMovementController movementCtrl;
    /// <summary>
    /// Riferimento al graphic controller
    /// </summary>
    private AgentGraphicController graphicCtrl;
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
        agentDistanceCtrl = GetComponent<AgentDistanceController>();
        agentGroupCtrl = GetComponent<AgentGroupController>();
        graphicCtrl = GetComponentInChildren<AgentGraphicController>();

        graphicCtrl.Init();
        agentGroupCtrl.Init(this);
    }

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    public void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;

        movementCtrl.Setup();
        agentDistanceCtrl.Setup();
        graphicCtrl.Setup(groupCtrl);
        agentGroupCtrl.Setup(groupCtrl);

        PoolManager.OnObjectSpawnEvent?.Invoke(this);
    }

    /// <summary>
    /// Funzione che esegue l'UnSetup dell'agent
    /// </summary>
    public void UnSetup(bool _death)
    {
        groupCtrl = null;
        if (_death)
            graphicCtrl.SpawnDeathVFX();

        graphicCtrl.UnSetup();
        agentGroupCtrl.UnSetup();

        PoolManager.OnObjectDestroyEvent?.Invoke(this);
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
