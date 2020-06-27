using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce l'inizio della prima fase del Boss 2
/// </summary>
public class Boss2StartPhase1State : Boss2StateBase
{
    [Header("Feedback")]
    //suono di attivazione del boss
    [SerializeField]
    protected string startPhaseSoundID;

    /// <summary>
    /// Riferiemtno al Boss Controller
    /// </summary>
    private Boss2Controller bossCtrl;
    /// <summary>
    /// Riferimento al Life Controller
    /// </summary>
    private BossLifeController lifeCtrl;
    /// <summary>
    /// Riferiemento all'event camera controller
    /// </summary>
    private LevelCameraController cameraCtrl;

    public override void Enter()
    {
        cameraCtrl = context.GetLevelManager().GetLevelCameraController();
        bossCtrl = context.GetBossController();
        lifeCtrl = bossCtrl.GetBossLifeController();

        lifeCtrl.SetCanTakeDamage(canTakeDirectDamage);
        bossCtrl.GetSoundController().PlayAudioClipOnTime(startPhaseSoundID);

        cameraCtrl.DoCameraShake(0.5f);

        Debug.Log("Phase 1 Iniziata");
        Complete();
    }
}
