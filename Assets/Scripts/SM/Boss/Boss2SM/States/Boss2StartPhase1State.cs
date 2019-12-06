using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce l'inizio della prima fase del Boss 2
/// </summary>
public class Boss2StartPhase1State : Boss2StateBase
{
    public override void Enter()
    {
        Debug.Log("Phase 1 Iniziata");
        Complete();
    }
}
