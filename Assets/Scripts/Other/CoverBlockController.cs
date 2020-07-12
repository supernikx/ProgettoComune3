using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Classe che gestisce il cover block
/// </summary>
public class CoverBlockController : MonoBehaviour
{
	[Header("CoverBlock Reference")]
	//Riferimento alla barrier che attiva il coverblock
	[SerializeField]
	private GameObject coverBlockBarrier;
	[SerializeField]
	private GameObject pilastro;
	[SerializeField]
	private Image fillImage;
	[SerializeField]
	private Gradient gradingColor;
	[SerializeField]
	private TextMeshProUGUI barrierAgentText;

	[Header("Feedback")]
	//suono di coverblock up
	[SerializeField]
	private string coverblocSoundIDkUP = "coverUP";
	//suono di coverblock down
	[SerializeField]
	private string coverblockSoundIDDown = "coverDown";

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
	/// Agent necessari ad attivare il cover block
	/// </summary>
	private int coverBlockNeedAgents;
	/// <summary>
	/// Current agents
	/// </summary>
	private List<AgentController> currentAgents = new List<AgentController>();
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
	/// Riferiemento al sound controller
	/// </summary>
	private SoundController soundCtrl;
	/// <summary>
	/// Riferiemento al material del coverblock
	/// </summary>
	private Material coverblockMaterial;
	/// <summary>
	/// Riferiemento al material del coverblock
	/// </summary>
	private Material pilastroMaterial;

	/// <summary>
	/// Funzione di Setup
	/// </summary>
	/// <param name="_coverBlockNeedAgents"></param>
	/// <param name="_coverBlockDuration"></param>
	public void Setup(int _coverBlockNeedAgents, float _coverBlockDuration, float _coverBlockHeatSpeed, float _coverBlockResetSpeed)
	{
		soundCtrl = GetComponent<SoundController>();
		coverBlockNeedAgents = _coverBlockNeedAgents;
		coverBlockDuration = _coverBlockDuration;
		coverBlockHeatSpeed = _coverBlockHeatSpeed;
		coverBlockResetSpeed = _coverBlockResetSpeed;

		coverblockMaterial = coverBlockBarrier.GetComponent<MeshRenderer>().material;
		pilastroMaterial = pilastro.GetComponent<MeshRenderer>().material;

		SetText();
		isCooldown = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Agent"))
		{
			AgentController agentCtrl = other.GetComponent<AgentController>();
			if (!currentAgents.Contains(agentCtrl))
				currentAgents.Add(agentCtrl);

			if (currentAgents.Count >= coverBlockNeedAgents)
			{
				if (!isTriggered && !isCooldown)
					soundCtrl.PlayAudioClipOnTime(coverblocSoundIDkUP);
				isTriggered = true;
			}
		}

		if (enable)
			SetText();
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
				soundCtrl.PlayAudioClipOnTime(coverblockSoundIDDown);
				isCooldown = true;
			}
		}
		else
		{
			coverBlockBarrier.SetActive(false);

			if (coverBlockTimer > 0)
			{
				coverBlockTimer = Mathf.Clamp(coverBlockTimer - Time.deltaTime, 0, coverBlockDuration);
			}
			else
			{
				isCooldown = false;

				if (isTriggered)
					soundCtrl.PlayAudioClipOnTime(coverblocSoundIDkUP);
			}
		}

		if (currentAgents.Count == 0)
			isTriggered = false;

		coverblockMaterial.SetColor("Color_A9F326B", gradingColor.Evaluate(coverBlockTimer / coverBlockDuration));
		//fillImage.fillAmount = coverBlockTimer / coverBlockDuration;
	}

	private void OnTriggerStay(Collider other)
	{
		if (currentAgents.Count >= coverBlockNeedAgents)
		{
			//
			isTriggered = true;
		}
		if (currentAgents.Count < coverBlockNeedAgents)
		{
			//soundCtrl.PlayAudioClipOnTime(coverblockDown);
			isTriggered = false;
			//isCooldown = true;
		}

		if (enable)
			SetText();
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Agent"))
		{
			AgentController agentCtrl = other.GetComponent<AgentController>();
			currentAgents.Remove(agentCtrl);

			if (currentAgents.Count < coverBlockNeedAgents)
			{
				if (isTriggered)
					soundCtrl.PlayAudioClipOnTime(coverblockSoundIDDown);
				isTriggered = false;
				//isCooldown = true;
			}
		}

		if (enable)
			SetText();
	}

	/// <summary>
	/// Funzione che imposta il testo
	/// </summary>
	/// <param name="_currentAgents"></param>
	private void SetText()
	{
		if (currentAgents.Count >= coverBlockNeedAgents)
			pilastroMaterial.SetColor("_EmissiveColor", Color.green);
		else
			pilastroMaterial.SetColor("_EmissiveColor", Color.red);

		//barrierAgentText.text = currentAgents.Count + "/" + coverBlockNeedAgents;
	}

	#region API
	/// <summary>
	/// Funzione che ritorna se il cover block sta coperendo
	/// </summary>
	/// <returns></returns>
	public bool IsCovering()
	{
		return coverBlockBarrier.activeInHierarchy;
	}

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
			currentAgents.Clear();
		}
	}

	/// <summary>
	/// Funzione che ritorna la lista degli agent
	/// </summary>
	/// <returns></returns>
	public List<AgentController> GetAgentList()
	{
		return currentAgents;
	}
	#endregion
}
