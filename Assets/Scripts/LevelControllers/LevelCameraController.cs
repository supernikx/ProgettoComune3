using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

/// <summary>
/// Classe che gestisce il controllo della camera nel livello
/// </summary>
public class LevelCameraController : MonoBehaviour
{
	[Header("Camera Settings")]
	[SerializeField]
	private CinemachineVirtualCamera startVirtualCam;

	/// <summary>
	/// Riferimento al Level Manager
	/// </summary>
	private LevelManager lvlMng;
	/// <summary>
	/// Riferimento al GroupController
	/// </summary>
	private GroupController groupCtrl;
	/// <summary>
	/// Riferiemento al camera shake
	/// </summary>
	private CinemachineBasicMultiChannelPerlin cameraShake;
	/// <summary>
	/// Riferiemento alla routine del camera shake
	/// </summary>
	private IEnumerator cameraShakeRoutine;

	/// <summary>
	/// Funzione di Setup dello script
	/// </summary>
	public void Setup(LevelManager _lvlMng)
	{
		lvlMng = _lvlMng;
		groupCtrl = lvlMng.GetGroupController();
		cameraShake = startVirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		cameraShake.m_AmplitudeGain = 0;

		startVirtualCam.Follow = groupCtrl.GetGroupCenterTransform();
	}

	/// <summary>
	/// Corotuine che esegue il camera shake
	/// </summary>
	/// <returns></returns>
	private IEnumerator CameraShakeCoroutine(float _duration)
	{
		cameraShake.m_AmplitudeGain = 1;
		yield return new WaitForSeconds(_duration);
		cameraShake.m_AmplitudeGain = 0;
	}

	#region API
	/// <summary>
	/// Funzione che fa partire il camera shake
	/// </summary>
	public void DoCameraShake(float _duration)
	{
		if (cameraShakeRoutine != null)
		{
			StopCoroutine(cameraShakeRoutine);
			cameraShake.m_AmplitudeGain = 0;
		}

		cameraShakeRoutine = CameraShakeCoroutine(_duration);
		StartCoroutine(cameraShakeRoutine);

	}

	/// <summary>
	/// Funzione che ritorna la cmaera
	/// </summary>
	/// <returns></returns>
	public CinemachineVirtualCamera GetVirtualCamera()
	{
		return startVirtualCam;

	}
	#endregion
}
