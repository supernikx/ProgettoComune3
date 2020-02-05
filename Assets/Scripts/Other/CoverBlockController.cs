using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il cover block
/// </summary>
public class CoverBlockController : MonoBehaviour
{
    [Header("CoverBlock Reference")]
    //Riferimento al collider che attiva la barriera
    [SerializeField]
    private GameObject coverBlockButton;
    //Riferimento alla barrier che attiva il coverblock
    [SerializeField]
    private GameObject coverBlockBarrier;

    /// <summary>
    /// Colpi necessari ad attivare il cover block
    /// </summary>
    private int coverBlockNeedHits;
    /// <summary>
    /// Durata del cover block una volta attivato
    /// </summary>
    private float coverBlockDuration;
    /// <summary>
    /// Identifica se il cover block è attivo
    /// </summary>
    private bool enable;
    /// <summary>
    /// Colpi ricevuti dal cover block
    /// </summary>
    private int coverBlockDoneHits;
    /// <summary>
    /// Riferimento alla coroutine del cover block
    /// </summary>
    private IEnumerator coverBlockRoutine;

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    /// <param name="_coverBlockHit"></param>
    /// <param name="_coverBlockDuration"></param>
    public void Setup(int _coverBlockHit, float _coverBlockDuration)
    {
        coverBlockNeedHits = _coverBlockHit;
        coverBlockDuration = _coverBlockDuration;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enable)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            coverBlockDoneHits++;
            if (coverBlockDoneHits == coverBlockNeedHits)
            {
                coverBlockRoutine = CoverBlockCoroutine();
                StartCoroutine(coverBlockRoutine);
            }
        }
    }

    /// <summary>
    /// Coroutine che gestisce il coverblock attivato
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoverBlockCoroutine()
    {
        coverBlockButton.SetActive(false);
        coverBlockBarrier.SetActive(true);

        yield return new WaitForSeconds(coverBlockDuration);

        coverBlockBarrier.SetActive(false);
        coverBlockButton.SetActive(true);
        coverBlockDoneHits = 0;
    }

    #region API
    /// <summary>
    /// Funzione che attiva/disattiva il cover block
    /// </summary>
    /// <param name="_enable"></param>
    public void Enable(bool _enable)
    {
        enable = _enable;
        if (enable)
        {
            coverBlockDoneHits = 0;
            coverBlockButton.SetActive(true);
            coverBlockBarrier.SetActive(false);
        }
        else
        {
            if (coverBlockRoutine != null)
                StopCoroutine(coverBlockRoutine);

            coverBlockButton.SetActive(false);
            coverBlockBarrier.SetActive(false);
        }
    }
    #endregion
}
