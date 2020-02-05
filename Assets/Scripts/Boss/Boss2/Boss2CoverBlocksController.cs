using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce i cover block del Boss2
/// </summary>
public class Boss2CoverBlocksController : MonoBehaviour
{
    [Header("Cover Block Settings")]
    //Lista di coverblock
    [SerializeField]
    private List<CoverBlockController> coverBlocks;
    //Hit necessari per attivare un cover block
    [SerializeField]
    private int coverBlockHit;
    //Durata dei cover block
    [SerializeField]
    private float coverBlockDuration;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    public void Setup()
    {
        for (int i = 0; i < coverBlocks.Count; i++)
        {
            coverBlocks[i].Setup(coverBlockHit, coverBlockDuration);
            coverBlocks[i].Enable(false);
        }
    }

    #region API
    /// <summary>
    /// Funzione che attiva/disattiva i coverblock
    /// </summary>
    /// <param name="_enable"></param>
    public void EnableCoverBlocks(bool _enable)
    {
        for (int i = 0; i < coverBlocks.Count; i++)
        {
            coverBlocks[i].Enable(_enable);
        }
    }
    #endregion
}
