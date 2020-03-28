using System;
using UnityEngine;

/// <summary>
/// Classe che gestisce le uscite della scena
/// </summary>
public class ChangeSceneTrigger : MonoBehaviour
{
    /// <summary>
    /// Evento che notifica il trigger di questa usicta
    /// </summary>
    public static Action<string> OnExitTriggered;

    [Header("Exit Trigger Settings")]
    //Nome della scena in cui porta questa uscita
    [SerializeField]
    private string sceneName;
    //ID dello spawn point su cui si vuole andare
    [SerializeField]
    private int spawnID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Agent"))
        {
            PersistentData.spawnPointID = spawnID;
            OnExitTriggered?.Invoke(sceneName);
        }
    }
}
