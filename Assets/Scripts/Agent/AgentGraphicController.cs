using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che si occupa di gestire la grafica dell'agent
/// </summary>
public class AgentGraphicController : MonoBehaviour
{
    [Header("Graphic Settings")]
    [SerializeField]
    private ObjectTypes deathVFX;

    /// <summary>
    /// Riferimento al group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento all'animator
    /// </summary>
    private Animator anim;
    /// <summary>
    /// Riferimento alla vecchia posizione
    /// </summary>
    private Vector3 oldPosition;
    /// <summary>
    /// Movement speed calcolata
    /// </summary>
    private float calculatedMovementSpeed;
    /// <summary>
    /// Bool che identifica se è stato eseguito il setup della classe
    /// </summary>
    private bool isSetupped = false;

    #region Setup
    /// <summary>
    /// Funzione che inizializza lo script e prende le referenza
    /// </summary>
    public void Init()
    {
        anim = GetComponentInChildren<Animator>();
    }

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    public void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;
        calculatedMovementSpeed = 0f;
        isSetupped = true;
    }

    /// <summary>
    /// Funzione che esegue l'UnSetup
    /// </summary>
    public void UnSetup()
    {
        isSetupped = false;
    }
    #endregion

    private void FixedUpdate()
    {
        if (!isSetupped || !groupCtrl.IsSetuppedAndEnabled())
            return;

        CalculateMovementSpeed();
        oldPosition = transform.position;
    }

    /// <summary>
    /// Funzione che calcola la velocità di moviemento
    /// </summary>
    private void CalculateMovementSpeed()
    {
        calculatedMovementSpeed = (transform.position - oldPosition).magnitude;
        anim.SetFloat("movementSpeed", calculatedMovementSpeed);
    }

    #region API
    /// <summary>
    /// Funnzione che spawna il VFX di morte dell'agent
    /// </summary>
    public void SpawnDeathVFX()
    {
        if (deathVFX != ObjectTypes.None)
        {
            GameObject pooledVFX = PoolManager.instance.GetPooledObject(deathVFX, gameObject);
            if (pooledVFX != null)
            {
                GeneralVFXController vfx = pooledVFX.GetComponent<GeneralVFXController>();
                if (vfx != null)
                    vfx.Spawn(transform.position);
            }
        }
    }
    #endregion
}
