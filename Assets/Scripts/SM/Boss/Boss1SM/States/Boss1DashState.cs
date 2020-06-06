using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di Dash del Boss 1
/// </summary>
public class Boss1DashState : Boss1StateBase
{
    [Header("State Settings")]
    //Velocità di movimento del Boss
    [SerializeField]
    private float movementSpeed;
    //Distanza massima percorribile dal movimento
    [SerializeField]
    private float maxMoveDistance;
    //Attiva/Disattiva il Trail durante il dash
    [SerializeField]
    private bool leaveTrail = true;

    [Header("Feedback")]
    //suono di dash del boss
    [SerializeField]
    private string dashSoundID = "dash";

    /// <summary>
    /// Riferimento al GroupController
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al BossController
    /// </summary>
    private Boss1Controller bossCtrl;
    /// <summary>
    /// Riferimento al BossCollisionController
    /// </summary>
    private BossCollisionController collisionCtrl;
    /// <summary>
    /// Riferimento al LifeController
    /// </summary>
    private BossLifeController lifeCtrl;
    /// <summary>
    /// Riferimento al BossPhaseController
    /// </summary>
    private Boss1PhaseController bossPhaseCtrl;
    /// <summary>
    /// Riferimento al TrailController
    /// </summary>
    private Boss1TrailController trailController;
    /// <summary>
    /// Riferimento al sound controller
    /// </summary>
    private SoundController soundCtrl;
    /// <summary>
    /// Distanza percorsa
    /// </summary>
    private float distanceTraveled;
    /// <summary>
    /// Vecchio dato distanza percorsa
    /// </summary>
    private float oldDistanceTraveled;

    public override void Enter()
    {
        groupCtrl = context.GetLevelManager().GetGroupController();
        bossCtrl = context.GetBossController();
        lifeCtrl = bossCtrl.GetBossLifeController();
        soundCtrl = bossCtrl.GetSoundController();
        collisionCtrl = bossCtrl.GetBossCollisionController();
        bossPhaseCtrl = bossCtrl.GetBossPhaseController();
        trailController = bossCtrl.GetBossTrailController();

        distanceTraveled = 0;
        LookAtPosition(groupCtrl.GetGroupCenterPoint());
        soundCtrl.PlayAudioClipOnTime(dashSoundID);

        if (leaveTrail)
            trailController.InstantiateNewTrail();

        bossPhaseCtrl.OnSecondPhaseStart += HandleOnSecondPhaseStart;
        bossPhaseCtrl.OnThirdPhaseStart += HandleOnThirdPhaseStart;
        lifeCtrl.OnBossDead += HandleOnBossDead;
        collisionCtrl.OnObstacleHit += HandleOnObstacleHit;
        collisionCtrl.OnAgentHit += HandleOnAgentHit;
    }

    public override void Tick()
    {
        oldDistanceTraveled = distanceTraveled;
        if (distanceTraveled >= maxMoveDistance)
        {
            float remamingDistance = maxMoveDistance - oldDistanceTraveled;
            Vector3 newPos = Vector3.MoveTowards(bossCtrl.transform.position, bossCtrl.transform.position + bossCtrl.transform.forward, remamingDistance);
            if (!collisionCtrl.CheckCollision(newPos))
                bossCtrl.transform.position = newPos;

            Complete();
        }
        else
        {
            Vector3 oldPos = bossCtrl.transform.position;
            Vector3 newPos = Vector3.MoveTowards(bossCtrl.transform.position, bossCtrl.transform.position + bossCtrl.transform.forward, movementSpeed * Time.deltaTime);
            if (collisionCtrl.CheckCollision(newPos))
                HandleOnObstacleHit(null);
            else
            {
                bossCtrl.transform.position = newPos;
                distanceTraveled += Vector3.Distance(bossCtrl.transform.position, oldPos);

                if (leaveTrail)
                    trailController.UpdateLastTrail();
            }
        }
    }


    /// <summary>
    /// Funzione che fa guardare al Boss la posizione passata come parametro
    /// </summary>
    /// <param name="_lookPos"></param>
    private void LookAtPosition(Vector3 _lookPos)
    {
        _lookPos.y = bossCtrl.transform.position.y;
        bossCtrl.transform.LookAt(_lookPos, Vector3.up);
    }

    #region Handlers
    /// <summary>
    /// Funzione che gestisce l'evento collisionCtrl.OnAgentHit
    /// <param name="obj"></param>
    private void HandleOnAgentHit(AgentController _agent)
    {
        groupCtrl.RemoveAgent(_agent, true);
    }

    /// <summary>
    /// Funzione che gestisce l'evento ollisionCtrl.OnObstacleHit
    /// <param name="obj"></param>
    private void HandleOnObstacleHit(GameObject _obstacle)
    {
        distanceTraveled = maxMoveDistance;
    }

    /// <summary>
    /// Funzione che gestisce l'evento di morte del Boss
    /// </summary>
    private void HandleOnBossDead()
    {
        Complete(1);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di inizio secondo fase del Boss
    /// </summary>
    private void HandleOnSecondPhaseStart()
    {
        Complete(2);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di inizio terza fase del Boss
    /// </summary>
    private void HandleOnThirdPhaseStart()
    {
        Complete(3);
    }
    #endregion

    public override void Exit()
    {
        if (collisionCtrl != null)
        {
            collisionCtrl.OnObstacleHit -= HandleOnObstacleHit;
            collisionCtrl.OnAgentHit -= HandleOnAgentHit;
        }

        if (lifeCtrl != null)
            lifeCtrl.OnBossDead -= HandleOnBossDead;

        if (bossPhaseCtrl != null)
        {
            bossPhaseCtrl.OnSecondPhaseStart -= HandleOnSecondPhaseStart;
            bossPhaseCtrl.OnThirdPhaseStart -= HandleOnThirdPhaseStart;
        }

        if (trailController != null && leaveTrail)
            trailController.EndTrail();

        bossCtrl = null;
        collisionCtrl = null;
        bossPhaseCtrl = null;
        lifeCtrl = null;
        distanceTraveled = 0;
    }
}
