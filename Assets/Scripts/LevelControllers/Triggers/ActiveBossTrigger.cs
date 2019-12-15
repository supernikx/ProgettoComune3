using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

/// <summary>
/// Classe che gestisce l'attivazione dei Boss
/// </summary>
public class ActiveBossTrigger : MonoBehaviour
{
    #region Actions
    /// <summary>
    /// Evento che notifica il trigger di questo boss
    /// </summary>
    public static Action<BossControllerBase> OnBossTriggered;
    #endregion

    [Header("Boss Trigger Settings")]
    //Boss da attivare
    [SerializeField]
    private BossControllerBase bossToEnable;
    //Bool che identifica se il trigger va disabilitato dopo che questo è avventuo
    [SerializeField]
    private bool disableAfterTrigger;
    //Director della cutscene di attivazione del Boss
    [SerializeField]
    private PlayableDirector cutsceneDirector;

    [Header("Binding Settings")]
    //Mappatura dei tasti per saltare la cutscene
    [SerializeField]
    InputAction inputSkipCutscene;

    /// <summary>
    /// Riferimento al level manager
    /// </summary>
    private LevelManager lvlMng;
    /// <summary>
    /// Riferimento al group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al collider
    /// </summary>
    private Collider triggerCollider;

    private void OnEnable()
    {
        inputSkipCutscene.Enable();
    }

    /// <summary>
    /// Funzione di Setup
    /// </summary>
    public void Setup(LevelManager _lvlMng)
    {
        lvlMng = _lvlMng;
        groupCtrl = lvlMng.GetGroupController();
        triggerCollider = GetComponent<Collider>();

        triggerCollider.enabled = true;
        bossToEnable.Setup(lvlMng);        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Agent"))
        {
            if (disableAfterTrigger)
                triggerCollider.enabled = false;

            if (cutsceneDirector != null)
                StartCutscene();
            else
                OnBossTriggered?.Invoke(bossToEnable);
        }
    }

    /// <summary>
    /// Funzione che fa partire la cutscene di inizio della bossfight
    /// </summary>
    private void StartCutscene()
    {
        inputSkipCutscene.performed += SkipCutscene;
        groupCtrl.Enable(false);
        cutsceneDirector.Play();
    }

    /// <summary>
    /// Funzione che esgue lo skip della cutscene
    /// </summary>
    /// <param name="_context"></param>
    private void SkipCutscene(InputAction.CallbackContext _context)
    {
        inputSkipCutscene.performed -= SkipCutscene;
        cutsceneDirector.time = cutsceneDirector.duration;
        //groupCtrl.Enable(true);
        //OnBossTriggered?.Invoke(bossToEnable);
    }

    /// <summary>
    /// Callback di fine cutscene
    /// </summary>
    public void EndCusceneCallback()
    {
        inputSkipCutscene.performed -= SkipCutscene;
        groupCtrl.Enable(true);
        OnBossTriggered?.Invoke(bossToEnable);
    }

    private void OnDisable()
    {
        inputSkipCutscene.performed -= SkipCutscene;
        inputSkipCutscene.Disable();
    }
}
