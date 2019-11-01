using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che funziona da manager per il gioco
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField]
    GameObject gameOverText;

    /// <summary>
    /// Riferimento al group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al BossPrototipo
    /// </summary>
    private BossPrototipoController bossPrototipo;

    void Start()
    {
        groupCtrl = FindObjectOfType<GroupController>();
        bossPrototipo = FindObjectOfType<BossPrototipoController>();

        groupCtrl.Setup();

        groupCtrl.OnGroupDead += HandleOnGroupDead;
    }

    //Debug
    bool notagain = false;
    private void Update()
    {
        if (!notagain && bossPrototipo != null && Input.GetKeyDown(KeyCode.K))
        {
            notagain = true;
            bossPrototipo.Setup(this);
        }
    }

    #region Handlers
    /// <summary>
    /// Funzione che gestisce l'evento di morte del gruppo
    /// </summary>
    private void HandleOnGroupDead()
    {
        gameOverText.SetActive(true);
        bossPrototipo.StopBoss();
        Destroy(groupCtrl.gameObject);
    }
    #endregion

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
    #endregion
    #endregion

    private void OnDisable()
    {
        groupCtrl.OnGroupDead -= HandleOnGroupDead;
    }
}
