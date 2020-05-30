using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe che gestisce il menù dei settings
/// </summary>
public class UISubmenu_Options : MonoBehaviour
{
	public void Setup()
	{

	}

	#region UI
	/// <summary>
	/// Funzione chiamata al cambio del valore del music slider
	/// </summary>
	/// <param name="_value"></param>
	public void MusicSliderChange(float _value)
	{

	}

	/// <summary>
	/// Funzione chiamata al cambio del valore del effect slider
	/// </summary>
	/// <param name="_value"></param>
	public void EffectSliderChange(float _value)
	{

	}

	/// <summary>
	/// Funzione chiamata al cambio del valore del quality
	/// </summary>
	/// <param name="_value"></param>
	public void QualitySettingsChange(int _value)
	{
		QualitySettings.SetQualityLevel(_value);
	}
	#endregion
}
