using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il laser
/// </summary>
public class LaserController : MonoBehaviour
{
    #region Actions
    /// <summary>
    /// Evento che notifica che un agent è stato colpito
    /// </summary>
    public Action<AgentController> OnAgentHit;
    #endregion

    [Header("laser References")]
    //Larghezza del laser
    [SerializeField]
    private float laserRadius;
    //Range massimo del laser
    [SerializeField]
    private float maxLaserRange;
    //Layer con cui può collider il laser
    [SerializeField]
    private LayerMask laserColliderLayer;
    //Layer degli agent
    [SerializeField]
    private LayerMask agentLayer;
    //Riferimento al LineRenderer
    [SerializeField]
    private LineRenderer lineRenderer;

    /// <summary>
    /// Bool che identifica se il laser è attivo
    /// </summary>
    private bool enable;

    // Update is called once per frame
    private void Update()
    {
        if (enable)
        {
            Vector3 nextLaserPoint;
            float checkAgentDistance = maxLaserRange;
            RaycastHit hit;

            //Controllo se colpisco un ostacolo
            if (Physics.SphereCast(transform.position, laserRadius, transform.forward, out hit, maxLaserRange, laserColliderLayer))
            {
                if (hit.collider)
                {
                    nextLaserPoint = hit.point;
                    checkAgentDistance = hit.distance;
                }
                else
                {
                    nextLaserPoint = transform.position + (transform.forward * maxLaserRange);
                }
            }
            else
            {
                nextLaserPoint = transform.position + (transform.forward * maxLaserRange);
            }
            
            //Controllo se colpisco un agent al max range dell'ostacolo
            if (Physics.SphereCast(transform.position, laserRadius, transform.forward, out hit, checkAgentDistance, agentLayer))
            {
                AgentController agent = hit.transform.gameObject.GetComponent<AgentController>();
                if (agent != null)
                    OnAgentHit?.Invoke(agent);
            }

            lineRenderer.SetPosition(1, nextLaserPoint);
        }
    }

    #region API
    /// <summary>
    /// Funzione che esegue lo start del laser
    /// </summary>
    public void StartLaser()
    {
        enable = true;
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = lineRenderer.startWidth = laserRadius;
        lineRenderer.SetPosition(0, transform.position);
    }

    /// <summary>
    /// Funzione che stoppa il laser
    /// </summary>
    public void StopLaser()
    {
        lineRenderer.enabled = false;
        enable = false;
    }
    #endregion
}
