using System;
using UnityEngine;

/// <summary>
/// Classe che gestisce le collisioni del boss
/// </summary>
public class BossPrototipoCollisionController : MonoBehaviour
{
    #region Actions
    public Action<GameObject> OnObstacleHit;
    public Action<AgentController> OnAgentHit;
    #endregion

    private void OnCollisionEnter(Collision collision)
    {
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
