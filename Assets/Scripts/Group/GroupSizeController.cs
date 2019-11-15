using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che controlla la size del gruppo
/// </summary>
public class GroupSizeController : MonoBehaviour
{
    /// <summary>
    /// Riferimento al group controller
    /// </summary>
    private GroupController groupCtrl;
    /// <summary>
    /// Riferimento alla coroutine che deve controllare la size del gruppo (expand o regroup)
    /// </summary>
    IEnumerator sizeControllerRoutine;

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_groupCtrl"></param>
    public void Setup(GroupController _groupCtrl)
    {
        groupCtrl = _groupCtrl;
    }

    /// <summary>
    /// Funzione chiamata alla pressione del tasto Group dal PlayerInput
    /// </summary>
    public void OnGroup()
    {
        if (!groupCtrl.IsSetuppedAndEnabled())
            return;

        if (sizeControllerRoutine != null)
            StopCoroutine(sizeControllerRoutine);

        sizeControllerRoutine = GroupCoroutine();
        StartCoroutine(sizeControllerRoutine);
    }

    /// <summary>
    /// Funzione chiamata alla pressione del tasto Expand dal PlayerInput
    /// </summary>
    public void OnExpand()
    {
        if (!groupCtrl.IsSetuppedAndEnabled())
            return;

        if (sizeControllerRoutine != null)
            StopCoroutine(sizeControllerRoutine);

        sizeControllerRoutine = ExpandGroupCoroutine();
        StartCoroutine(sizeControllerRoutine);
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
                        agents[i].GetAgentMovementController().Move(expandDirection, false);
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
        //Prendo il riferimento agli agent
        List<AgentController> agents = groupCtrl.GetAgents();
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
                if (distanceCtrl.CheckRegroupDistance(groupCenter))
                {
                    //Calcolo la distanza
                    float distance = Vector3.Distance(agents[i].transform.position, groupCenter);

                    //Controllo la distanza tra l'agent e il centro del gruppo
                    if (distance > distanceCtrl.GetRegroupDistance())
                    {
                        //Se è maggiore di quella prevista per il raggruppamento lo sposto
                        regroup = (groupCenter - agents[i].transform.position).normalized;
                        regroup.y = 0;
                        agents[i].GetAgentMovementController().Move(regroup, false);
                        agentsAtWrongDistance++;
                    }
                }
            }

            yield return null;
        }
    }
}