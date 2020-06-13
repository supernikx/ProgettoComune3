﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce la parte grafica del Boss 1
/// </summary>
public class Boss1GraphicController : MonoBehaviour
{
	/// <summary>
	/// Riferimento all'animator
	/// </summary>
	private Animator anim;
	/// <summary>
	/// Riferiemento al boss controller
	/// </summary>
	private Boss1Controller bossCtrl;
	/// <summary>
	/// Riferiemento allo state machine controller del boss 1
	/// </summary>
	private Boss1SMController smCtrl;

	/// <summary>
	/// Funzione di Setup
	/// </summary>
	public void Setup(Boss1Controller _bossCtrl, Boss1SMController _smCtrl)
	{
		bossCtrl = _bossCtrl;
		smCtrl = _smCtrl;
		anim = GetComponent<Animator>();

		smCtrl.OnStateEnter += HandleOnStateEnter;
		smCtrl.OnStateExit += HandlOnStateExit;
	}

	/// <summary>
	/// Funzione che gestisce l'evento d'ingresso degli stati
	/// </summary>
	/// <param name="obj"></param>
	private void HandleOnStateEnter(IState obj)
	{
		switch (obj.GetID())
		{
			case "Jump":
				anim.ResetTrigger("Idle");
				anim.SetTrigger("Jump");
				break;
			case "Shoot":
				anim.ResetTrigger("Idle");
				anim.SetTrigger("Shoot");
				break;
			case "Dash":
				anim.ResetTrigger("Idle");
				anim.SetTrigger("Dash");
				break;
			case "Death":
				anim.SetTrigger("Death");
				break;
			default:
				anim.SetTrigger("Idle");
				break;
		}
	}

	/// <summary>
	/// Funzione che gestisce l'evento di uscita dagli stati
	/// </summary>
	/// <param name="obj"></param>
	private void HandlOnStateExit(IState obj)
	{
		//switch (obj.GetID())
		//{
		//	case "Waiting":
		//	case "Phase1":
		//	case "Phase2":
		//	case "Phase3":
		//		break;
		//	default:
		//		anim.SetTrigger("Idle");
		//		break;
		//}
	}

	/// <summary>
	/// Funzione che gestisce l'evento di fine dell'animazione di morte del boss
	/// </summary>
	private void HandlOnDeathAnimationEnd()
	{
		bossCtrl.DisableBoss();
	}

	private void OnDisable()
	{
		if (smCtrl != null)
		{
			smCtrl.OnStateEnter -= HandleOnStateEnter;
			smCtrl.OnStateExit -= HandlOnStateExit;
		}
	}
}
