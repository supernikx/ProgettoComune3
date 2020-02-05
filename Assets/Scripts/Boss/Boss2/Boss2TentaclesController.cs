using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce i tentacoli del Boss 2
/// </summary>
public class Boss2TentaclesController : MonoBehaviour
{
    #region Action
    /// <summary>
    /// Evento che notifica che un tentacolo è morto
    /// </summary>
    public Action<int> OnTentacleDead;
    /// <summary>
    /// Evento che notifica che tutti i tentacoli sono porti
    /// </summary>
    public Action OnAllTentaclesDead;
    #endregion

    [Header("Reference Tentacles Settings")]
    //Tentacoli della fase 1
    [SerializeField]
    private List<TentacleController> tentacles;

    /// <summary>
    /// Riferimento al Boss Controller
    /// </summary>
    private Boss2Controller bossCtrl;
    /// <summary>
    /// Riferimento al collision Controller
    /// </summary>
    private BossCollisionController collisionCtrl;
    /// <summary>
    /// Lista che contiene i tentacoli che sono vivi
    /// </summary>
    private List<TentacleController> aliveTentacles;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_bossCtrl"></param>
    public void Setup(Boss2Controller _bossCtrl)
    {
        bossCtrl = _bossCtrl;
        collisionCtrl = bossCtrl.GetBossCollisionController();
        aliveTentacles = new List<TentacleController>();
    }

    /// <summary>
    /// Funzione che esegue il Setup dei tentacoli della fase 2
    /// </summary>
    public void TentaclesSetup()
    {
        aliveTentacles.Clear();

        for (int i = 0; i < tentacles.Count; i++)
        {
            tentacles[i].gameObject.SetActive(true);
            aliveTentacles.Add(tentacles[i]);
            aliveTentacles[i].Setup(this);
            aliveTentacles[i].OnTentacleDead += HandleOnTentacleDead;
            aliveTentacles[i].OnAgentHit += HandleOnAgentHit;
        }
    }

    #region Handles
    /// <summary>
    /// Funzione che gestisce l'evento di hit di un agent
    /// </summary>
    /// <param name="obj"></param>
    private void HandleOnAgentHit(AgentController _agent)
    {
        collisionCtrl.OnAgentHit?.Invoke(_agent);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di morte di un tentacolo
    /// </summary>
    /// <param name="obj"></param>
    private void HandleOnTentacleDead(TentacleController _tentacle)
    {
        _tentacle.OnTentacleDead -= HandleOnTentacleDead;
        _tentacle.OnAgentHit -= HandleOnAgentHit;

        aliveTentacles.Remove(_tentacle);

        OnTentacleDead?.Invoke(_tentacle.GetDeadTentacleDamage());
        if (aliveTentacles.Count == 0)
            OnAllTentaclesDead?.Invoke();
    }
    #endregion

    #region API
    /// <summary>
    /// Funzione che chiama lo stomp del tentacolo passato come parametro
    /// </summary>
    /// <param name="_tentacleIndex"></param>
    /// <param name="_stompPosition"></param>
    /// <param name="_stompDuration"></param>
    public void Stomp(int _tentacleIndex, Vector3 _stompPosition, float _stompDuration)
    {
        if (_tentacleIndex < 0 || _tentacleIndex > aliveTentacles.Count - 1)
            return;

        aliveTentacles[_tentacleIndex].Stomp(_stompPosition, _stompDuration);
    }

    /// <summary>
    /// Funzione che chiama il reset del tentacolo passato come parametro
    /// </summary>
    /// <param name="_tentacleIndex"></param>
    /// <param name="_resetDuration"></param>
    public void Reset(int _tentacleIndex, float _resetDuration)
    {
        if (_tentacleIndex < 0 || _tentacleIndex > aliveTentacles.Count - 1)
            return;

        aliveTentacles[_tentacleIndex].Reset(_resetDuration);
    }
    #endregion
}
