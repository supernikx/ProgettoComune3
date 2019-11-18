﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce l'inizio della terza fase del Boss
/// </summary>
public class Boss1StartPhase3State : Boss1StateBase
{
    public override void Enter()
    {
        Debug.Log("Phase 3 Iniziata");
        Complete();
    }
}