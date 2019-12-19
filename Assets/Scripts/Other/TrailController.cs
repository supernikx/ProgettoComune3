using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che contralla il Trail
/// </summary>
public class TrailController : MonoBehaviour
{
    #region Action
    /// <summary>
    /// Evento che notifica l'hit con un agent
    /// </summary>
    public Action<AgentController> OnAgentHit;
    /// <summary>
    /// Evento che notifica il despawn del Trail
    /// </summary>
    public Action<TrailController> OnTrailDespawn;
    #endregion

    [Header("Trail Settings")]
    /// <summary>
    /// Offset sulla Y
    /// </summary>
    private float startPosYOffset = 5.4f;

    /// <summary>
    /// Dimensione iniziale del Trail
    /// </summary>
    private Vector3 trailStartSize;
    /// <summary>
    /// Ultima Posizione nota del Trail
    /// </summary>
    private Vector3 trailLastPosition;
    /// <summary>
    /// Posizione iniziale del Trail
    /// </summary>
    private Vector3 trailStartPosition;
    /// <summary>
    /// Riferimento alla Coroutine di despawn
    /// </summary>
    private IEnumerator trailDespawnRoutine;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    public void Setup(Vector3 _startPos, Quaternion _rotation)
    {
        trailStartPosition = FixYPos(_startPos);
        transform.position = trailStartPosition;
        transform.rotation = _rotation;
        trailStartSize = transform.localScale;
    }

    /// <summary>
    /// Funzione che aggiorna la posizione del Trail e ne modifica le dimensioni
    /// </summary>
    /// <param name="_newPos"></param>
    public void UpdateTrailPos(Vector3 _newPos)
    {
        _newPos = FixYPos(_newPos);
        Vector3 newTrailCenter = (_newPos + trailStartPosition) / 2;

        float distance = Vector3.Distance(_newPos, trailStartPosition);
        Vector3 newTrailSize = trailStartSize;
        trailStartSize.z = distance;
        
        transform.position = newTrailCenter;
        transform.localScale = trailStartSize;
    }

    /// <summary>
    /// Funzione che finisce il tracking della posizione e inizia la coroutine di despawn
    /// </summary>
    /// <param name="_lastPos"></param>
    /// <param name="_trailDespawnDelay"></param>
    /// <param name="_trailDespawnTime"></param>
    public void EndTrailUpdate(Vector3 _lastPos, float _trailDespawnDelay, float _trailDespawnTime)
    {        
        trailLastPosition = FixYPos(_lastPos);
        trailDespawnRoutine = TrailDespawnCoroutine(_trailDespawnDelay, _trailDespawnTime);
        StartCoroutine(trailDespawnRoutine);
    }

    /// <summary>
    /// Coroutine che fa despawnare gradualmente il Trail
    /// </summary>
    /// <param name="_trailDespawnDelay"></param>
    /// <param name="_trailDespawnTime"></param>
    /// <returns></returns>
    private IEnumerator TrailDespawnCoroutine(float _trailDespawnDelay, float _trailDespawnTime)
    {
        yield return new WaitForSeconds(_trailDespawnDelay);

        float distance = Vector3.Distance(trailStartPosition, trailLastPosition);
        float sizeRate = distance / _trailDespawnTime;
        Vector3 newPosStartPos;
        Vector3 newTrailCenter;
        Vector3 newTrailSize;

        while (distance > 0.5f)
        {
            distance -= sizeRate * Time.deltaTime;
            newPosStartPos = trailLastPosition - (transform.forward * distance);
            newTrailCenter = ((newPosStartPos + trailLastPosition)) / 2;

            newTrailSize = trailStartSize;
            trailStartSize.z = distance;

            transform.position = newTrailCenter;
            transform.localScale = trailStartSize;
            yield return null;
        }

        OnTrailDespawn?.Invoke(this);
        Destroy(gameObject);
    }

    /// <summary>
    /// Funzione che aggiusta la posizione del Vector3 passato in base all'offset della Y
    /// </summary>
    /// <param name="_vectorToFix"></param>
    /// <returns></returns>
    private Vector3 FixYPos(Vector3 _vectorToFix)
    {
        _vectorToFix.y = _vectorToFix.y - startPosYOffset;
        return _vectorToFix;
    }

    private void OnTriggerEnter(Collider other)
    {
        AgentController agent = other.GetComponent<AgentController>();
        if (agent != null)
            OnAgentHit?.Invoke(agent);
    }

    private void OnDisable()
    {
        if (trailDespawnRoutine != null)
            StopCoroutine(trailDespawnRoutine);
    }
}
