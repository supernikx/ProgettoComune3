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
    private Transform groupStartPosition;

    /// <summary>
    /// Riferimento al level scene controller
    /// </summary>
    private LevelSceneController lvlSceneCtrl;
    /// <summary>
    /// Riferimento al level camera controller
    /// </summary>
    private LevelCameraController lvlCamCtrl;
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
        groupCtrl = FindObjectOfType<GroupController>();

        lvlSceneCtrl.Setup();
        lvlCamCtrl.Setup(this);

        groupCtrl.Enable(true);
        groupCtrl.Move(groupStartPosition.position);
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
    #endregion
    #endregion
}
