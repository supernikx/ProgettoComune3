using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe che gestisce i dati persistenti fra le scene
/// </summary>
public static class PersistentData
{
    //Data
    public static int spawnPointID;

    static PersistentData()
    {
        spawnPointID = -1;
    }
}
