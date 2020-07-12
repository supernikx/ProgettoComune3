using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce la fine della demo
/// </summary>
public class EndDemoTrigger : MonoBehaviour
{
	private UI_Manager uiMng;
	private UIMenu_Gameplay gameplayPanel;

	private void Start()
	{
		uiMng = GameManager.instance.GetUIManager();
		gameplayPanel = FindObjectOfType<UIMenu_Gameplay>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (UserData.GetBossDefeated() == 3)
			return;

		if (other.gameObject.layer == LayerMask.NameToLayer("Agent"))
		{
			StartCoroutine(EndDemoPanelCoroutine());			
			UserData.SetBossDefeated(3);
		}
	}

	/// <summary>
	/// Coroutine che fa apparire e sparire il pannello di fine demo
	/// </summary>
	/// <returns></returns>
	private IEnumerator EndDemoPanelCoroutine()
	{
		gameplayPanel.ToggleEndDemoPanel(true);
		yield return new WaitForSeconds(3f);
		gameplayPanel.ToggleEndDemoPanel(false);
	}
}
