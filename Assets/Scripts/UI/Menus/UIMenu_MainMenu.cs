using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Classe che gestisce il Pannello di MainMenu
/// </summary>
public class UIMenu_MainMenu : UIMenu_Base
{
	#region Actions
	/// <summary>
	/// Evento che notifica la pressione del bottone Start
	/// </summary>
	public Action StartButtonPressed;
	#endregion

	[Header("Panel References")]
	[SerializeField]
	private Button startButton;
	[SerializeField]
	private Button continueButton;
	[SerializeField]
	private Button optionsButtton;
	[SerializeField]
	private Button quitButton;
	[SerializeField]
	private UISubmenu_Options optionsPanel;

	public override void CustomSetup(UIManagerBase _controller)
	{
		base.CustomSetup(_controller);
		optionsPanel.Setup();



		if (UserData.GetBossDefeated() == 0)
		{
			continueButton.interactable = false;
			Navigation newStartNav = new Navigation();
			newStartNav.mode = Navigation.Mode.Explicit;
			newStartNav.selectOnDown = optionsButtton;
			startButton.navigation = newStartNav;

			Navigation newOptionsNav = new Navigation();
			newOptionsNav.mode = Navigation.Mode.Explicit;
			newOptionsNav.selectOnUp = startButton;
			newOptionsNav.selectOnDown = quitButton;
			optionsButtton.navigation = newOptionsNav;
		}
		else
		{
			continueButton.interactable = true;
			Navigation newStartNav = new Navigation();
			newStartNav.mode = Navigation.Mode.Explicit;
			newStartNav.selectOnDown = continueButton;
			startButton.navigation = newStartNav;

			Navigation newContinueNav = new Navigation();
			newContinueNav.mode = Navigation.Mode.Explicit;
			newContinueNav.selectOnUp = startButton;
			newContinueNav.selectOnDown = optionsButtton;
			continueButton.navigation = newContinueNav;

			Navigation newOptionsNav = new Navigation();
			newOptionsNav.mode = Navigation.Mode.Explicit;
			newOptionsNav.selectOnUp = continueButton;
			newOptionsNav.selectOnDown = quitButton;
			optionsButtton.navigation = newOptionsNav;
		}
	}

	/// <summary>
	/// Funzione che gestisce il bottone di Start del pannello
	/// </summary>
	public void StartButton()
	{
		UserData.SetBossDefeated(0);
		StartButtonPressed?.Invoke();
	}

	/// <summary>
	/// Funzione che gestisce il bottone di Start del pannello
	/// </summary>
	public void ContinueButton()
	{
		StartButtonPressed?.Invoke();
	}

	/// <summary>
	/// Funzione che gestisce il bottone di options del pannello
	/// </summary>
	public void OptionnsButton()
	{
		optionsPanel.Enable(true);
	}

	/// <summary>
	/// Funzione che gestisce il bottone di Quit del pannello
	/// </summary>
	public void QuitButton()
	{
		Application.Quit();
	}
}
