using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di salto del Boss
/// </summary>
public class Boss1JumpState : Boss1StateBase
{
	#region Actions

	#endregion

	[Header("State Settings")]
    //Altezza del salto
    [SerializeField]
    private float jumpHeight;
    //Velocità di sollevamento
    [SerializeField]
    private float raisingSpeed;
    //Velocità di movimento
    [SerializeField]
    private float movementSpeed;
    //Velocità di atterraggio
    [SerializeField]
    private float landingSpeed;

    /// <summary>
    /// Enumeratore delle 3 fasi dello stato
    /// </summary>
    private enum StatePhases
    {
        Raising,
        Moving,
        Landing,
    }
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
    /// Riferimento al RigidBody
    /// </summary>
    private Rigidbody rb;
    /// <summary>
    /// Variabile che identifica la fase attuale dello stato
    /// </summary>
    private StatePhases currentStatePhase;
    /// <summary>
    /// Posizione sulla Y iniziale del Boss
    /// </summary>
    private float startHeigth;
    /// <summary> 
    /// Posizione massima sulla Y del Boss
    /// </summary>
    private float maxHeigth;
    /// <summary>
    /// Distanza da percorrere
    /// </summary>
    private float distanceToTravel;
    /// <summary>
    /// Distanza percorsa
    /// </summary>
    private float distanceTraveled;
    /// <summary>
    /// Distanza percorsa
    /// </summary>
    private float oldDistanceTraveled;

    public override void Enter()
    {
        groupCtrl = context.GetLevelManager().GetGroupController();
        bossCtrl = context.GetBossController();
        lifeCtrl = bossCtrl.GetBossLifeController();
        collisionCtrl = bossCtrl.GetBossCollisionController();
        rb = collisionCtrl.GetRigidBody();
        bossPhaseCtrl = bossCtrl.GetBossPhaseController();

        rb.useGravity = false;
        startHeigth = bossCtrl.transform.position.y;
        maxHeigth = startHeigth + jumpHeight;
        distanceTraveled = 0;
        distanceToTravel = 0;
        currentStatePhase = StatePhases.Raising;

        bossPhaseCtrl.OnSecondPhaseStart += HandleOnSecondPhaseStart;
        bossPhaseCtrl.OnThirdPhaseStart += HandleOnThirdPhaseStart;
        lifeCtrl.OnBossDead += HandleOnBossDead;
        collisionCtrl.OnObstacleHit += HandleOnObstacleHit;
        collisionCtrl.OnAgentHit += HandleOnAgentHit;
    }

    public override void Tick()
    {
        switch (currentStatePhase)
        {
            case StatePhases.Raising:
                RaisingBehaviour();
                break;
            case StatePhases.Moving:
                MovingBehaviour();
                break;
            case StatePhases.Landing:
                LandingBehaviour();
                break;
        }
    }

    /// <summary>
    /// Funzione che esegue il comportamento della fase di Raising
    /// </summary>
    private void RaisingBehaviour()
    {
        if (bossCtrl.transform.position.y >= maxHeigth)
        {
            Vector3 landingPosition = groupCtrl.GetGroupCenterPoint();
            LookAtPosition(landingPosition);
            Vector3 fakeBossPos = bossCtrl.transform.position;
            fakeBossPos.y = landingPosition.y;
            distanceToTravel = Vector3.Distance(landingPosition, fakeBossPos);
            currentStatePhase = StatePhases.Moving;
        }
        else
            bossCtrl.transform.position = Vector3.MoveTowards(bossCtrl.transform.position, bossCtrl.transform.position + Vector3.up, raisingSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Funzione che esegue il comportamento della fase di Moving
    /// </summary>
    private void MovingBehaviour()
    {
        oldDistanceTraveled = distanceTraveled;
        if (distanceTraveled >= distanceToTravel)
        {
            float remamingDistance = Mathf.Abs(distanceToTravel - oldDistanceTraveled);
            Vector3 newPos = Vector3.MoveTowards(bossCtrl.transform.position, bossCtrl.transform.position + bossCtrl.transform.forward, remamingDistance);
            if (!collisionCtrl.CheckCollision(newPos))
                bossCtrl.transform.position = newPos;

            currentStatePhase = StatePhases.Landing;
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
            }
        }
    }

    /// <summary>
    /// Funzione che esegue il comportamento della fase di Landing
    /// </summary>
    private void LandingBehaviour()
    {
        if (startHeigth >= bossCtrl.transform.position.y)
        {
            Vector3 fiexdBossPosition = bossCtrl.transform.position;
            fiexdBossPosition.y = startHeigth;
            bossCtrl.transform.position = fiexdBossPosition;
            Complete();
        }
        else
            bossCtrl.transform.position = Vector3.MoveTowards(bossCtrl.transform.position, bossCtrl.transform.position - Vector3.up, landingSpeed * Time.deltaTime);
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
        distanceTraveled = distanceToTravel;
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

        if (rb != null)
            rb.useGravity = true;

        bossCtrl = null;
        collisionCtrl = null;
        bossPhaseCtrl = null;
        lifeCtrl = null;
        currentStatePhase = StatePhases.Raising;
        distanceTraveled = 0;
        distanceToTravel = 0;
        startHeigth = 0;
        maxHeigth = 0;
    }
}
