using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Classe che gestisce i suoni
/// </summary>
public class SoundController : MonoBehaviour
{
	/// <summary>
	/// Classe che associa ad ogni clip un id
	/// </summary>
	[System.Serializable]
	private class Clip
	{
		public string id;
		public AudioClip audioClip;
	}

	[SerializeField]
	private List<AudioSource> audioSources;
	[Header("Clip")]
	[SerializeField]
	private List<Clip> audioClips;

	/// <summary>
	/// Funzione che ritorna un audio source libera
	/// </summary>
	/// <returns></returns>
	private AudioSource GetFreeAudioSource()
	{
		for (int i = 0; i < audioSources.Count; i++)
		{
			if (!audioSources[i].isPlaying)
				return audioSources[i];
		}

		Debug.LogWarning("Non ci sono audio source disponibili");
		return null;
	}

	/// <summary>
	/// Funzione che ritorna l'audio clip tramite ID
	/// </summary>
	/// <param name="_id"></param>
	/// <returns></returns>
	private AudioClip GetAudioClipByID(string _id)
	{
		for (int i = 0; i < audioClips.Count; i++)
		{
			if (audioClips[i].id == _id)
				return audioClips[i].audioClip;
		}

		return null;
	}

	#region API
	/// <summary>
	/// Funzione che riproduce una clip una volta
	/// </summary>
	/// <param name="_id"></param>
	public void PlayAudioClipOnTime(string _id)
	{
		AudioSource audioSource = GetFreeAudioSource();
		if (audioSource == null)
			return;

		AudioClip clip = GetAudioClipByID(_id);
		if (clip == null)
			return;

		audioSource.PlayOneShot(clip);
	}

	/// <summary>
	/// Funzione che riproduce una clip in loop
	/// </summary>
	/// <param name="_id"></param>
	public void PlayClipLoop(string _id)
	{
		AudioSource audioSource = GetFreeAudioSource();
		if (audioSource == null)
			return;

		AudioClip clip = GetAudioClipByID(_id);
		if (clip == null)
			return;

		audioSource.loop = true;
		audioSource.clip = clip;
		audioSource.Play();
	}

	/// <summary>
	/// Funzione che stoppa una clip in loop
	/// </summary>
	/// <param name="_id"></param>
	public void StopClipLoop(string _id)
	{
		AudioClip clip = GetAudioClipByID(_id);
		if (clip == null)
			return;

		for (int i = 0; i < audioSources.Count; i++)
		{
			if (audioSources[i].clip == clip)
			{
				audioSources[i].loop = false;
				audioSources[i].clip = null;
				audioSources[i].Stop();
				break;
			}
		}
	}
	#endregion
}
