using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce l'inizio dell seconda fase del Boss
/// </summary>
public class Boss1StartPhase2State : Boss1StateBase
{
    public override void Enter()
    {
        Debug.Log("Phase 2 Iniziata");
        Complete();
    }
}
