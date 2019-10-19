using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che si occupa di controllare il feedback della freccia di mira
/// </summary>
public class AimArrowFeedback : MonoBehaviour
{
    /// <summary>
    /// Funzione che aggiorna la posizione e l'orientamento della freccia
    /// </summary>
    /// <param name="_groupCenterPosition"></param>
    /// <param name="_aimDirection"></param>
    public void UpdateArrow(Vector3 _groupCenterPosition, Vector3 _aimDirection)
    {
        //Prima lo sposto
        transform.position = _groupCenterPosition;
        Vector3 directionToLook = _aimDirection;
        directionToLook.y = 0f;
        if (directionToLook != Vector3.zero)
            transform.rotation = Quaternion.AngleAxis(-90f, Vector3.up) * Quaternion.LookRotation(directionToLook);
    }
}
