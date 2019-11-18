using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce le collisioni dell'agent
/// </summary>
public class AgentCollisionController : MonoBehaviour
{
    /// <summary>
    /// Riferimento all'agent controller
    /// </summary>
    private AgentController agentCtrl;
    /// <summary>
    /// Riferimento al collider
    /// </summary>
    private new Collider collider;
    /// <summary>
    /// Riferimento al rigidBody
    /// </summary>
    private Rigidbody rb;
    /// <summary>
    /// Bool che indentifica se lo script è setuppato
    /// </summary>
    private bool isSetupped = false;

    #region Setup
    /// <summary>
    /// Funzione che Inizializza lo script e prende le referenza
    /// </summary>
    public void Init(AgentController _agentCtrl)
    {
        agentCtrl = _agentCtrl;
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_agentCtrl"></param>
    public void Setup()
    {
        isSetupped = true;
    }

    /// <summary>
    /// Funzione che esegue l'UnSetup dello script
    /// </summary>
    public void UnSetup()
    {
        isSetupped = false;
    }
    #endregion

    #region API
    #region Getter
    /// <summary>
    /// Funzione che ritorna il RigidBody
    /// </summary>
    /// <returns></returns>
    public Rigidbody GetRigidBody()
    {
        return rb;
    }

    /// <summary>
    /// Funzione che ritorna il collider
    /// </summary>
    /// <returns></returns>
    public Collider GetCollider()
    {
        return collider;
    }
    #endregion
    #endregion
}
