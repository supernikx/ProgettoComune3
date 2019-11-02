using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce le collisioni dell'agent
/// </summary>
public class AgentCollisionController : MonoBehaviour
{
    /// <summary>
    /// Bool che indentifica se c'è una collision con il ground o no
    /// </summary>
    private bool groundCollision;
    /// <summary>
    /// Riferimento all'agent controller
    /// </summary>
    private AgentController agentCtrl;

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_agentCtrl"></param>
    public void Setup(AgentController _agentCtrl)
    {
        agentCtrl = _agentCtrl;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            groundCollision = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            groundCollision = false;
        }
    }

    #region API
    #region Getter
    /// <summary>
    /// Funzione che ritorna se si è in collisione con il terreno
    /// </summary>
    /// <returns></returns>
    public bool IsGroundCollision()
    {
        return groundCollision;
    }
    #endregion
    #endregion
}
