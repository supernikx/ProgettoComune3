using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che controlla il feedback del gruppo
/// </summary>
public class GroupFeedbackController : MonoBehaviour
{
    /// <summary>
    /// Fiferimento al group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento all'aim arrow
    /// </summary>
    private AimArrowFeedback aimArrow;

    /// <summary>
    /// Funzione dio Setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    public void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;
        aimArrow = FindObjectOfType<AimArrowFeedback>();

        aimArrow.Setup(groupCtrl);
    }

    #region API
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
