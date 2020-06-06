using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce l'inizio della prima fase del Boss 1
/// </summary>
public class Boss1StartPhase1State : Boss1StateBase
{
    [Header("Phase Settings")]
    //Se il boss può prendere danno diretto
    [SerializeField]
    private bool canTakeDirectDamage;

    [Header("Feedback")]
    //suono di attivazione del boss
    [SerializeField]
    private string startPhaseSoundID = "phase1";

    /// <summary>
    /// Riferiemtno al Boss Controller
    /// </summary>
    private Boss1Controller bossCltr;
    /// <summary>
    /// Riferimento al Life Controller
    /// </summary>
    private BossLifeController lifeCtrl;

    public override void Enter()
    {
        bossCltr = context.GetBossController();
        lifeCtrl = bossCltr.GetBossLifeController();

        lifeCtrl.SetCanTakeDamage(canTakeDirectDamage);
        bossCltr.GetSoundController().PlayAudioClipOnTime(startPhaseSoundID);

        Debug.Log("Phase 1 Iniziata");
        Complete();
    }
}
