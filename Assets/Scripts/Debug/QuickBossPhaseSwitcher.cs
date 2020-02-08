using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuickBossPhaseSwitcher : MonoBehaviour
{
    public InputAction phase1Map;
    public InputAction phase2Map;
    public InputAction phase3Map;

    int currentBossPhase;

    private void OnEnable()
    {
        phase1Map.Enable();
        phase2Map.Enable();
        phase3Map.Enable();

        phase1Map.performed += GoToPhase1;
        phase2Map.performed += GoToPhase2;
        phase3Map.performed += GoToPhase3;
    }

    private void GoToPhase1(InputAction.CallbackContext _context)
    {
        BossControllerBase bossCtrl = FindObjectOfType<BossControllerBase>();
        if (bossCtrl != null && bossCtrl.IsSetuppedAndEnabled())
        {
            StateMachineBase bossSM  = bossCtrl.GetComponent<StateMachineBase>();
            if (bossSM != null)
            {
                bossSM.GoToState("Phase1");
            }
        }
    }

    private void GoToPhase2(InputAction.CallbackContext _context)
    {
        BossControllerBase bossCtrl = FindObjectOfType<BossControllerBase>();
        if (bossCtrl != null && bossCtrl.IsSetuppedAndEnabled())
        {
            StateMachineBase bossSM = bossCtrl.GetComponent<StateMachineBase>();
            if (bossSM != null)
            {
                bossSM.GoToState("Phase2");
            }
        }
    }

    private void GoToPhase3(InputAction.CallbackContext _context)
    {
        BossControllerBase bossCtrl = FindObjectOfType<BossControllerBase>();
        if (bossCtrl != null && bossCtrl.IsSetuppedAndEnabled())
        {
            StateMachineBase bossSM = bossCtrl.GetComponent<StateMachineBase>();
            if (bossSM != null)
            {
                bossSM.GoToState("Phase3");
            }
        }
    }

    private void OnDisable()
    {
        phase1Map.performed -= GoToPhase1;
        phase2Map.performed -= GoToPhase2;
        phase3Map.performed -= GoToPhase3;

        phase1Map.Disable();
        phase2Map.Disable();
        phase3Map.Disable();
    }
}
