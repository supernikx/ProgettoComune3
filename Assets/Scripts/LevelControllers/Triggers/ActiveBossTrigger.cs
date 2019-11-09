using System;
using UnityEngine;

/// <summary>
/// Classe che gestisce l'attivazione dei Boss
/// </summary>
public class ActiveBossTrigger : MonoBehaviour
{
    /// <summary>
    /// Evento che notifica il trigger di questo boss
    /// </summary>
    public static Action<BossControllerBase> OnBossTriggered;

    [Header("Boss Trigger Settings")]
    //Boss da attivare
    [SerializeField]
    private BossControllerBase bossToEnable;
    //Bool che identifica se il trigger va disabilitato dopo che questo è avventuo
    [SerializeField]
    private bool disableAfterTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Agent"))
        {
            OnBossTriggered?.Invoke(bossToEnable);
            if (disableAfterTrigger)
                gameObject.SetActive(false);
        }
    }
}
