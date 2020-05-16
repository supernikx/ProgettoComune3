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
    //Numero di agent necessari per attivare i coverblock
    [SerializeField]
    private int coverBlockAgentAmmount;
    //Durata dei cover block
    [SerializeField]
    private float coverBlockDuration;
    //Velocità di Heat del coverblock
    [SerializeField]
    private float coverBlockHeatSpeed;
    //Velocità di reset del coverblock
    [SerializeField]
    private float coverBlockResetSpeed;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    public void Setup()
    {
        for (int i = 0; i < coverBlocks.Count; i++)
        {
            coverBlocks[i].Setup(coverBlockAgentAmmount, coverBlockDuration, coverBlockHeatSpeed, coverBlockResetSpeed);
            coverBlocks[i].Enable(true);
        }
    }

    #region API
    /// <summary>
    /// Funzione che ritorna i coverblock
    /// </summary>
    /// <returns></returns>
    public List<CoverBlockController> GetCoverBlocks()
    {
        return coverBlocks;
    }

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
