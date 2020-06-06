using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce l'inizio della quarta fase del Boss 2
/// </summary>
public class Boss2StartPhase4State : Boss2StateBase
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

    public override void Enter()
    {
        bossCtrl = context.GetBossController();
        lifeCtrl = bossCtrl.GetBossLifeController();

        lifeCtrl.SetCanTakeDamage(canTakeDirectDamage);
        bossCtrl.GetSoundController().PlayAudioClipOnTime(startPhaseSoundID);
        bossCtrl.ChangeColor(Color.white);

        Debug.Log("Phase 4 Iniziata");
        Complete();
    }
}
