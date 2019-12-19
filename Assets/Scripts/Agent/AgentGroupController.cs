using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che controlla il comportamento dell'agent rispetto al gruppo
/// </summary>
public class AgentGroupController : MonoBehaviour
{
    [Header("Agent Group Settings")]
    //Distanza massima sulla Y che può stare rispetto al gruppo
    [SerializeField]
    private float maxYDistanceFromGroup;

    /// <summary>
    /// Riferimento al agent controller
    /// </summary>
    private AgentController agentCtrl;
    /// <summary>
    /// Riferimento all'agent group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Identifica se lo script è setuppato
    /// </summary>
    private bool isSetupped = false;

    public void Init(AgentController _agentCtrl)
    {
        agentCtrl = _agentCtrl;
    }

    /// <summary>
    /// Funzione di setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    public void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;
        isSetupped = true;
    }

    /// <summary>
    /// Funzione di Unsetup
    /// </summary>
    public void UnSetup()
    {
        isSetupped = false;
    }

    private void Update()
    {
        if (!isSetupped || !groupCtrl.IsSetuppedAndEnabled())
            return;

        float groupY = groupCtrl.GetGroupCenterPoint().y;

        if (Mathf.Abs(transform.position.y - groupY) > maxYDistanceFromGroup)
            groupCtrl.RemoveAgent(agentCtrl);
    }
}
