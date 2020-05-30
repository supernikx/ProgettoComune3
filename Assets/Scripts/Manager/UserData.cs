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

	private void Awake()
	{
		instance = this;
		LoadData();
	}

	#region API
	/// <summary>
	/// Funzione che carica i dati
	/// </summary>
	public static void LoadData()
	{
		instance.musicVolume = PlayerPrefs.GetFloat("music", 0);
		instance.effectVolume = PlayerPrefs.GetFloat("effect", 0);
		instance.graphicQuality = PlayerPrefs.GetInt("quality", 2);
	}

	/// <summary>
	/// Funzione che salva i dati
	/// </summary>
	public void SaveData()
	{
		PlayerPrefs.SetFloat("music", instance.musicVolume);
		PlayerPrefs.SetFloat("effect", instance.effectVolume);
		PlayerPrefs.SetInt("quality", instance.graphicQuality);
	}

	#region Setter
	#region Getter
	/// <summary>
	/// Funzione che imposta il volume della musica
	/// </summary>
	/// <returns></returns>
	public static void SetMusicVolume(float _volume)
	{
		instance.musicVolume = _volume;
		PlayerPrefs.SetFloat("music", instance.musicVolume);
	}

	/// <summary>
	/// Funzione che imposta il volume dell'effetto
	/// </summary>
	/// <returns></returns>
	public static void SetEffectVolume(float _volume)
	{
		instance.effectVolume = _volume;
		PlayerPrefs.SetFloat("effect", instance.effectVolume);
	}

	/// <summary>
	/// Funzione che imposta l'id dellq qualità grafica
	/// </summary>
	/// <returns></returns>
	public static void SetGraphicQuality(int _quality)
	{
		instance.graphicQuality = _quality;
		PlayerPrefs.SetInt("quality", instance.graphicQuality);
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
		return instance.musicVolume = PlayerPrefs.GetFloat("music", 0);
	}

	/// <summary>
	/// Funzione che ritorna il volume dell'effetto
	/// </summary>
	/// <returns></returns>
	public static float GetEffectVolume()
	{
		return instance.effectVolume = PlayerPrefs.GetFloat("effect", 0);
	}

	/// <summary>
	/// Funzione che ritorna l'id dellq qualità grafica
	/// </summary>
	/// <returns></returns>
	public static int GetGraphicQuality()
	{
		return instance.graphicQuality = PlayerPrefs.GetInt("quality", 2);
	}
	#endregion
	#endregion
}
