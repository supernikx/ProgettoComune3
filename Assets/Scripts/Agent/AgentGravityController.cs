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
    /// Riferimento al group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al RigidBody
    /// </summary>
    private Rigidbody rb;
    /// <summary>
    /// Bool che identifica se lo script è setuppato o no
    /// </summary>
    private bool isSetupped = false;

    #region Setup

    /// <summary>
    /// Funzione che inizializza lo script e prende le referenze
    /// </summary>
    public void Init()
    {
        rb = GetComponent<Rigidbody>();
        UnSetup();
    }

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    public void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;
        isSetupped = true;
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    /// <summary>
    /// Funzione che esegue l'UnSetup
    /// </summary>
    public void UnSetup()
    {
        isSetupped = false;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    #endregion

    private void FixedUpdate()
    {
        if (!isSetupped)
            return;

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallingMultiplier - 1) * Time.deltaTime;
        }
    }
}
