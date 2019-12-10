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
    /// Evento che notifica l'avvenuta rotazione del tentacolo
    /// </summary>
    public Action OnTentaclesRotated;
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
    private List<TentacleController> phase1Tentacles;
    //Tentacoli della fase 3
    [SerializeField]
    private List<TentacleController> phase2Tentacles;
    //Zone
    [SerializeField]
    private List<Transform> zones;

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
    /// Lista che contiene i tentacoli che stanno ruotando
    /// </summary>
    private List<TentacleController> rotatingTentacles;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_bossCtrl"></param>
    public void Setup(Boss2Controller _bossCtrl)
    {
        bossCtrl = _bossCtrl;
        collisionCtrl = bossCtrl.GetBossCollisionController();
        rotatingTentacles = new List<TentacleController>();
        aliveTentacles = new List<TentacleController>();
    }

    /// <summary>
    /// Funzione che esegue il Setup dei tentacoli della fase 1
    /// </summary>
    public void Phase1TentaclesSetup()
    {
        aliveTentacles.Clear();

        for (int i = 0; i < phase2Tentacles.Count; i++)
        {
            phase2Tentacles[i].OnTentacleRotated -= HandleOnTentacleRotated;
            phase2Tentacles[i].OnTentacleDead -= HandleOnTentacleDead;
            phase2Tentacles[i].OnAgentHit -= HandleOnAgentHit;
            phase2Tentacles[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < phase1Tentacles.Count; i++)
        {
            phase1Tentacles[i].gameObject.SetActive(true);
            aliveTentacles.Add(phase1Tentacles[i]);
            aliveTentacles[i].Setup(this, GetFixedZoneIndex(i));
            aliveTentacles[i].OnTentacleDead += HandleOnTentacleDead;
            aliveTentacles[i].OnAgentHit += HandleOnAgentHit;
        }
    }

    /// <summary>
    /// Funzione che esegue il Setup dei tentacoli della fase 2
    /// </summary>
    public void Phase2TentaclesSetup()
    {
        aliveTentacles.Clear();

        for (int i = 0; i < phase1Tentacles.Count; i++)
        {
            phase1Tentacles[i].OnTentacleRotated -= HandleOnTentacleRotated;
            phase1Tentacles[i].OnTentacleDead -= HandleOnTentacleDead;
            phase1Tentacles[i].OnAgentHit -= HandleOnAgentHit;
            phase1Tentacles[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < phase2Tentacles.Count; i++)
        {
            phase2Tentacles[i].gameObject.SetActive(true);
            aliveTentacles.Add(phase2Tentacles[i]);
            aliveTentacles[i].Setup(this);
            aliveTentacles[i].OnTentacleDead += HandleOnTentacleDead;
            aliveTentacles[i].OnAgentHit += HandleOnAgentHit;
        }
    }

    #region Handles
    /// <summary>
    /// Funzione che gestisce l'evento di rotazione completata
    /// </summary>
    /// <param name="_tentacle"></param>
    private void HandleOnTentacleRotated(TentacleController _tentacle)
    {
        _tentacle.OnTentacleRotated -= HandleOnTentacleRotated;
        rotatingTentacles.Remove(_tentacle);

        if (rotatingTentacles.Count == 0)
            OnTentaclesRotated?.Invoke();
    }

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
        _tentacle.OnTentacleRotated -= HandleOnTentacleRotated;
        _tentacle.OnTentacleDead -= HandleOnTentacleDead;
        _tentacle.OnAgentHit -= HandleOnAgentHit;

        rotatingTentacles.Remove(_tentacle);
        aliveTentacles.Remove(_tentacle);

        OnTentacleDead?.Invoke(_tentacle.GetDeadTentacleDamage());
        if (aliveTentacles.Count == 0)
            OnAllTentaclesDead?.Invoke();
        else if (rotatingTentacles.Count == 0)
            OnTentaclesRotated?.Invoke();
    }
    #endregion

    #region API
    /// <summary>
    /// Funzione che fa ruotare i tentacoli
    /// </summary>
    /// <param name="_rotationTime"></param>
    public void Rotate(float _rotationTime)
    {
        for (int i = 0; i < aliveTentacles.Count; i++)
        {
            aliveTentacles[i].OnTentacleRotated += HandleOnTentacleRotated;
            rotatingTentacles.Add(aliveTentacles[i]);
            aliveTentacles[i].RotateToNextPoint(_rotationTime);
        }
    }

    /// <summary>
    /// Funzione che fa saltare i tentacoli che si trovano nella zona passata come parametro
    /// </summary>
    /// <param name="_jumpZone"></param>
    /// <param name="_jumpForce"></param>
    /// <param name="_jumpTime"></param>
    public void Jump(int _jumpZone, float _jumpForce, float _jumpTime)
    {
        if (_jumpZone < 0 || _jumpZone > zones.Count - 1)
            return;

        for (int i = 0; i < aliveTentacles.Count; i++)
        {
            if (aliveTentacles[i].GetCurrentZone() == _jumpZone)
            {
                aliveTentacles[i].Jump(_jumpForce, _jumpTime);
                return;
            }
        }
    }

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

    #region Getter
    /// <summary>
    /// Funzione che ritorna la transform della zone in base all'index
    /// </summary>
    /// <param name="_zoneIndex"></param>
    /// <returns></returns>
    public Transform GetZoneByIndex(int _zoneIndex)
    {
        int fixedIndex = GetFixedZoneIndex(_zoneIndex);
        return zones[fixedIndex];
    }

    /// <summary>
    /// Funzione che ritorna l'index della zone passata come parametro
    /// </summary>
    /// <param name="_zone"></param>
    /// <returns></returns>
    public int GetIndexByZone(Transform _zone)
    {
        for (int i = 0; i < zones.Count; i++)
        {
            if (zones[i] == _zone)
                return i;
        }

        return -1;
    }

    /// <summary>
    /// Funzione che ritorna l'index delle zone fixato
    /// </summary>
    /// <param name="_zoneIndex"></param>
    /// <returns></returns>
    public int GetFixedZoneIndex(int _zoneIndex)
    {
        if (_zoneIndex > zones.Count - 1)
            return _zoneIndex - zones.Count;

        if (_zoneIndex < 0)
            return zones.Count - _zoneIndex;

        return _zoneIndex;
    }
    #endregion
    #endregion
}
