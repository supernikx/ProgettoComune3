using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce i dati dell'utente
/// </summary>
public class UserData : MonoBehaviour
{
	private static UserData instance;

	private float musicVolume;
	private float effectVolume;
	private int graphicQuality;
	private int bossDefeated;

	private void Awake()
	{
		instance = this;
		LoadData();
	}

	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.B))
			PlayerPrefs.DeleteKey("boss");
#endif
	}

	#region API
	/// <summary>
	/// Funzione che carica i dati
	/// </summary>
	public static void LoadData()
	{
		if (instance != null)
		{
			instance.musicVolume = PlayerPrefs.GetFloat("music", 0);
			instance.effectVolume = PlayerPrefs.GetFloat("effect", 0);
			instance.graphicQuality = PlayerPrefs.GetInt("quality", 2);
			instance.bossDefeated = PlayerPrefs.GetInt("boss", 0);
		}
	}

	/// <summary>
	/// Funzione che salva i dati
	/// </summary>
	public void SaveData()
	{
		if (instance != null)
		{
			PlayerPrefs.SetFloat("music", instance.musicVolume);
			PlayerPrefs.SetFloat("effect", instance.effectVolume);
			PlayerPrefs.SetInt("quality", instance.graphicQuality);
			PlayerPrefs.SetInt("boss", instance.bossDefeated);
		}
	}

	#region Setter
	#region Getter
	/// <summary>
	/// Funzione che imposta il volume della musica
	/// </summary>
	/// <returns></returns>
	public static void SetMusicVolume(float _volume)
	{
		if (instance != null)
		{
			instance.musicVolume = _volume;
			PlayerPrefs.SetFloat("music", instance.musicVolume);
		}
	}

	/// <summary>
	/// Funzione che imposta il volume dell'effetto
	/// </summary>
	/// <returns></returns>
	public static void SetEffectVolume(float _volume)
	{
		if (instance != null)
		{
			instance.effectVolume = _volume;
			PlayerPrefs.SetFloat("effect", instance.effectVolume);
		}
	}

	/// <summary>
	/// Funzione che imposta l'id dellq qualità grafica
	/// </summary>
	/// <returns></returns>
	public static void SetGraphicQuality(int _quality)
	{
		if (instance != null)
		{
			instance.graphicQuality = _quality;
			PlayerPrefs.SetInt("quality", instance.graphicQuality);
		}
	}

	/// <summary>
	/// Funzione che imposta il numero di boss sconfitti
	/// </summary>
	/// <returns></returns>
	public static void SetBossDefeated(int _boss)
	{
		if (instance != null)
		{
			instance.bossDefeated = _boss;
			PlayerPrefs.SetInt("boss", instance.bossDefeated);
		}
	}
	#endregion
	#endregion

	#region Getter
	/// <summary>
	/// Funzione che ritorna il volume della musica
	/// </summary>
	/// <returns></returns>
	public static float GetMusicVolume()
	{
		if (instance != null)
			return instance.musicVolume = PlayerPrefs.GetFloat("music", 0);
		else
			return PlayerPrefs.GetFloat("music", 0);
	}

	/// <summary>
	/// Funzione che ritorna il volume dell'effetto
	/// </summary>
	/// <returns></returns>
	public static float GetEffectVolume()
	{
		if (instance != null)
			return instance.effectVolume = PlayerPrefs.GetFloat("effect", 0);
		else
			return PlayerPrefs.GetFloat("effect", 0);
	}

	/// <summary>
	/// Funzione che ritorna l'id dellq qualità grafica
	/// </summary>
	/// <returns></returns>
	public static int GetGraphicQuality()
	{
		if (instance != null)
			return instance.graphicQuality = PlayerPrefs.GetInt("quality", 2);
		else
			return PlayerPrefs.GetInt("quality", 2);
	}

	/// <summary>
	/// Funzione che ritorna l'id dellq qualità grafica
	/// </summary>
	/// <returns></returns>
	public static int GetBossDefeated()
	{
		if (instance != null)
			return instance.bossDefeated = PlayerPrefs.GetInt("boss", 0);
		else
			return PlayerPrefs.GetInt("boss", 0); ;
	}
	#endregion
	#endregion
}
