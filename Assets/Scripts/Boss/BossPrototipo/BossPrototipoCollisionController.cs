using System;
using UnityEngine;

/// <summary>
/// Classe che gestisce le collisioni del boss
/// </summary>
public class BossPrototipoCollisionController : MonoBehaviour
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
    /// Riferimento al Boss Prototipo Controller
    /// </summary>
    private BossPrototipoController bossCtrl;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_bossCtrl"></param>
    public void Setup(BossPrototipoController _bossCtrl)
    {
        bossCtrl = _bossCtrl;
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
}
