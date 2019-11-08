using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce lo stato di Setup della GameSM
/// </summary>
public class GameSetupState : GameSMStateBase
{
    /// <summary>
    /// Riferimento al GameManager
    /// </summary>
    private GameManager gm;

    public override void Enter()
    {
        gm = context.GetGameManager();
        UI_Manager uiMng = FindObjectOfType<UI_Manager>();

        gm.SetUIManager(uiMng);
        uiMng.Setup(gm);

        Complete();
    }
}
