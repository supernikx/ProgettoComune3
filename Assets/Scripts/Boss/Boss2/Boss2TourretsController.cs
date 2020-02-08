using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce le torrette del Boss 2
/// </summary>
public class Boss2TourretsController : MonoBehaviour
{
    #region Action
    /// <summary>
    /// Evento che notifica che una torretta è morto
    /// </summary>
    public Action<int> OnTourretDead;
    /// <summary>
    /// Evento che notifica che tutte le torrette sono morte
    /// </summary>
    public Action OnAllTourretsDead;
    #endregion

    [Header("Reference Tourrets Settings")]
    //Tentacoli della fase 1
    [SerializeField]
    private Transform tourretParent;

    /// <summary>
    /// Riferimento al group ctrl
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al Boss Controller
    /// </summary>
    private Boss2Controller bossCtrl;
    /// <summary>
    /// Riferimento al collision Controller
    /// </summary>
    private BossCollisionController collisionCtrl;
    /// <summary>
    /// Lista che contiene le torrette che sono vive
    /// </summary>
    private List<TourretController> aliveTouretts;
    /// <summary>
    /// Bool che identifica se le torrette possono sparare
    /// </summary>
    private bool canAim;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_bossCtrl"></param>
    public void Setup(Boss2Controller _bossCtrl)
    {
        bossCtrl = _bossCtrl;
        groupCtrl = bossCtrl.GetLevelManager().GetGroupController();
        collisionCtrl = bossCtrl.GetBossCollisionController();
        aliveTouretts = new List<TourretController>();
    }

    /// <summary>
    /// Funzione che esegue il Setup delle torrette
    /// </summary>
    public void TourretsSetup()
    {
        aliveTouretts.Clear();

        TourretController[] tourrets = tourretParent.GetComponentsInChildren<TourretController>();
        for (int i = 0; i < tourrets.Length; i++)
        {
            tourrets[i].gameObject.SetActive(true);
            aliveTouretts.Add(tourrets[i]);
            aliveTouretts[i].Setup(this);
            aliveTouretts[i].OnTourretDead += HandleOnTourretDead;
            aliveTouretts[i].OnAgentHit += HandleOnAgentHit;
        }
    }

    private void Update()
    {
        if (canAim)
            AimTourrets();
    }

    /// <summary>
    /// Funzione che fa mirare le torrette nella direzione del gruppo
    /// </summary>
    private void AimTourrets()
    {
        for (int i = 0; i < aliveTouretts.Count; i++)
        {
            aliveTouretts[i].Aim(groupCtrl.GetGroupCenterPoint());
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
    /// Funzione che gestisce l'evento di morte di una torretta
    /// </summary>
    /// <param name="obj"></param>
    private void HandleOnTourretDead(TourretController _tentacle)
    {
        _tentacle.OnTourretDead -= HandleOnTourretDead;
        _tentacle.OnAgentHit -= HandleOnAgentHit;

        aliveTouretts.Remove(_tentacle);

        OnTourretDead?.Invoke(_tentacle.GetDeadTentacleDamage());
        if (aliveTouretts.Count == 0)
            OnAllTourretsDead?.Invoke();
    }
    #endregion

    #region API
    /// <summary>
    /// Funzione che blocca/sblocca la mira della torretta passata come parametro
    /// </summary>
    /// <param name="_tourretIndex"></param>
    /// <param name="_lockAim"></param>
    public void LockAim(int _tourretIndex, bool _lockAim)
    {
        if (!CheckTourretIndex(_tourretIndex))
            return;

        aliveTouretts[_tourretIndex].SetCanAim(!_lockAim);
    }

    /// <summary>
    /// Funzione che chiama lo shoot della torretta passata come parametro
    /// </summary>
    /// <param name="_tourretIndex"></param>
    public void Shoot(int _tourretIndex)
    {
        if (!CheckTourretIndex(_tourretIndex))
            return;

        aliveTouretts[_tourretIndex].Shoot();
    }

    /// <summary>
    /// Funzione che controlla se l'index della torretta passato è disponibile
    /// </summary>
    /// <param name="_tourretIndex"></param>
    /// <returns></returns>
    public bool CheckTourretIndex(int _tourretIndex)
    {
        return _tourretIndex >= 0 && _tourretIndex <= aliveTouretts.Count - 1;
    }

    #region Setter
    /// <summary>
    /// Funzione che imposta se le torrette possono sparare
    /// </summary>
    /// <param name="_canAim"></param>
    public void SetCanAim(bool _canAim)
    {
        canAim = _canAim;
    }
    #endregion
    #endregion
}
