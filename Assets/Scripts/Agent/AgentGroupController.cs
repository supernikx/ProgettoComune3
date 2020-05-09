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
    /// Riferimento al RigidBody
    /// </summary>
    private Rigidbody rb;
    /// <summary>
    /// Identifica se lo script è setuppato
    /// </summary>
    private bool isSetupped = false;

    public void Init(AgentController _agentCtrl)
    {
        rb = GetComponent<Rigidbody>();
        agentCtrl = _agentCtrl;

        UnSetup();
    }

    /// <summary>
    /// Funzione di setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    public void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        isSetupped = true;
    }

    /// <summary>
    /// Funzione di Unsetup
    /// </summary>
    public void UnSetup()
    {
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        isSetupped = false;
    }

    private void FixedUpdate()
    {
        if (!isSetupped || !groupCtrl.IsSetuppedAndEnabled())
            return;

        float groupY = groupCtrl.GetGroupCenterPoint().y;
        float yOffset = Mathf.Abs(transform.position.y - groupY);

        if (yOffset > maxYDistanceFromGroup)
            groupCtrl.RemoveAgent(agentCtrl, true);
    }
}
