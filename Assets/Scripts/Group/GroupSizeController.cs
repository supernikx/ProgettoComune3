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
    /// Bool che indentifica se la classe è setuppata
    /// </summary>
    private bool isSetupped = false;
    /// <summary>
    /// Bool che identifica se il gruppo si deve raggruppare
    /// </summary>
    private bool regroup;
    /// <summary>
    /// Bool che identifica se il gruppo si deve espandere
    /// </summary>
    private bool expand;
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
        isSetupped = true;
    }

    private void Update()
    {
        ReadInput();
    }

    private void FixedUpdate()
    {
        if (regroup || expand)
        {
            expand = regroup = false;
            StartCoroutine(sizeControllerRoutine);
        }
    }


    /// <summary>
    /// Funzione che si occupa di leggere gl'input
    /// </summary>
    private void ReadInput()
    {
        if (Input.GetButtonDown("Regroup"))
        {
            if (expand)
                expand = false;

            if (sizeControllerRoutine != null)
                StopCoroutine(sizeControllerRoutine);

            sizeControllerRoutine = RegroupGroupCoroutine();
            regroup = true;
        }

        if (Input.GetButtonDown("Expand"))
        {
            if (regroup)
                regroup = false;

            if (sizeControllerRoutine != null)
                StopCoroutine(sizeControllerRoutine);

            sizeControllerRoutine = ExpandGroupCoroutine();

            expand = true;
        }
    }

    /// <summary>
    /// Coroutine che espande il gruppo
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExpandGroupCoroutine()
    {
        //Prendo il riferimento agli agent
        List<AgentController> agents = groupCtrl.GetAgents();
        int agentsAtCorrectDistance = agents.Count;
        Vector3 groupCenter;
        Vector3 expandDirection;

        //Finchè tutti gli agent non sono all distanza corretta li sposto
        while (agentsAtCorrectDistance > 0)
        {
            groupCenter = groupCtrl.GetGroupCenterPoint();
            agentsAtCorrectDistance = 0;

            for (int i = 0; i < agents.Count; i++)
            {
                //Controllo la distanza tra l'agent e il centro del gruppo
                if (Vector3.Distance(agents[i].transform.position, groupCenter) < agents[i].GetExpandDistance())
                {
                    //Se è minore di quella prevista per l'espansione lo sposto
                    expandDirection = (agents[i].transform.position - groupCenter).normalized;
                    expandDirection.y = 0;
                    agents[i].GetAgentMovementController().Move(expandDirection, false);
                    agentsAtCorrectDistance++;
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// Coroutine che raggruppa il gruppo
    /// </summary>
    /// <returns></returns>
    private IEnumerator RegroupGroupCoroutine()
    {
        //Prendo il riferimento agli agent
        List<AgentController> agents = groupCtrl.GetAgents();
        int agentsAtCorrectDistance = agents.Count;
        Vector3 groupCenter;
        Vector3 regroup;

        //Finchè tutti gli agent non sono all distanza corretta li sposto
        while (agentsAtCorrectDistance > 0)
        {
            groupCenter = groupCtrl.GetGroupCenterPoint();
            agentsAtCorrectDistance = 0;

            for (int i = 0; i < agents.Count; i++)
            {
                //Controllo la distanza tra l'agent e il centro del gruppo
                if (Vector3.Distance(agents[i].transform.position, groupCenter) > agents[i].GetRegroupDistance())
                {
                    //Se è maggiore di quella prevista per il raggruppamento lo sposto
                    regroup = (groupCenter - agents[i].transform.position).normalized;
                    regroup.y = 0;
                    agents[i].GetAgentMovementController().Move(regroup, false);
                    agentsAtCorrectDistance++;
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }
}