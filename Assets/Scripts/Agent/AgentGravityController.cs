using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che controlla la gravità dell'agent
/// </summary>
public class AgentGravityController : MonoBehaviour
{
    [Header("Gravity Settings")]
    //Moltiplicatore per la caduta
    [SerializeField]
    private float fallingMultiplier;

    /// <summary>
    /// Riferimento all'agent controller
    /// </summary>
    private AgentController agentCtrl;
    /// <summary>
    /// Riferimento al RigidBody
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_collisionCtrl"></param>
    public void Setup(AgentController _agentCtrl)
    {
        agentCtrl = _agentCtrl;
        rb = agentCtrl.GetAgentCollisionController().GetRigidBody();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallingMultiplier - 1) * Time.deltaTime;
        }
    }
}
