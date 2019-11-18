using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il trail per il Boss 1
/// </summary>
public class Boss1TrailController : MonoBehaviour
{
    [Header("Trail Settings")]
    //Riferimento al prefab del TrailController
    [SerializeField]
    private TrailController trailControllerPrefab;
    //Tempo prima che il trail cominci a sparire
    [SerializeField]
    private float trailDespawnDelayTime;
    //Tempo che impiega il trail a sparire
    [SerializeField]
    private float trailDespawnTime;

    /// <summary>
    /// Riferimento al Boss Controller
    /// </summary>
    private Boss1Controller bossCtrl;
    /// <summary>
    /// Riferimento al Boss Collision Controller
    /// </summary>
    private BossCollisionController bossCollisionCtrl;
    /// <summary>
    /// Lista di Trail attualmente in gioco
    /// </summary>
    private List<TrailController> trails;
    /// <summary>
    /// Riferimento al Trail che si sta monitorando
    /// </summary>
    private TrailController currentTrail;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_bossCtrl"></param>
    public void Setup(Boss1Controller _bossCtrl)
    {
        bossCtrl = _bossCtrl;
        bossCollisionCtrl = bossCtrl.GetBossCollisionController();
        trails = new List<TrailController>();
    }

    #region Handlers
    /// <summary>
    /// Funzione che gestisce l'evento di despawn del Trail
    /// </summary>
    /// <param name="_trail"></param>
    private void HandleOnTrailDespawn(TrailController _trail)
    {
        if (trails.Contains(_trail))
        {
            _trail.OnAgentHit -= HandleOnAgentHit;
            _trail.OnTrailDespawn -= HandleOnTrailDespawn;
            trails.Remove(_trail);
        }
    }

    /// <summary>
    /// Funzione che gestisce l'evento di agent hit del trail
    /// </summary>
    /// <param name="_agent"></param>
    private void HandleOnAgentHit(AgentController _agent)
    {
        bossCollisionCtrl.OnAgentHit?.Invoke(_agent);
    }
    #endregion

    #region API
    /// <summary>
    /// Funzione che istanzia un nuovo Trail
    /// </summary>
    public void InstantiateNewTrail()
    {
        TrailController newTrail = Instantiate(trailControllerPrefab);
        newTrail.OnAgentHit += HandleOnAgentHit;
        newTrail.OnTrailDespawn += HandleOnTrailDespawn;

        newTrail.Setup(transform.position, bossCtrl.transform.rotation);
        trails.Add(newTrail);
        currentTrail = newTrail;
    }

    /// <summary>
    /// Funzione che aggiorna la posizione dell'ultimo trail istanziato
    /// </summary>
    public void UpdateLastTrail()
    {
        if (currentTrail != null)
            currentTrail.UpdateTrailPos(transform.position);
    }

    /// <summary>
    /// Funzione che interrompe l'aggiornamento dell'ultimo Trail
    /// </summary>
    public void EndTrail()
    {
        if (currentTrail != null)
            currentTrail.EndTrailUpdate(transform.position, trailDespawnDelayTime, trailDespawnTime);

        currentTrail = null;
    }
    #endregion

    private void OnDisable()
    {
        if (trails != null)
        {
            foreach (TrailController t in trails)
            {
                t.OnAgentHit -= HandleOnAgentHit;
                t.OnTrailDespawn -= HandleOnTrailDespawn;
            }
        }
    }
}
