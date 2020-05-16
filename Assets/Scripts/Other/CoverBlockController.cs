using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il cover block
/// </summary>
public class CoverBlockController : MonoBehaviour
{
	[Header("CoverBlock Reference")]
	//Riferimento alla barrier che attiva il coverblock
	[SerializeField]
	private GameObject coverBlockBarrier;

	/// <summary>
	/// Identifica se il cover block è attivo
	/// </summary>
	private bool enable;
	/// <summary>
	/// Durata del cover block una volta attivato
	/// </summary>
	private float coverBlockDuration;
	/// <summary>
	/// Velocità di surriscaldamento del cover block
	/// </summary>
	private float coverBlockHeatSpeed;
	/// <summary>
	/// Velocità di reset del cover block
	/// </summary>
	private float coverBlockResetSpeed;
	/// <summary>
	/// Aegent necessari ad attivare il cover block
	/// </summary>
	private int coverBlockNeedAgents;
	/// <summary>
	/// Aegent attuali sul cover block
	/// </summary>
	private int coverBlockCurrentAgents;
	/// <summary>
	/// Riferimento alla coroutine del cover block
	/// </summary>
	private IEnumerator coverBlockRoutine;
	/// <summary>
	/// Bool che identifica se il coverblock è stato triggerato
	/// </summary>
	private bool isTriggered;
	/// <summary>
	/// Bool che identifica se il coverblock è in cooldown
	/// </summary>
	private bool isCooldown;
	/// <summary>
	/// Timer del coverblock
	/// </summary>
	private float coverBlockTimer;

	/// <summary>
	/// Funzione di Setup
	/// </summary>
	/// <param name="_coverBlockNeedAgents"></param>
	/// <param name="_coverBlockDuration"></param>
	public void Setup(int _coverBlockNeedAgents, float _coverBlockDuration, float _coverBlockHeatSpeed, float _coverBlockResetSpeed)
	{
		coverBlockNeedAgents = _coverBlockNeedAgents;
		coverBlockDuration = _coverBlockDuration;
		coverBlockHeatSpeed = _coverBlockHeatSpeed;
		coverBlockResetSpeed = _coverBlockResetSpeed;

		isCooldown = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Agent"))
		{
			coverBlockCurrentAgents++;
			if (coverBlockCurrentAgents >= coverBlockNeedAgents)
			{
				isTriggered = true;
			}
		}
	}

	private void Update()
	{
		if (!enable)
			return;

		if (isTriggered && !isCooldown)
		{
			coverBlockBarrier.SetActive(true);
			coverBlockTimer += Time.deltaTime;
			if (coverBlockTimer >= coverBlockDuration)
			{
				isCooldown = true;
			}
		}
		else
		{
			coverBlockBarrier.SetActive(false);

			if (coverBlockTimer > 0)
			{
				coverBlockTimer = Mathf.Clamp(coverBlockTimer - Time.deltaTime, 0, coverBlockDuration);
				if (coverBlockTimer == 0)
					isCooldown = false;
			}
			else
			{
				isCooldown = false;
			}
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (coverBlockCurrentAgents >= coverBlockNeedAgents)
		{
			isTriggered = true;
		}
		if (coverBlockCurrentAgents < coverBlockNeedAgents)
		{
			isTriggered = false;
			isCooldown = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Agent"))
		{
			coverBlockCurrentAgents--;
			if (coverBlockCurrentAgents < coverBlockNeedAgents)
			{
				isTriggered = false;
				isCooldown = true;
			}
		}
	}

	#region API
	/// <summary>
	/// Funzione che attiva/disattiva il cover block
	/// </summary>
	/// <param name="_enable"></param>
	public void Enable(bool _enable)
	{
		enable = _enable;
		if (enable)
		{
			coverBlockBarrier.SetActive(false);
			isCooldown = false;
		}
		else
		{
			if (coverBlockRoutine != null)
				StopCoroutine(coverBlockRoutine);

			gameObject.SetActive(false);
		}
	}
	#endregion
}
