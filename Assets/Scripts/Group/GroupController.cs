using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il gruppo e fornisce il punto di accesso agli altri controller del gruppo
/// </summary>
public class GroupController : MonoBehaviour
{
    [Header("Group Spawn Settings")]
    //Agenti iniziali del gruppo
    [SerializeField]
    private int groupStartAgents;
    //Range di spawn rispetto alla posizione del group
    [SerializeField]
    private float spawnRange;
    //Riferimento al prefab dell'agent
    [SerializeField]
    private AgentController agentPrefab;

    [Header("Group Settings")]
    //Numero massimo di agents che possono essere presenti
    [SerializeField]
    private int groupMaxAgents;
    //Numero minimo di agents che possono essere presenti
    [SerializeField]
    private int groupMinAgents;

    //Lista di agent che formano il guppo
    private List<AgentController> agents;
    /// <summary>
    /// Riferimento al group movement controller
    /// </summary>
    private GroupMovementController groupMovementCtrl;
    /// <summary>
    /// Riferimento al shoot controller
    /// </summary>
    private GroupShootController shootCtrl;

    #region Setup
    /// <summary>
    /// Funzione che esegue il setup
    /// </summary>
    public void Setup()
    {
        groupMovementCtrl = GetComponent<GroupMovementController>();
        shootCtrl = GetComponent<GroupShootController>();

        AgentsSetup();
        groupMovementCtrl.Setup(this);
        shootCtrl.Setup(this);
    }

    /// <summary>
    /// Funzione che spawna ed esegue il Setup degli agents
    /// </summary>
    private void AgentsSetup()
    {
        agents = new List<AgentController>();

        for (int i = 0; i < groupStartAgents; i++)
            InstantiateNewAgent();
    }
    #endregion

    #region API
    #region Getter
    /// <summary>
    /// Funzione che ritorna il movement controller del group
    /// </summary>
    /// <returns></returns>
    public GroupMovementController GetGroupMovementController()
    {
        return groupMovementCtrl;
    }

    /// <summary>
    /// Funzione che ritorna l shoot controller del group
    /// </summary>
    /// <returns></returns>
    public GroupShootController GetGroupShootController()
    {
        return shootCtrl;
    }

    /// <summary>
    /// Funzione che ritorna la lista di agents;
    /// </summary>
    /// <returns></returns>
    public List<AgentController> GetAgents()
    {
        return agents;
    }

    /// <summary>
    /// Funzione che ritorna la posizione centrale del gruppo
    /// </summary>
    /// <returns></returns>
    public Vector3 GetGroupCenterPoint()
    {
        if (agents.Count == 0)
        {
            return transform.position;
        }
        else if (agents.Count == 1)
        {
            return agents[0].transform.position;
        }

        Bounds bounds = new Bounds(agents[0].transform.position, Vector3.zero);
        for (int i = 0; i < agents.Count; i++)
        {
            bounds.Encapsulate(agents[i].transform.position);
        }

        return bounds.center;
    }
    #endregion

    /// <summary>
    /// Funzione che si occupa di rimuovere un agent
    /// ritorna true se sono rispettate tutte le condizione, altrimenti false
    /// </summary>
    /// <returns></returns>
    public bool RemoveRandomAgent()
    {
        if (agents == null || agents.Count == 0 || agents.Count <= groupMinAgents)
            return false;

        AgentController agentToRemove = agents[Random.Range(0, agents.Count)];
        agents.Remove(agentToRemove);
        Destroy(agentToRemove.gameObject);
        return true;
    }

    /// <summary>
    /// Funzione che si occupa di generare un nuovo agent
    /// ritorna true se sono rispettate tutte le condizione, altrimenti false
    /// </summary>
    /// <returns></returns>
    public bool InstantiateNewAgent()
    {
        if (IsGroupFull())
            return false;

        Vector3 groupCenterPoint = GetGroupCenterPoint();
        Vector2 randomCirclePoint = Random.insideUnitCircle * spawnRange;
        Vector3 randomSpawnPosition = new Vector3(randomCirclePoint.x + groupCenterPoint.x, transform.position.y, randomCirclePoint.y + groupCenterPoint.z);
        AgentController newAgent = Instantiate(agentPrefab, randomSpawnPosition, Quaternion.identity, transform);
        newAgent.Setup(this);
        agents.Add(newAgent);
        return true;
    }

    /// <summary>
    /// Funzione che ritorna true se il gruppo è pieno, altrimenti false
    /// </summary>
    /// <returns></returns>
    public bool IsGroupFull()
    {
        if (agents == null || agents.Count >= groupMaxAgents)
            return true;

        return false;
    }
    #endregion
}
