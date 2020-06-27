using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Classe che gestisce la parte grafica del Boss 1
/// </summary>
public class Boss1GraphicController : MonoBehaviour
{
	[Header("Graphic Settings")]
	[SerializeField]
	private Renderer bossGraphic;
	[SerializeField]
	private Color hitFlashColor;

	/// <summary>
	/// Riferimento all'animator
	/// </summary>
	private Animator anim;
	/// <summary>
	/// Riferiemento al boss controller
	/// </summary>
	private Boss1Controller bossCtrl;
	/// <summary>
	/// Riferiemento al life controller
	/// </summary>
	private BossLifeController lifeCtrl;
	/// <summary>
	/// Riferiemento allo state machine controller del boss 1
	/// </summary>
	private Boss1SMController smCtrl;
	/// <summary>
	/// Riferimento alla coroutine dell'effetto di flash
	/// </summary>
	private IEnumerator flashEffectRoutine;

	/// <summary>
	/// Funzione di Setup
	/// </summary>
	public void Setup(Boss1Controller _bossCtrl, Boss1SMController _smCtrl)
	{
		bossCtrl = _bossCtrl;
		smCtrl = _smCtrl;
		lifeCtrl = bossCtrl.GetBossLifeController();
		anim = GetComponent<Animator>();

		smCtrl.OnStateEnter += HandleOnStateEnter;
		smCtrl.OnStateExit += HandlOnStateExit;
		lifeCtrl.OnBossTakeDamage += HandleOnBossTakeDamage;
	}

	#region Handlers
	/// <summary>
	/// Funzione chiamata quando il boss prende danno
	/// </summary>
	/// <param name="_damage"></param>
	private void HandleOnBossTakeDamage(int _damage)
	{
		Color startColor = bossGraphic.material.GetColor("Color_A9F326B");
		if (flashEffectRoutine != null)
		{
			StopCoroutine(flashEffectRoutine);
			bossGraphic.material.SetColor("Color_A9F326B", startColor);
		}

		flashEffectRoutine = FlashEffectCoroutine(startColor);
		StartCoroutine(flashEffectRoutine);
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
	#endregion

	/// <summary>
	/// Coroutine che esgue l'effetto di flash
	/// </summary>
	/// <param name="_startColor"></param>
	/// <returns></returns>
	private IEnumerator FlashEffectCoroutine(Color _startColor)
	{
		bossGraphic.material.SetColor("Color_A9F326B", hitFlashColor);
		yield return new WaitForSeconds(0.05f);
		bossGraphic.material.SetColor("Color_A9F326B", _startColor);
	}

	private void OnDisable()
	{
		if (smCtrl != null)
		{
			smCtrl.OnStateEnter -= HandleOnStateEnter;
			smCtrl.OnStateExit -= HandlOnStateExit;
		}

		if (lifeCtrl != null)
			lifeCtrl.OnBossTakeDamage -= HandleOnBossTakeDamage;
	}
}
