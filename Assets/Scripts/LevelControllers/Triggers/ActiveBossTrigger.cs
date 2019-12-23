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
    private PlayableDirector startBossCutsceneDirector;
    //Director della cutscene di fine del Boss
    [SerializeField]
    private PlayableDirector endBossCutsceneDirector;

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

        LevelBossController.OnBossFightEnd += HandleOnBossFightEnd;

        triggerCollider.enabled = true;
        bossToEnable.Setup(lvlMng);
    }

    #region Handlers
    /// <summary>
    /// Funzione che gestisce l'evento di fine boss fight
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void HandleOnBossFightEnd(BossControllerBase _bossCtrl, bool _win)
    {
        if (_win)
        {
            if (endBossCutsceneDirector != null)
                EndBossCutscene();
        }
    }
    #endregion

    #region StartCutscene
    /// <summary>
    /// Funzione che fa partire la cutscene di inizio della bossfight
    /// </summary>
    private void StartBossCutscene()
    {
        inputSkipCutscene.performed += SkipStartBossCutscene;
        groupCtrl.Enable(false);
        startBossCutsceneDirector.Play();
    }

    /// <summary>
    /// Funzione che esgue lo skip della cutscene
    /// </summary>
    /// <param name="_context"></param>
    private void SkipStartBossCutscene(InputAction.CallbackContext _context)
    {
        inputSkipCutscene.performed -= SkipStartBossCutscene;
        startBossCutsceneDirector.time = startBossCutsceneDirector.duration;
    }

    /// <summary>
    /// Callback di fine cutscene di inizio bossfight
    /// </summary>
    public void StartCutsceneCallback()
    {
        inputSkipCutscene.performed -= SkipStartBossCutscene;
        groupCtrl.Enable(true);
        OnBossTriggered?.Invoke(bossToEnable);
    }
    #endregion

    #region EndCurscene
    /// <summary>
    /// Funzione che fa partire la cutscene di fine della bossfight
    /// </summary>
    private void EndBossCutscene()
    {
        inputSkipCutscene.performed += SkipEndBossCutscene;
        groupCtrl.Enable(false);
        endBossCutsceneDirector.Play();
    }

    /// <summary>
    /// Funzione che esgue lo skip della cutscene fine bossfight
    /// </summary>
    /// <param name="_context"></param>
    private void SkipEndBossCutscene(InputAction.CallbackContext _context)
    {
        inputSkipCutscene.performed -= SkipEndBossCutscene;
        endBossCutsceneDirector.time = endBossCutsceneDirector.duration;
    }

    /// <summary>
    /// Callback di fine cutscene della fine bossfight
    /// </summary>
    public void EndCutsceneCallback()
    {
        inputSkipCutscene.performed -= SkipEndBossCutscene;
        groupCtrl.Enable(true);
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Agent"))
        {
            if (disableAfterTrigger)
                triggerCollider.enabled = false;

            if (startBossCutsceneDirector != null)
                StartBossCutscene();
            else
                OnBossTriggered?.Invoke(bossToEnable);
        }
    }

    private void OnDisable()
    {
        LevelBossController.OnBossFightEnd -= HandleOnBossFightEnd;
        inputSkipCutscene.performed -= SkipStartBossCutscene;
        inputSkipCutscene.performed -= SkipEndBossCutscene;
        inputSkipCutscene.Disable();
    }
}
