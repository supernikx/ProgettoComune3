using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che controlla il feedback del gruppo
/// </summary>
public class GroupFeedbackController : MonoBehaviour
{
    [Header("Feedback References")]
    //Riferimento all'aim arrow
    [SerializeField]
    private AimArrowFeedback aimArrow;
    [Header("VFX References")]
    //Riferimento al VFX di reloading
    [SerializeField]
    private ParticleSystem reloadVFX;

    /// <summary>
    /// Fiferimento al group controller
    /// </summary>
    private GroupController groupCtrl;

    /// <summary>
    /// Funzione dio Setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    public void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;

        aimArrow.Setup(groupCtrl);
    }

    #region API
    /// <summary>
    /// Funzione che imposta il reload VFX
    /// </summary>
    /// <param name="_enable"></param>
    public void SetReloadVFX(bool _enable)
    {
        if (reloadVFX == null)
            return;

        if(_enable)
        {
            reloadVFX.transform.position = groupCtrl.GetGroupCenterPoint();
            reloadVFX.Play();
        }
        else
        {
            reloadVFX.Stop();
        }
    }

    #region Getter
    /// <summary>
    /// Funzione che ritorna l'aim arrow
    /// </summary>
    /// <returns></returns>
    public AimArrowFeedback GetAimArrow()
    {
        return aimArrow;
    }
    #endregion
    #endregion
}
