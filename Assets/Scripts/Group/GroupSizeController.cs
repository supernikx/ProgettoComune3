using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Classe che controlla la size del gruppo
/// </summary>
public class GroupSizeController : MonoBehaviour
{
    #region Actions
    public Action<bool> OnGroupPressed;
    #endregion

    [Header("Size Settings")]
    //Velocità di espansione
    [SerializeField]
    private float expandSpeed;
    //Velocità di raggruppamento
    [SerializeField]
    private float groupSpeed;

    /// <summary>
    /// Riferimento al group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento al group shoot controller
    /// </summary>
    private GroupShootController groupShootCtrl;
    /// <summary>
    /// Riferimento alla coroutine che deve controllare la size del gruppo (expand o regroup)
    /// </summary>
    private IEnumerator sizeControllerRoutine;
    /// <summary>
    /// Bool che indetifica se il gruppo si sta raggruppando
    /// </summary>
    private bool grouping;
    /// <summary>
    /// Bool che identifica se il gruppo può espandersi/raggrupparsi
    /// </summary>
    private bool canChangeSize;

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    public void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;
        groupShootCtrl = groupCtrl.GetGroupShootController();

        groupShootCtrl.OnReloadingStart += HandleOnReloadingStart;
        groupShootCtrl.OnReloadingEnd += HandleOnReloadingEnd;
        groupCtrl.OnGroupDead += HandleOnGroupDead;

        canChangeSize = true;
    }

    /// <summary>
    /// Funzione chiamata alla pressione del tasto Group dal PlayerInput
    /// </summary>
    public void OnGroup(InputValue _value)
    {
        grouping = (int)_value.Get<float>() == 1;

        if (!groupCtrl.IsSetuppedAndEnabled() || !canChangeSize)
            return;

        if (sizeControllerRoutine != null)
            StopCoroutine(sizeControllerRoutine);

        if (grouping)
        {
            sizeControllerRoutine = GroupCoroutine();
            StartCoroutine(sizeControllerRoutine);
        }

        OnGroupPressed?.Invoke(grouping);
    }

    /// <summary>
    /// Funzione chiamata alla pressione del tasto Expand dal PlayerInput
    /// </summary>
    public void OnExpand()
    {
        if (!groupCtrl.IsSetuppedAndEnabled() || !canChangeSize)
            return;

        if (sizeControllerRoutine != null)
            StopCoroutine(sizeControllerRoutine);

        sizeControllerRoutine = ExpandGroupCoroutine();
        StartCoroutine(sizeControllerRoutine);
    }

    /// <summary>
    /// Funzione che ritorna se il gruppo si sta raggruppando o no
    /// </summary>
    /// <returns></returns>
    public bool IsGrouping()
    {
        return grouping;
    }

    /// <summary>
    /// Coroutine che espande il gruppo
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExpandGroupCoroutine()
    {
        //Prendo il riferimento agli agent
        List<AgentController> agents = groupCtrl.GetAgents();
        int agentsAtWrongDistance = agents.Count;
        Vector3 groupCenter;
        Vector3 expandDirection;

        //Faccio calcolare ad ogni agent la distanza
        for (int i = 0; i < agents.Count; i++)
            agents[i].GetAgentDistanceController().CalculateDistances();


        //Finchè tutti gli agent non sono all distanza corretta li sposto
        while (agentsAtWrongDistance > 0)
        {
            groupCenter = groupCtrl.GetGroupCenterPoint();
            agentsAtWrongDistance = 0;

            for (int i = 0; i < agents.Count; i++)
            {
                //L'agent controlla se ci sono ostacoli nel tragitto
                AgentDistanceController distanceCtrl = agents[i].GetAgentDistanceController();
                if (distanceCtrl.CheckExpandDistance(groupCenter))
                {
                    //Calcolo la distanza
                    float distance = Vector3.Distance(agents[i].transform.position, groupCenter);

                    //Controllo la distanza tra l'agent e il centro del gruppo
                    if (distance < distanceCtrl.GetExpandDistance())
                    {
                        //Se è minore di quella prevista per l'espansione lo sposto
                        expandDirection = (agents[i].transform.position - groupCenter).normalized;
                        expandDirection.y = 0;
                        agents[i].GetAgentMovementController().Move(expandDirection, false, expandSpeed);
                        agentsAtWrongDistance++;
                    }
                }
            }

            yield return null;
        }
    }

    /// <summary>
    /// Coroutine che raggruppa il gruppo
    /// </summary>
    /// <returns></returns>
    private IEnumerator GroupCoroutine()
    {
        while (grouping)
        {
            //Prendo il riferimento agli agent
            List<AgentController> agents = groupCtrl.GetAgents();
            if (agents == null || agents.Count == 0 || agents.Count == 1)
                grouping = false;

            int agentsAtWrongDistance = agents.Count;
            Vector3 groupCenter;
            Vector3 regroup;

            //Faccio calcolare ad ogni agent la distanza
            for (int i = 0; i < agents.Count; i++)
                agents[i].GetAgentDistanceController().CalculateDistances();

            //Finchè tutti gli agent non sono all distanza corretta li sposto
            while (agentsAtWrongDistance > 0)
            {
                groupCenter = groupCtrl.GetGroupCenterPoint();
                agentsAtWrongDistance = 0;

                for (int i = 0; i < agents.Count; i++)
                {
                    //L'agent controlla se ci sono ostacoli nel tragitto
                    AgentDistanceController distanceCtrl = agents[i].GetAgentDistanceController();
                    if (distanceCtrl.CheckGroupDistance(groupCenter))
                    {
                        //Calcolo la distanza
                        float distance = Vector3.Distance(agents[i].transform.position, groupCenter);

                        //Controllo la distanza tra l'agent e il centro del gruppo
                        if (distance > distanceCtrl.GetGroupDistance())
                        {
                            //Se è maggiore di quella prevista per il raggruppamento lo sposto
                            regroup = (groupCenter - agents[i].transform.position).normalized;
                            regroup.y = 0;
                            agents[i].GetAgentMovementController().Move(regroup, false, groupSpeed);
                            agentsAtWrongDistance++;
                        }
                    }
                }

                yield return null;
            }
        }
    }

    #region Handlers
    /// <summary>
    /// Funzione che gestisce l'evento di inizio ricarica
    /// </summary>
    /// <param name="_reloadingTime"></param>
    private void HandleOnReloadingStart(float _reloadingTime)
    {
        canChangeSize = false;

        if (sizeControllerRoutine != null)
            StopCoroutine(sizeControllerRoutine);
    }

    /// <summary>
    /// Funzione che gestisce l'evento di fine ricarica
    /// </summary>
    private void HandleOnReloadingEnd()
    {
        canChangeSize = true;
    }

    /// <summary>
    /// Funzione che gestisce l'evento di morte del gruppo
    /// </summary>
    private void HandleOnGroupDead()
    {
        grouping = false;

        if (sizeControllerRoutine != null)
            StopCoroutine(sizeControllerRoutine);
    }
    #endregion

    private void OnDisable()
    {
        groupShootCtrl.OnReloadingStart -= HandleOnReloadingStart;
        groupShootCtrl.OnReloadingEnd -= HandleOnReloadingEnd;
        groupCtrl.OnGroupDead -= HandleOnGroupDead;
    }
}