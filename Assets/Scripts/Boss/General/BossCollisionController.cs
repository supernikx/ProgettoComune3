using System;
using UnityEngine;

/// <summary>
/// Classe che gestisce le collisioni del boss
/// </summary>
public class BossCollisionController : MonoBehaviour
{
    #region Actions
    /// <summary>
    /// Evento che notifica la collisione con un ostacolo
    /// </summary>
    public Action<GameObject> OnObstacleHit;
    /// <summary>
    /// Evento che notifica la collisione con un agent
    /// </summary>
    public Action<AgentController> OnAgentHit;
    #endregion

    /// <summary>
    /// Riferimento al BossController
    /// </summary>
    private BossControllerBase bossCtrl;
    /// <summary>
    /// Riferimento al RigidBody
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_bossCtrl"></param>
    public void Setup(BossControllerBase _bossCtrl)
    {
        bossCtrl = _bossCtrl;
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (bossCtrl == null || (bossCtrl != null && !bossCtrl.IsSetuppedAndEnabled()))
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            OnObstacleHit?.Invoke(collision.gameObject);
        }
        else
        {
            AgentController agent = collision.gameObject.GetComponent<AgentController>();
            if (agent != null)
                OnAgentHit?.Invoke(agent);
        }
    }

    #region API
    #region Getter
    /// <summary>
    /// Funzione che ritorna il rigid body
    /// </summary>
    /// <returns></returns>
    public Rigidbody GetRigidBody()
    {
        return rb;
    }
    #endregion
    #endregion
}
