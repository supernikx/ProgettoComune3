﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Classe che gestisce lo sparo del gruppo
/// </summary>
public class GroupShootController : MonoBehaviour
{
    #region Actions
    /// <summary>
    /// Evento che notifica lo start della ricarica e passa come parametro il tempo di ricarica
    /// </summary>
    public Action<float> OnReloadingStart;
    /// <summary>
    /// Evento che notifica la ricarica in corso e passa come parametro il tempo trascorso
    /// </summary>
    public Action<float> OnReloadingInProgress;
    /// <summary>
    /// Evento che noticia la fine della ricarica
    /// </summary>
    public Action OnReloadingEnd;
    #endregion

    [Header("Shoot Settings")]
    //Referenza al prefab del proiettile
    [SerializeField]
    private PlayerBulletController bulletPrefab;
    //Offset di altezza di sparo rispetto all posizione del gruppo
    [SerializeField]
    private float shootHeight;

    [Header("Reloading Settings")]
    //Tempo di ricarica
    [SerializeField]
    private float reloadingTime;
    //Numero di personaggi che ricarica
    [SerializeField]
    private float reloadingAgents;
    //Se true la ricarica può essere interrotta altrimenti no
    [SerializeField]
    private bool canBeInterruped;

    /// <summary>
    /// Riferimento al group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al group movement controller
    /// </summary>
    private GroupMovementController groupMovementCtrl;
    /// <summary>
    /// Riferimento al group size controller
    /// </summary>
    private GroupSizeController groupSizeCtrl;
    /// <summary>
    /// Riferimento al PlayerInput
    /// </summary>
    private PlayerInput playerInput;
    /// <summary>
    /// bool che identifica se è possibile sparare o no
    /// </summary>
    private bool canShoot;
    /// <summary>
    /// Riferimento al vettore di sparo
    /// </summary>
    private Vector3 shootVector;
    /// <summary>
    /// Riferimento all'aim feedbakc
    /// </summary>
    private AimArrowFeedback aimFeedback;
    /// <summary>
    /// Riferimento alla Coroutine di Reloading
    /// </summary>
    private IEnumerator reloadingRoutine;

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    public void Setup(GroupController _groupCtrl)
    {        
        groupCtrl = _groupCtrl;
        groupMovementCtrl = groupCtrl.GetGroupMovementController();
        groupSizeCtrl = groupCtrl.GetGroupSizeController();
        playerInput = groupCtrl.GetPlayerInput();
        aimFeedback = groupCtrl.GetGroupFeedbackController().GetAimArrow();

        groupCtrl.OnGroupDead += EndReloading;
        canShoot = true;
    }

    private void Update()
    {
        if (!groupCtrl.IsSetuppedAndEnabled() || !canShoot)
        {
            aimFeedback.DisableArrow();
            return;
        }

        aimFeedback.EnableArrow();
        aimFeedback.UpdateArrow(groupCtrl.GetGroupCenterPoint(), shootVector);
    }

    /// <summary>
    /// Funzione chiamata alla mira dal PlayerInput
    /// </summary>
    public void OnLook(InputValue _value)
    {
        if (!groupCtrl.IsSetuppedAndEnabled() || !canShoot)
            return;

        if (playerInput.currentControlScheme == "Gamepad")
        {
            Vector2 newAim = _value.Get<Vector2>();
            if (newAim.x != 0 || newAim.y != 0)
            {
                shootVector.x = newAim.x;
                shootVector.z = newAim.y;
            }
        }
        else
        {
            Vector3 fixedMousePosition = new Vector3(_value.Get<Vector2>().x, _value.Get<Vector2>().y, 0);
            Vector3 screenCenterPosition = Camera.main.WorldToScreenPoint(groupCtrl.GetGroupCenterPoint());
            screenCenterPosition.z = 0;
            Vector3 mouseDirection = (fixedMousePosition - screenCenterPosition).normalized;
            shootVector = new Vector3(mouseDirection.x, 0, mouseDirection.y);
        }
    }

    /// <summary>
    /// Funzione chiamata allo sparo dal PlayerInput
    /// </summary>
    public void OnShoot()
    {
        if (!CanShoot())
            return;

        ShootAgent();
    }

    /// <summary>
    /// Funzione chiamata alla ricarica dal PlayerInput
    /// </summary>
    public void OnReloading()
    {
        if (!CanShoot())
            return;

        ReloadAgent();
    }

    /// <summary>
    /// Funzione che si occupa di sparare 
    /// </summary>
    private void ShootAgent()
    {
        if (groupCtrl.RemoveRandomAgent())
        {
            Vector3 shootPoint = groupCtrl.GetGroupCenterPoint();
            shootPoint.y = shootPoint.y + shootHeight;
            PlayerBulletController newBullet = PoolManager.instance.GetPooledObject(ObjectTypes.PlayerBullet, gameObject).GetComponent<PlayerBulletController>();
            if (newBullet != null)
            {
                newBullet.transform.SetPositionAndRotation(shootPoint, Quaternion.LookRotation(shootVector.normalized));
                newBullet.Setup();
            }
        }
    }

    /// <summary>
    /// Funzione che si occupa di ricaricare
    /// </summary>
    private void ReloadAgent()
    {
        if (!groupCtrl.IsGroupFull())
        {
            if (!canBeInterruped)
                groupCtrl.GetGroupMovementController().SetCanMove(false);
            else
            {
                groupMovementCtrl.OnGroupMove += EndReloading;
                groupMovementCtrl.ResetMovementVelocity();
            }

            canShoot = false;
            OnReloadingStart?.Invoke(reloadingTime);

            reloadingRoutine = ReloadingCoroutine();
            StartCoroutine(reloadingRoutine);
        }
    }

    /// <summary>
    /// Funzione che reimposta i valori come prima la ricarica
    /// </summary>
    private void EndReloading()
    {
        if (reloadingRoutine != null)
            StopCoroutine(reloadingRoutine);

        if (!canBeInterruped)
            groupCtrl.GetGroupMovementController().SetCanMove(true);
        else
            groupMovementCtrl.OnGroupMove -= EndReloading;

        canShoot = true;
        OnReloadingEnd?.Invoke();
    }

    /// <summary>
    /// Coroutine che gestisce la ricarica
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReloadingCoroutine()
    {
        WaitForFixedUpdate wffu = new WaitForFixedUpdate();
        float timer = 0f;

        while (timer < reloadingTime)
        {
            timer += Time.deltaTime;
            OnReloadingInProgress?.Invoke(timer);
            yield return wffu;
        }

        for (int i = 0; i < reloadingAgents; i++)
            if (!groupCtrl.IsGroupFull())
                groupCtrl.InstantiateNewAgent();

        EndReloading();
    }

    /// <summary>
    /// Funzione che fa tutti i controlli necessari e ritorna se si può sparare
    /// </summary>
    /// <returns></returns>
    private bool CanShoot()
    {
        return (groupCtrl.IsSetuppedAndEnabled() && canShoot && !groupSizeCtrl.IsGrouping());
    }

    private void OnDisable()
    {
        if (groupCtrl != null)
            groupCtrl.OnGroupDead -= EndReloading;

        if (groupMovementCtrl != null)
            groupMovementCtrl.OnGroupMove -= EndReloading;
    }
}
