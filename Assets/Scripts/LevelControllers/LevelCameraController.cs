using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
    /// Funzione di Setup dello script
    /// </summary>
    public void Setup(LevelManager _lvlMng)
    {
        lvlMng = _lvlMng;
        groupCtrl = lvlMng.GetGroupController();

        startVirtualCam.Follow = groupCtrl.GetGroupCenterTransform();
    }
}
