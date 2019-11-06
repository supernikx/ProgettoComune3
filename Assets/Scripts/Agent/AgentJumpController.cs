using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il salto dell'agent
/// </summary>
public class AgentJumpController : MonoBehaviour
{
    [Header("Jump Settings")]
    // Altezza massima raggiungibile in unity unit
    [SerializeField]
    private float jumpForce;
    //Delay dalla pressione del tasto al salto effettivo
    [SerializeField]
    private Vector2 jumpDelayRange;

    /// <summary>
    /// Riferimento all'agent controller
    /// </summary>
    private AgentController agentCtrl;
    /// <summary>
    /// Riferimento all'agent collision ctrl
    /// </summary>
    private AgentCollisionController agentCollisionCtrl;
    /// <summary>
    /// Riferimento al RigidBody
    /// </summary>
    private Rigidbody rb;
    /// <summary>
    /// Bool che identifica se l'agent può saltare
    /// </summary>
    private bool canJump;
    /// <summary>
    /// Bool che identifica se è stato premuto il tasto di salto
    /// </summary>
    private bool jumpPressed;
    /// <summary>
    /// Tempo di delay del salto
    /// </summary>
    private float jumpDelay;
    /// <summary>
    /// Timer che tiene conto del delay del salto
    /// </summary>
    private float jumpTimer;
    /// <summary>
    /// Bool che identifica se lo script è setuppato
    /// </summary>
    private bool isSetupped = false;

    #region Setup
    /// <summary>
    /// Funzione che Inizializza lo script e prende le referenza
    /// </summary>
    /// <param name="_agentCtrl"></param>
    public void Init(AgentController _agentCtrl)
    {
        agentCtrl = _agentCtrl;
        agentCollisionCtrl = agentCtrl.GetAgentCollisionController();
        rb = agentCollisionCtrl.GetRigidBody();
    }

    /// <summary>
    /// Funzione che segue il setup
    /// </summary>
    /// <param name="_agentCtrl"></param>
    public void Setup()
    {
        canJump = true;
        jumpPressed = false;
        isSetupped = true;
    }

    /// <summary>
    /// Funzione che esegue l'UnSetup
    /// </summary>
    public void UnSetup()
    {
        isSetupped = false;
    }
    #endregion

    private void FixedUpdate()
    {
        if (!isSetupped)
            return;

        if (agentCollisionCtrl.IsGroundCollision())
            canJump = true;
        else
            canJump = false;

        if (jumpPressed)
        {
            jumpTimer += Time.deltaTime;
            if (jumpTimer >= jumpDelay)
            {
                if (canJump)
                {
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    //rb.velocity = Vector3.up * jumpForce;
                    canJump = false;
                }

                jumpPressed = false;
            }
        }
    }

    /// <summary>
    /// Funzione che fa partire il salto
    /// </summary>
    public void Jump()
    {
        if (canJump && !jumpPressed)
        {
            jumpPressed = true;
            jumpDelay = Random.Range(jumpDelayRange.x, jumpDelayRange.y);
            jumpTimer = 0;
        }
    }
}
