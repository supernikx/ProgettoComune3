using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Classe che gestisce il menù dei settings
/// </summary>
public class UISubmenu_Options : MonoBehaviour
{
	[Header("Settings References")]
	[SerializeField]
	private GameObject panel;
	[SerializeField]
	private GameObject defaultSelectedButton;
	[SerializeField]
	private Slider musicSlider;
	[SerializeField]
	private Slider effectSlider;
	[SerializeField]
	private TMP_Dropdown qualityDropDown;

	[Header("Mixer References")]
	[SerializeField]
	private AudioMixer audioMixer;

	private GameObject oldButtonSelected;
	private bool isEnable;
	private int currentQuality;
	private float currentMusicVolume;
	private float currentEffectVolume;

	/// <summary>
	/// Funzione di Setup
	/// </summary>
	public void Setup()
	{
		ResetButton();
		panel.SetActive(false);
	}

	/// <summary>
	/// Funzione che abilita/disabilita il pannello
	/// </summary>
	/// <param name="_enable"></param>
	public void Enable(bool _enable)
	{
		isEnable = _enable;
		if (isEnable)
		{
			GetData();
			UpdateMenuGraphic();
			oldButtonSelected = EventSystem.current.currentSelectedGameObject;
			EventSystem.current.SetSelectedGameObject(defaultSelectedButton);
		}
		else 
		{
			EventSystem.current.SetSelectedGameObject(oldButtonSelected);
			ResetButton();
		}

		panel.SetActive(isEnable);
	}

	/// <summary>
	/// Funzione che prende i dati
	/// </summary>
	private void GetData()
	{
		currentMusicVolume = UserData.GetMusicVolume();
		currentEffectVolume = UserData.GetEffectVolume();
		currentQuality = UserData.GetGraphicQuality();
	}

	/// <summary>
	/// Funzione che imposta i dati
	/// </summary>
	private void SetData()
	{
		audioMixer.SetFloat("EffectsVolume", currentEffectVolume);
		audioMixer.SetFloat("MusicVolume", currentMusicVolume);
		QualitySettings.SetQualityLevel(currentQuality);
	}

	/// <summary>
	/// Funzione che aggiorna la grafica del pannello
	/// </summary>
	private void UpdateMenuGraphic()
	{
		musicSlider.value = currentMusicVolume;
		effectSlider.value = currentEffectVolume;
		qualityDropDown.value = currentQuality;
	}

	#region UI
	/// <summary>
	/// Funzione che reimposta i dati
	/// </summary>
	public void BackButton()
	{
		Enable(false);
	}

	/// <summary>
	/// Funzione chiamata al cambio del valore del music slider
	/// </summary>
	/// <param name="_value"></param>
	public void MusicSliderChange(float _value)
	{
		currentMusicVolume = _value;
		SetData();
	}

	/// <summary>
	/// Funzione chiamata al cambio del valore del effect slider
	/// </summary>
	/// <param name="_value"></param>
	public void EffectSliderChange(float _value)
	{
		currentEffectVolume = _value;
		SetData();
	}

	/// <summary>
	/// Funzione chiamata al cambio del valore del quality
	/// </summary>
	/// <param name="_value"></param>
	public void QualitySettingsChange(int _value)
	{
		currentQuality = _value;
		SetData();
	}

	/// <summary>
	/// Funzione che salva i dati
	/// </summary>
	public void SaveButton()
	{
		UserData.SetMusicVolume(currentMusicVolume);
		UserData.SetEffectVolume(currentEffectVolume);
		UserData.SetGraphicQuality(currentQuality);
	}

	/// <summary>
	/// Funzione che reimposta i dati
	/// </summary>
	public void ResetButton()
	{
		GetData();
		SetData();
		UpdateMenuGraphic();
	}
	#endregion
}
