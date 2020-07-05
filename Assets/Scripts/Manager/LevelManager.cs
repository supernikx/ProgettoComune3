using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce la sessione di Gioco
/// </summary>
public class LevelManager : MonoBehaviour
{
	[Header("Level Settings")]
	//Punto iniziale del gruppo
	[SerializeField]
	private List<GroupStartPosition> groupStartPositions;
	//Blocco del boss 2
	[SerializeField]
	private GameObject boss2Block;
	//Blocco del boss 3
	[SerializeField]
	private GameObject boss3Block;

	/// <summary>
	/// Classe che associa ad uno spawn point un ID
	/// </summary>
	[System.Serializable]
	private class GroupStartPosition
	{
		public Transform groupStartPosition;
		public int positionID;
	}

	/// <summary>
	/// Riferimento al level scene controller
	/// </summary>
	private LevelSceneController lvlSceneCtrl;
	/// <summary>
	/// Riferimento al level camera controller
	/// </summary>
	private LevelCameraController lvlCamCtrl;
	/// <summary>
	/// Riferimento al level pause controller
	/// </summary>
	private LevelPauseController lvlPauseCtrl;
	/// <summary>
	/// Riferimento al level boss controller
	/// </summary>
	private LevelBossController lvlBossCtrl;
	/// <summary>
	/// Riferimento al level tutorial controller
	/// </summary>
	private LevelTutorialController lvlTutorialCtrl;
	/// <summary>
	/// Riferimento al GroupController
	/// </summary>
	private GroupController groupCtrl;

	/// <summary>
	/// Funzione di Setup dello script
	/// </summary>
	public void Setup()
	{
		lvlSceneCtrl = GetComponent<LevelSceneController>();
		lvlCamCtrl = GetComponent<LevelCameraController>();
		lvlPauseCtrl = GetComponent<LevelPauseController>();
		lvlBossCtrl = GetComponent<LevelBossController>();
		lvlTutorialCtrl = GetComponent<LevelTutorialController>();
		groupCtrl = FindObjectOfType<GroupController>();

		lvlSceneCtrl.Setup();
		lvlCamCtrl.Setup(this);
		lvlPauseCtrl.Setup();

		if (lvlBossCtrl != null)
			lvlBossCtrl.Setup(this);

		if (lvlTutorialCtrl != null)
			lvlTutorialCtrl.Setup(this);

		int bossDefeated = UserData.GetBossDefeated();
		if (boss2Block != null)
		{
			if (bossDefeated == 0)
				boss2Block.SetActive(true);
			else
				boss2Block.SetActive(false);
		}

		if (boss3Block != null)
		{
			if (bossDefeated < 2)
				boss3Block.SetActive(true);
			else
				boss3Block.SetActive(false);
		}

		Vector3 startPositon = transform.position;
		for (int i = 0; i < groupStartPositions.Count; i++)
		{
			if (groupStartPositions[i].positionID == PersistentData.spawnPointID)
			{
				startPositon = groupStartPositions[i].groupStartPosition.position;
				break;
			}
		}

		groupCtrl.FillGroup();
		groupCtrl.Move(startPositon);
		groupCtrl.Enable(true);
	}

	#region API
	#region Getter
	/// <summary>
	/// Funzione che ritorna il GroupController
	/// </summary>
	/// <returns></returns>
	public GroupController GetGroupController()
	{
		return groupCtrl;
	}

	/// <summary>
	/// Funzione che ritorna il level scene controller
	/// </summary>
	/// <returns></returns>
	public LevelSceneController GetLevelSceneController()
	{
		return lvlSceneCtrl;
	}

	/// <summary>
	/// Funzione che ritorna il level pause controller
	/// </summary>
	/// <returns></returns>
	public LevelPauseController GetLevelPauseController()
	{
		return lvlPauseCtrl;
	}

	/// <summary>
	/// Funzione che ritorna il level boss controller
	/// </summary>
	/// <returns></returns>
	public LevelBossController GetLevelBossController()
	{
		return lvlBossCtrl;
	}

	/// <summary>
	/// Funzione che ritorna il camera controller del livello
	/// </summary>
	/// <returns></returns>
	public LevelCameraController GetLevelCameraController()
	{
		return lvlCamCtrl;
	}
	#endregion
	#endregion
}
