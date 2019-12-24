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

    [Header("Collision Settings")]
    //Layer degli ostacoli
    [SerializeField]
    private LayerMask obstacleLayer;

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
    /// <summary>
    /// Funzione che controlla se ci sono collisioni nel nuovo punto di moviemento
    /// </summary>
    /// <param name="_newPos"></param>
    /// <returns></returns>
    public bool CheckCollision(Vector3 _newPos)
    {
        float distance = Vector3.Distance(_newPos, bossCtrl.transform.position);
        Vector3 fiexdPos = bossCtrl.transform.position;
        fiexdPos.y += 0.5f;
        Ray ray = new Ray(fiexdPos, bossCtrl.transform.forward);
        RaycastHit hitInfo;
        return Physics.Raycast(ray, out hitInfo, distance + 1f, obstacleLayer);
    }

    #region Getter
    /// <summary>
    /// Funzione che ritorna il rigid body
    /// </summary>
    /// <returns></returns>
    public Rigidbody GetRigidBody()
    {
        return rb;
    }

    /// <summary>
    /// Funzione che ritorna il layer di ostacoli
    /// </summary>
    /// <returns></returns>
    public LayerMask GetObstacleLayer()
    {
        return obstacleLayer;
    }
    #endregion
    #endregion
}
