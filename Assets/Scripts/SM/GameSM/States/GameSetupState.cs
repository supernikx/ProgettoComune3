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
        SceneReferenceManager sceneRef = FindObjectOfType<SceneReferenceManager>();

        gm.SetUIManager(uiMng);
        gm.SetSceneReferenceManager(sceneRef);

        uiMng.Setup(gm);

        Complete();
    }
}
