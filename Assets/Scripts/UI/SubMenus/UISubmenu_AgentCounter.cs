using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Submenu che che gestisce la ui del counter dei pitottini
/// </summary>
public class UISubmenu_AgentCounter : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI agentCounterText;

	private int currentAgents;
	private GroupController groupCtrl;

	/// <summary>
	/// Funzione che esegue il Setup
	/// </summary>
	/// <param name="_groupCtrl"></param>
	public void Setup(GroupController _groupCtrl)
	{
		groupCtrl = _groupCtrl;

		groupCtrl.OnAgentSpawn += HandleOnAgentSpawn;
		groupCtrl.OnAgentRemoved += HandleOnAgentDead;
		groupCtrl.OnGroupDead += HandleOnGroupDead;

		currentAgents = groupCtrl.GetGroupCont();
		agentCounterText.text = currentAgents.ToString();
	}

	#region Hanlder
	/// <summary>
	/// Funzione che ascolta l'evento di morte del gruppo
	/// </summary>
	private void HandleOnGroupDead()
	{
		gameObject.SetActive(false);
	}

	/// <summary>
	/// Funzione che ascolta l'evento di morte di un agent
	/// </summary>
	private void HandleOnAgentDead(AgentController obj)
	{
		currentAgents--;
		agentCounterText.text = currentAgents.ToString();
	}

	/// <summary>
	/// Funzione che ascolta l'evento di spawn di un agent
	/// </summary>
	private void HandleOnAgentSpawn(AgentController obj)
	{
		currentAgents++;
		agentCounterText.text = currentAgents.ToString();
	}
	#endregion

	private void OnDestroy()
	{
		if (groupCtrl != null)
		{
			groupCtrl.OnAgentSpawn -= HandleOnAgentSpawn;
			groupCtrl.OnAgentRemoved -= HandleOnAgentDead;
			groupCtrl.OnGroupDead -= HandleOnGroupDead;
		}
	}
}
