using System;
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
	public Action OnReloadingStart;
	/// <summary>
	/// Evento che notifica la ricarica in corso e passa come parametro il tempo trascorso
	/// </summary>
	public Action OnReloadingInProgress;
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

	[Header("Charge Settings")]
	[SerializeField]
	private float chargeTime;

	[Header("Reloading Settings")]
	//Range in cui deve entrare l'orb per risultare ricaricato
	[SerializeField]
	private float reloadRange;
	//Layer dell'orb
	[SerializeField]
	private LayerMask orbLayer;
	//Se true la ricarica può essere interrotta altrimenti no
	[SerializeField]
	private bool canBeInterruped;

	[Header("Feedback")]
	//suono di ricarica dei pitottini
	[SerializeField]
	private string reloadingSoundID = "reloading";
	//suono di sparo dei pitottini
	[SerializeField]
	private string shootSoundID = "shoot";

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
	/// Riferimento al group orb controller
	/// </summary>
	private GroupOrbController groupOrbCtrl;
	/// <summary>
	/// Riferimento al group ffeedback controller
	/// </summary>
	private GroupFeedbackController groupFeedbackCtrl;
	/// <summary>
	/// Riferimento al sound controller
	/// </summary>
	private SoundController soundCtrl;
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

	private bool isShootButtonDown;
	private float shootButtonUpDownDelay;

	/// <summary>
	/// Funzione che esegue il Setup
	/// </summary>
	/// <param name="_groupCtrl"></param>
	public void Setup(GroupController _groupCtrl)
	{
		groupCtrl = _groupCtrl;
		groupMovementCtrl = groupCtrl.GetGroupMovementController();
		groupSizeCtrl = groupCtrl.GetGroupSizeController();
		groupOrbCtrl = groupCtrl.GetGroupOrbController();
		soundCtrl = groupCtrl.GetSoundController();
		playerInput = groupCtrl.GetPlayerInput();
		groupFeedbackCtrl = groupCtrl.GetGroupFeedbackController();
		aimFeedback = groupFeedbackCtrl.GetAimArrow();

		groupCtrl.OnGroupDead += EndReloading;
		canShoot = true;
	}

	private void Update()
	{
		if (!groupCtrl.IsSetuppedAndEnabled() || !canShoot)
		{
			aimFeedback.DisableArrow();
			shootButtonUpDownDelay = 0f;
			isShootButtonDown = false;
			return;
		}

		if (isShootButtonDown)
			shootButtonUpDownDelay += Time.deltaTime;
		else
			shootButtonUpDownDelay = 0;

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
	public void OnShoot(InputValue _value)
	{
		if (!CanShoot())
			return;

		bool buttonPressed = _value.Get<float>() > 0;

		if (isShootButtonDown && !buttonPressed)
		{
			if (shootButtonUpDownDelay > chargeTime)
				ShootCharge();
			else
				ShootAgent();
		}

		isShootButtonDown = buttonPressed;
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
				soundCtrl.PlayAudioClipOnTime(shootSoundID);
				newBullet.transform.SetPositionAndRotation(shootPoint, Quaternion.LookRotation(shootVector.normalized));
				newBullet.Setup();
			}
		}
	}

	/// <summary>
	/// Funzione che si occupa di sparare il colpo caricato
	/// </summary>
	private void ShootCharge()
	{
		Vector3 shootPoint = groupCtrl.GetGroupCenterPoint();
		shootPoint.y = shootPoint.y + shootHeight;

		Quaternion shootPointRotation = Quaternion.LookRotation(shootVector.normalized);

		for (int i = 0, rot = -15; i < 3; i++, rot += 15)
		{
			if (groupCtrl.RemoveRandomAgent())
			{
				PlayerBulletController newBullet = PoolManager.instance.GetPooledObject(ObjectTypes.PlayerBullet, gameObject).GetComponent<PlayerBulletController>();
				if (newBullet != null)
				{
					Quaternion shootRotation = shootPointRotation * Quaternion.Euler(0, rot, 0);
					newBullet.transform.SetPositionAndRotation(shootPoint, shootRotation);
					newBullet.Setup();
				}
			}
		}
	}

	/// <summary>
	/// Funzione che si occupa di ricaricare
	/// </summary>
	private void ReloadAgent()
	{
		if (!groupCtrl.IsGroupFull() && groupOrbCtrl.CanReload())
		{
			if (!canBeInterruped)
				groupCtrl.GetGroupMovementController().SetCanMove(false);
			else
			{
				groupMovementCtrl.OnGroupMove += EndReloading;
				groupMovementCtrl.ResetMovementVelocity();
			}

			canShoot = false;
			OnReloadingStart?.Invoke();

			reloadingRoutine = ReloadingCoroutine();
			StartCoroutine(reloadingRoutine);
		}
	}

	/// <summary>
	/// Coroutine che gestisce la ricarica
	/// </summary>
	/// <returns></returns>
	private IEnumerator ReloadingCoroutine()
	{
		WaitForFixedUpdate wffu = new WaitForFixedUpdate();
		Vector3 groupCenterPos;
		groupFeedbackCtrl.SetReloadVFX(true);
		soundCtrl.PlayClipLoop(reloadingSoundID);

		while (groupOrbCtrl.CanReload() && !groupCtrl.IsGroupFull())
		{
			groupCenterPos = groupCtrl.GetGroupCenterPoint();
			groupOrbCtrl.MoveOrbs(groupCenterPos);
			Collider[] htiOrbs = new Collider[10];
			htiOrbs = Physics.OverlapSphere(groupCenterPos, reloadRange, orbLayer);
			for (int i = 0; i < htiOrbs.Length; i++)
			{
				if (htiOrbs[i] == null)
					continue;

				OrbController orbCtrl = htiOrbs[i].GetComponent<OrbController>();
				if (orbCtrl != null)
				{
					orbCtrl.Destroy();
					if (!groupCtrl.IsGroupFull())
						groupCtrl.InstantiateNewAgent();
				}
			}

			OnReloadingInProgress?.Invoke();
			yield return wffu;
		}
		
		soundCtrl.StopClipLoop(reloadingSoundID);
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

	#region API
	/// <summary>
	/// Funzione che ritorna se si sta caricando
	/// </summary>
	/// <returns></returns>
	public bool IsReloading()
	{
		return !canShoot;
	}

	/// <summary>
	/// Funzione che reimposta i valori come prima la ricarica
	/// </summary>
	public void EndReloading()
	{
		if (reloadingRoutine != null)
		{
			StopCoroutine(reloadingRoutine);
			groupFeedbackCtrl.SetReloadVFX(false);
		}

		if (!canBeInterruped)
			groupCtrl.GetGroupMovementController().SetCanMove(true);
		else
			groupMovementCtrl.OnGroupMove -= EndReloading;

		groupFeedbackCtrl.SetReloadVFX(false);
		canShoot = true;
		OnReloadingEnd?.Invoke();
	}
	#endregion

	private void OnDisable()
	{
		if (groupCtrl != null)
			groupCtrl.OnGroupDead -= EndReloading;

		if (groupMovementCtrl != null)
			groupMovementCtrl.OnGroupMove -= EndReloading;
	}
}
