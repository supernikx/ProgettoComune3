using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Classe che gestisce il gruppo e fornisce il punto di accesso agli altri controller del gruppo
/// </summary>
public class GroupController : MonoBehaviour
{
    #region Actions
    /// <summary>
    /// Evento che notifica la morte del gruppo
    /// </summary>
    public Action OnGroupDead;
    /// <summary>
    /// Evento che notifica lo spawn di un nuovo agent
    /// </summary>
    public Action<AgentController> OnAgentSpawn;
    /// <summary>
    /// Evento che notifica la morte di un agent
    /// </summary>
    public Action<AgentController> OnAgentRemoved;
    #endregion

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
    //Layer del terreno
    [SerializeField]
    private LayerMask groundLayer;

    [Header("Group Settings")]
    //Numero massimo di agents che possono essere presenti
    [SerializeField]
    private int groupMaxAgents;
    //Numero minimo di agents che possono essere presenti
    [SerializeField]
    private int groupMinAgents;
    //Oggetto che tiene il centro del gruppo
    [SerializeField]
    private Transform groupCenterObject;

    /// <summary>
    /// Lista di agent che formano il guppo
    /// </summary>
    private List<AgentController> agents;
    /// <summary>
    /// Riferimento al group movement controller
    /// </summary>
    private GroupMovementController groupMovementCtrl;
    /// <summary>
    /// Riferimento al group size controller
    /// </summary>
    private GroupSizeController groupSizeCtrl;
    /// <summary>
    /// Riferimento al shoot controller
    /// </summary>
    private GroupShootController shootCtrl;
    /// <summary>
    /// Riferimento al group feedback controller
    /// </summary>
    private GroupFeedbackController groupFeedbackCtrl;
    /// <summary>
    /// Riferimento al group orb controller
    /// </summary>
    private GroupOrbController groupOrbCtrl;
    /// <summary>
    /// Riferimento al PlayerInput
    /// </summary>
    private PlayerInput playerInput;
    /// <summary>
    /// Bool che identifica se lo script è setuppato
    /// </summary>
    private bool isSetupped = false;
    /// <summary>
    /// Bool che identifica se lo script è attivo
    /// </summary>
    private bool isEnabled = false;
    /// <summary>
    /// Vecchia posizione del centro del gruppo
    /// </summary>
    private Vector3? oldGroupCenterPos = null;

    #region Setup
    /// <summary>
    /// Funzione che esegue il setup
    /// </summary>
    public void Setup()
    {
        groupMovementCtrl = GetComponent<GroupMovementController>();
        groupSizeCtrl = GetComponent<GroupSizeController>();
        shootCtrl = GetComponent<GroupShootController>();
        groupFeedbackCtrl = GetComponent<GroupFeedbackController>();
        groupOrbCtrl = GetComponent<GroupOrbController>();
        playerInput = GetComponent<PlayerInput>();

        //Feedback setup prima di tutti perchè deve gestire eventi di spawn degli agent
        groupFeedbackCtrl.Setup(this);

        AgentsSetup();
        groupMovementCtrl.Setup(this);
        groupSizeCtrl.Setup(this);
        groupOrbCtrl.Setup(this);
        shootCtrl.Setup(this);

        isSetupped = true;
        isEnabled = false;
    }

    /// <summary>
    /// Funzione che spawna ed esegue il Setup degli agents
    /// </summary>
    public void AgentsSetup()
    {
        agents = new List<AgentController>();
        oldGroupCenterPos = null;

        for (int i = 0; i < groupStartAgents; i++)
            InstantiateNewAgent();
    }
    #endregion

