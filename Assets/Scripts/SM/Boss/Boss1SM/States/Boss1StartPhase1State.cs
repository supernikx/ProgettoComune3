using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce l'inizio della prima fase del Boss
/// </summary>
public class Boss1StartPhase1State : Boss1StateBase
{
    public override void Enter()
    {
        Debug.Log("Phase 1 Iniziata");
        Complete();
    }
}
