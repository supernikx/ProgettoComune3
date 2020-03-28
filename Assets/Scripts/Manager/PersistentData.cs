using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe che gestisce i dati persistenti fra le scene
/// </summary>
public class PersistentData : MonoBehaviour
{
    //Singleton
    private static PersistentData instance;

    //Data
    public static int spawnPointID;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
        spawnPointID = -1;
    }
}
