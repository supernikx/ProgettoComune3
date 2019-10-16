using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che funziona da manager per il gioco
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Riferimento al group controller
    /// </summary>
    private GroupController groupCtrl;


    void Start()
    {
        groupCtrl = FindObjectOfType<GroupController>();

        groupCtrl.Setup();
    }
}
