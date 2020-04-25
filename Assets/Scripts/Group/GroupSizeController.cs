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
	//Velocità di raggruppamento
	[SerializeField]
	private float groupSpeed;
	//Tempo di ricarica dell'abilità di raggruppamento
	[SerializeField]
	private float groupCountdown;
	//Tempo della durata dell'abilità di raggruppamento
	[SerializeField]
	private float groupTime;

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
	/// Riferimento alla coroutine che deve controllare il countdown del resize
	/// </summary>
	private IEnumerator groupCountdowRoutine;
	/// <summary>
	/// Bool che indetifica se il gruppo si sta raggruppando
	/// </summary>
	private bool grouping;
	/// <summary>
	/// Bool che identifica se il gruppo può espandersi/raggrupparsi
	/// </summary>
	private bool canChangeSize;

	private bool oldCanChangeSize;

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

	#region API
	/// <summary>
	/// Funzione chiamata alla pressione del tasto Group dal PlayerInput
	/// </summary>
	public void OnGroup(InputValue _value)
	{
		grouping = (int)_value.Get<float>() == 1;

		if (groupCtrl.IsSetuppedAndEnabled() && canChangeSize)
		{
			if (sizeControllerRoutine != null)
				StopCoroutine(sizeControllerRoutine);

			if (grouping)
			{
				sizeControllerRoutine = GroupCoroutine();
				StartCoroutine(sizeControllerRoutine);
			}
			else if (!grouping && oldCanChangeSize)
			{
				groupCountdowRoutine = GroupCountdownCoroutine();
				StartCoroutine(groupCountdowRoutine);
			}
		}

		if (!canChangeSize)
			OnGroupPressed?.Invoke(false);
		else
			OnGroupPressed?.Invoke(grouping);

		oldCanChangeSize = canChangeSize;
	}

	/// <summary>
	/// Funzione che esegue il raggruppamento dei Pitottini su un punto
	/// </summary>
	/// <param name="_groupPosition"></param>
	public void GroupOnPoint(Vector3 _groupPosition)
	{
		if (sizeControllerRoutine != null)
			StopCoroutine(sizeControllerRoutine);

		sizeControllerRoutine = GroupOnPointCoroutine(_groupPosition);
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
	#endregion

	/// <summary>
	/// Coroutine che raggruppa il gruppo
	/// </summary>
	/// <returns></returns>
	private IEnumerator GroupCoroutine()
	{
		float timer = 0;
		while (grouping && groupCtrl.IsSetuppedAndEnabled())
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
			while (agentsAtWrongDistance > 0 && groupCtrl.IsSetuppedAndEnabled())
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

				timer += Time.deltaTime;
				if (timer >= groupTime)
				{
					grouping = false;
					OnGroupPressed?.Invoke(grouping);
					groupCountdowRoutine = GroupCountdownCoroutine();
					StartCoroutine(groupCountdowRoutine);
					break;
				}
				yield return null;
			}

			yield return null;
		}
	}

	/// <summary>
	/// Coroutine che raggruppa il gruppo in un punto
	/// </summary>
	/// <param name="_groupPosition"></param>
	/// <returns></returns>
	private IEnumerator GroupOnPointCoroutine(Vector3 _groupPosition)
	{
		//Prendo il riferimento agli agent
		List<AgentController> agents = groupCtrl.GetAgents();

		int agentsAtWrongDistance = agents.Count;
		Vector3 groupCenter = _groupPosition;
		Vector3 regroup;

		//Faccio calcolare ad ogni agent la distanza
		for (int i = 0; i < agents.Count; i++)
			agents[i].GetAgentDistanceController().CalculateDistances();

		//Finchè tutti gli agent non sono all distanza corretta li sposto
		while (agentsAtWrongDistance > 0)
		{
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

	/// <summary>
	/// Coroutine che conta il countdown di ricarica del dash
	/// </summary>
	/// <returns></returns>
	private IEnumerator GroupCountdownCoroutine()
	{
		canChangeSize = false;
		yield return new WaitForSeconds(groupCountdown);
		canChangeSize = true;
	}

	#region Handlers
	/// <summary>
	/// Funzione che gestisce l'evento di inizio ricarica
	/// </summary>
	private void HandleOnReloadingStart()
	{
		canChangeSize = false;

		if (groupCountdowRoutine != null)
			StopCoroutine(groupCountdowRoutine);

		if (sizeControllerRoutine != null)
			StopCoroutine(sizeControllerRoutine);
	}

	/// <summary>
	/// Funzione che gestisce l'evento di fine ricarica
	/// </summary>
	private void HandleOnReloadingEnd()
	{
		canChangeSize = true;

		if (groupCountdowRoutine != null)
			StopCoroutine(groupCountdowRoutine);
	}

	/// <summary>
	/// Funzione che gestisce l'evento di morte del gruppo
	/// </summary>
	private void HandleOnGroupDead()
	{
		grouping = false;
		canChangeSize = true;

		if (sizeControllerRoutine != null)
			StopCoroutine(sizeControllerRoutine);

		if (groupCountdowRoutine != null)
			StopCoroutine(groupCountdowRoutine);
	}
	#endregion

	private void OnDisable()
	{
		groupShootCtrl.OnReloadingStart -= HandleOnReloadingStart;
		groupShootCtrl.OnReloadingEnd -= HandleOnReloadingEnd;
		groupCtrl.OnGroupDead -= HandleOnGroupDead;
	}

	#region Unused
	/*
    //Velocità di espansione
    [SerializeField]
    private float expandSpeed;

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
        while (agentsAtWrongDistance > 0 && groupCtrl.IsSetuppedAndEnabled())
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
    */
	#endregion
}