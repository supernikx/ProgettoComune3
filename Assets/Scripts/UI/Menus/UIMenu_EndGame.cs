using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Classe che gestisce il pannello di EndGame
/// </summary>
public class UIMenu_EndGame : UIMenu_Base
{
    #region Actions
    /// <summary>
    /// Evento che notifica la pressione del pulsante di Retry
    /// </summary>
    public Action RetryButtonPressed;
    /// <summary>
    /// Evento che notifica la pressione del pulsante di Quit
    /// </summary>
    public Action QuitButtonPressed;
    #endregion

    [Header("Panel Settings")]
    //Scritta di end game
    [SerializeField]
    private TextMeshProUGUI gameOverText;

    /// <summary>
    /// Riferimento alla coroutine di dissolvenza del testo di end game
    /// </summary>
    private IEnumerator gameOverTextRoutine;

    /// <summary>
    /// Override della funzione di Toggle del UIMenu_Base
    /// </summary>
    /// <param name="_value"></param>
    public override void ToggleMenu(bool _value)
    {
        base.ToggleMenu(_value);

        if (_value)
        {
            gameOverTextRoutine = GameOverTextCoroutine();
            StartCoroutine(gameOverTextRoutine);
        }
        else
        {
            if (gameOverTextRoutine != null)
                StopCoroutine(gameOverTextRoutine);
        }
    }

    /// <summary>
    /// Funzione che gestisce il pulsante di Retry del pannello
    /// </summary>
    public void RetryButton()
    {
        RetryButtonPressed?.Invoke();
    }

    /// <summary>
    /// Funzione che gestisce il pulsante di Retry del pannello
    /// </summary>
    public void QuitButton()
    {
        QuitButtonPressed?.Invoke();
    }

    /// <summary>
    /// Coroutine che fa un effetto dissolvenza sulla scritta di end game
    /// </summary>
    /// <returns></returns>
    private IEnumerator GameOverTextCoroutine()
    {
        gameOverText.alpha = 0;
        yield return new WaitForSeconds(0.2f);
        while (gameOverText.alpha < 1)
        {
            gameOverText.alpha += Time.deltaTime * 3f;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