    private void Update()
    {
        if (!IsSetuppedAndEnabled())
            return;

        groupCenterObject.position = CalculateGroupCenterPoint();
    }

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
    /// Funzione che ritorna il group size controller
    /// </summary>
    /// <returns></returns>
    public GroupSizeController GetGroupSizeController()
    {
        return groupSizeCtrl;
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
    /// Funzione che ritorna il group feedback controller
    /// </summary>
    /// <returns></returns>
    public GroupFeedbackController GetGroupFeedbackController()
    {
        return groupFeedbackCtrl;
    }

    /// <summary>
    /// Funzione che ritorna il group orb controller
    /// </summary>
    /// <returns></returns>
    public GroupOrbController GetGroupOrbController()
    {
        return groupOrbCtrl;
    }

    /// <summary>
    /// Funzione che ritorna il riferimento al PlayerInput
    /// </summary>
    /// <returns></returns>
    public PlayerInput GetPlayerInput()
    {
        return playerInput;
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
    /// Funzione che ritorna quanti membri ci sono nel gruppo
    /// </summary>
    /// <returns></returns>
    public int GetGroupCont()
    {
        return agents.Count;
    }

    /// <summary>
    /// Funzione che ritorna quanti membri possono esserci al massimo del gruppo
    /// </summary>
    /// <returns></returns>
    public int GetGroupMaxAgentCont()
    {
        return groupMaxAgents;
    }

    /// <summary>
    /// Funzione che ritorna quanti membri  possono esserci al minimo del gruppo
    /// </summary>
    /// <returns></returns>
    public int GetGroupMinAgentCont()
    {
        return groupMinAgents;
    }

    /// <summary>
    /// Funzione che ritorna la posizione centrale del gruppo
    /// </summary>
    /// <returns></returns>
    public Vector3 GetGroupCenterPoint()
    {
        return groupCenterObject.position;
    }

    /// <summary>
    /// Funzione che ritorna la transform centrale del gruppo
    /// </summary>
    /// <returns></returns>
    public Transform GetGroupCenterTransform()
    {
        return groupCenterObject;
    }

    /// <summary>
    /// Funzione che ritorna se lo script è attivo
    /// </summary>
    /// <returns></returns>
    public bool IsEnabled()
    {
        return isEnabled;
    }

    /// <summary>
    /// Funzione che ritorna se lo script è setuppato
    /// </summary>
    /// <returns></returns>
    public bool IsSetupped()
    {
        return isSetupped;
    }

    /// <summary>
    /// Funzione che ritorna se lo script è setuppato e attivo
    /// </summary>
    /// <returns></returns>
    public bool IsSetuppedAndEnabled()
    {
        return isSetupped && isEnabled;
    }
    #endregion

    /// <summary>
    /// Funzione che sposta il gruppo nella posizione passata come parametro
    /// </summary>
    /// <param name="position"></param>
    public void Move(Vector3 position)
    {
        transform.position = position;

        Vector3 currentPosition = transform.position;
        Vector2 randomCirclePoint = Vector2.zero;
        Vector3 randomPosition = Vector3.zero;
        foreach (AgentController agent in agents)
        {
            randomCirclePoint = UnityEngine.Random.insideUnitCircle * spawnRange;
            randomPosition = new Vector3(randomCirclePoint.x + currentPosition.x, currentPosition.y, randomCirclePoint.y + currentPosition.z);
            agent.transform.position = randomPosition;
        }
    }

    /// <summary>
    /// Funzione che si occupa di rimuovere un agent
    /// ritorna true se sono rispettate tutte le condizione, altrimenti false
    /// </summary>
    /// <returns></returns>
    public bool RemoveRandomAgent()
    {
        if (agents == null || agents.Count == 0 || agents.Count <= groupMinAgents)
        {
            CheckGroupCount();
            return false;
        }

        AgentController agentToRemove = agents[UnityEngine.Random.Range(0, agents.Count)];
        RemoveAgent(agentToRemove, false);
        return true;
    }

    /// <summary>
    /// Funzione che si occupa di rimuovere l'agent passato come parametro
    /// </summary>
    /// <param name="_agentToRemove"></param>
    /// <param name="_death"></param>
    public void RemoveAgent(AgentController _agentToRemove, bool _death)
    {
        if (agents != null && agents.Count > 0)
        {
            if (_death)
                groupOrbCtrl.InstantiatedOrb(_agentToRemove.transform.position);

            _agentToRemove.UnSetup(_death);
            agents.Remove(_agentToRemove);
        }

        OnAgentRemoved?.Invoke(_agentToRemove);
        CheckGroupCount();
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
        Vector2 randomCirclePoint = UnityEngine.Random.insideUnitCircle * spawnRange;
        Vector3 randomSpawnPosition = new Vector3(randomCirclePoint.x + groupCenterPoint.x, groupCenterPoint.y, randomCirclePoint.y + groupCenterPoint.z);
        randomSpawnPosition = CheckPointIsInsideGround(randomSpawnPosition);

        AgentController newAgent = PoolManager.instance.GetPooledObject(ObjectTypes.Agent, gameObject).GetComponent<AgentController>();
        if (newAgent != null)
        {
            newAgent.transform.position = randomSpawnPosition;
            newAgent.transform.SetParent(transform);
            newAgent.Setup(this);
            agents.Add(newAgent);
            OnAgentSpawn?.Invoke(newAgent);
            return true;
        }

        return false;
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

    /// <summary>
    /// Funzione che abilita/disabilita il gruppo in base al parametro passato
    /// </summary>
    /// <param name="_enableGrup"></param>
    public void Enable(bool _enableGrup)
    {
        isEnabled = _enableGrup;
    }
    #endregion

    /// <summary>
    /// Funzione che controlla se ci sono ancora membri nel gruppo
    /// </summary>
    private void CheckGroupCount()
    {
        if (agents != null && agents.Count == 0)
            OnGroupDead?.Invoke();
    }

    /// <summary>
    /// Funzione che Calcola la posizione centrale del gruppo
    /// </summary>
    /// <returns></returns>
    private Vector3 CalculateGroupCenterPoint()
    {
        if (agents.Count == 0)
        {
            if (oldGroupCenterPos == null)
                return transform.position;
            else
                return oldGroupCenterPos.Value;
        }
        else if (agents.Count == 1)
        {
            oldGroupCenterPos = agents[0].transform.position;
            return agents[0].transform.position;
        }

        Vector3 centerPoint = Vector3.zero;
        for (int i = 0; i < agents.Count; i++)
            centerPoint += agents[i].transform.position;

        oldGroupCenterPos = groupCenterObject.transform.position;
        return centerPoint / agents.Count;
    }

    /// <summary>
    /// Funzione che controlla se il punto è dentro il ground, se false fixa la posizione
    /// </summary>
    /// <param name="_point"></param>
    /// <returns></returns>
    private Vector3 CheckPointIsInsideGround(Vector3 _point)
    {
        //Fix della posizone per evitare di controllare dentro il collider
        _point.y += 0.1f;

        //Controllo se il punto è sul terreno
        Ray ray = new Ray(_point, Vector3.down);
        if (Physics.Raycast(ray, 1f, groundLayer))
            return _point;

        //Se non è sul terreno prendo il punto opposto
        Vector3 groupCenter = GetGroupCenterPoint();
        Vector3 pointDir = (_point - groupCenter).normalized;
        float pointDis = Vector3.Distance(groupCenter, _point);
        Vector3 oppositePoint = groupCenter + (-pointDir * pointDis);

        //Controllo se il punto opposto è sul terreno
        Ray oppositeRay = new Ray(oppositePoint, Vector3.down);
        if (Physics.Raycast(oppositeRay, 1f, groundLayer))
            return oppositePoint;

        //Se anche il punto opposto è fuori dal terreno spawna in centro
        return groupCenter;
    }
}
