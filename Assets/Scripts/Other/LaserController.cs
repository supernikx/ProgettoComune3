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

    [Header("Laser References")]
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
    /// Raggio del laser
    /// </summary>
    private float laserRadius;
    /// <summary>
    /// Lunghezza massima del laser
    /// </summary>
    private float maxLaserRange;
    /// <summary>
    /// Bool che identifica se il laser è attivo
    /// </summary>
    private bool enable;
    /// <summary>
    /// Riferimento alla coroutine di spawn del laser
    /// </summary>
    private IEnumerator spawnRoutine;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_maxLaserRange"></param>
    /// <param name="_laserRadius"></param>
    public void Setup(float _maxLaserRange, float _laserRadius)
    {
        maxLaserRange = _maxLaserRange;
        laserRadius = _laserRadius;

        StopLaser();
    }

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
    /// Funzione che avvia la coroutine di spawn del laser
    /// </summary>
    /// <param name="_spawnTime"></param>
    /// <param name="_spawnDirection"></param>
    /// <param name="_onSpawnCallback"></param>
    public void SpawnLaser(float _spawnTime, Vector3 _spawnDirection, Action _onSpawnCallback)
    {
        _spawnDirection.y = transform.position.y;
        Quaternion targetRotation = Quaternion.LookRotation((_spawnDirection - transform.position).normalized, Vector3.up);
        transform.rotation = targetRotation;

        spawnRoutine = SpawnLaserCoroutine(_spawnTime, _onSpawnCallback);
        StartCoroutine(spawnRoutine);
    }

    /// <summary>
    /// Funzione che esegue lo start del laser
    /// </summary>
    public void StartLaser()
    {
        enable = true;
    }

    /// <summary>
    /// Funzione che stoppa il laser
    /// </summary>
    public void StopLaser()
    {
        lineRenderer.enabled = false;
        enable = false;

        if (spawnRoutine != null)
            StopCoroutine(spawnRoutine);
    }

    #region Getter
    /// <summary>
    /// Funzione che ritorna se il laser è attivo
    /// </summary>
    /// <returns></returns>
    public bool IsEnable()
    {
        return enable;
    }
    #endregion
    #endregion

    /// <summary>
    /// Coroutine che fa spawnare il laser
    /// </summary>
    /// <param name="_spawnTime"></param>
    /// <param name="_onSpawnCallback"></param>
    /// <returns></returns>
    private IEnumerator SpawnLaserCoroutine(float _spawnTime, Action _onSpawnCallback)
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.startWidth = lineRenderer.startWidth = laserRadius;

        float laserRange = 0;
        float rangeOffset;
        WaitForFixedUpdate wffu = new WaitForFixedUpdate();

        if (_spawnTime != 0)
            rangeOffset = maxLaserRange / _spawnTime;
        else
            rangeOffset = maxLaserRange;


        while (laserRange < maxLaserRange)
        {
            laserRange += rangeOffset * Time.deltaTime;

            Vector3 nextLaserPoint;
            float checkAgentDistance = laserRange;
            RaycastHit hit;

            //Controllo se colpisco un ostacolo
            if (Physics.SphereCast(transform.position, laserRadius, transform.forward, out hit, laserRange, laserColliderLayer))
            {
                if (hit.collider)
                {
                    nextLaserPoint = hit.point;
                    checkAgentDistance = hit.distance;
                }
                else
                {
                    nextLaserPoint = transform.position + (transform.forward * laserRange);
                }
            }
            else
            {
                nextLaserPoint = transform.position + (transform.forward * laserRange);
            }

            //Controllo se colpisco un agent al max range dell'ostacolo
            if (Physics.SphereCast(transform.position, laserRadius, transform.forward, out hit, checkAgentDistance, agentLayer))
            {
                AgentController agent = hit.transform.gameObject.GetComponent<AgentController>();
                if (agent != null)
                    OnAgentHit?.Invoke(agent);
            }

            lineRenderer.SetPosition(1, nextLaserPoint);
            yield return wffu;
        }

        _onSpawnCallback?.Invoke();
    }
}
