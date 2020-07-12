using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce la parte grafica del Boss 2
/// </summary>
public class Boss2GraphicController : MonoBehaviour
{
	[Header("Graphic Settings")]
	[SerializeField]
	private Renderer bossGraphic;
	[SerializeField]
	private Color hitFlashColor;

	[Header("VFX References")]
	[SerializeField]
	private ParticleSystem superattackChargeVFX;
	[SerializeField]
	private ParticleSystem superattackVFX;

	/// <summary>
	/// Riferiemento al boss controller
	/// </summary>
	private Boss2Controller bossCtrl;
	/// <summary>
	/// Riferiemento al life controller
	/// </summary>
	private BossLifeController lifeCtrl;
	/// <summary>
	/// Riferimento alla coroutine dell'effetto di flash
	/// </summary>
	private IEnumerator flashEffectRoutine;
	/// <summary>
	/// Colore di default
	/// </summary>
	private Color defaultColor;

	/// <summary>
	/// Funzione di Setup
	/// </summary>
	public void Setup(Boss2Controller _bossCtrl)
	{
		bossCtrl = _bossCtrl;
		lifeCtrl = bossCtrl.GetBossLifeController();
		defaultColor = bossGraphic.material.GetColor("Color_A9F326B");

		lifeCtrl.OnBossTakeDamage += HandleOnBossTakeDamage;
	}

	#region Handlers
	/// <summary>
	/// Funzione chiamata quando il boss prende danno
	/// </summary>
	/// <param name="_damage"></param>
	private void HandleOnBossTakeDamage(int _damage)
	{
		if (!lifeCtrl.GetCanTakeDamage())
			return;

		if (flashEffectRoutine != null)
		{
			StopCoroutine(flashEffectRoutine);
			bossGraphic.material.SetColor("Color_A9F326B", defaultColor);
		}

		flashEffectRoutine = FlashEffectCoroutine();
		StartCoroutine(flashEffectRoutine);
	}
	#endregion

	/// <summary>
	/// Coroutine che esgue l'effetto di flash
	/// </summary>
	/// <param name="_startColor"></param>
	/// <returns></returns>
	private IEnumerator FlashEffectCoroutine()
	{
		bossGraphic.material.SetColor("Color_A9F326B", hitFlashColor);
		yield return new WaitForSeconds(0.05f);
		bossGraphic.material.SetColor("Color_A9F326B", defaultColor);
	}

	#region API
	#region VFX
	/// <summary>
	/// Funzione che gestisce il vfx del superattack
	/// </summary>
	/// <param name="_play"></param>
	public void SuperAttackVFX(bool _play)
	{
		if (_play)
			superattackVFX.Play();
		else
			superattackVFX.Stop();
	}

	/// <summary>
	/// Funzione che gestisce il vfx di carica del superattack
	/// </summary>
	/// <param name="_play"></param>
	public void SuperAttackChargeVFX(bool _play)
	{
		if (_play)
			superattackChargeVFX.Play();
		else
			superattackChargeVFX.Stop();
	}
	#endregion

	#region Color
	/// <summary>
	/// Funzione di Debug che cambia il colore del boss
	/// </summary>
	/// <param name="_color"></param>
	public void ChangeColor(Color _color)
	{
		if (flashEffectRoutine != null)
			StopCoroutine(flashEffectRoutine);

		bossGraphic.material.SetColor("Color_A9F326B", _color);
	}

	/// <summary>
	/// Funzione che resetta il colore a quello iniziale
	/// </summary>
	public void ResetColor()
	{
		if (flashEffectRoutine != null)
			StopCoroutine(flashEffectRoutine);

		bossGraphic.material.SetColor("Color_A9F326B", defaultColor);
	}
	#endregion
	#endregion

	private void OnDisable()
	{
		if (lifeCtrl != null)
			lifeCtrl.OnBossTakeDamage -= HandleOnBossTakeDamage;
	}
}
