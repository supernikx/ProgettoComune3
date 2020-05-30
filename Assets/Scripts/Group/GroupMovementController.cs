using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Classe che gestisce gl'input del gruppo
/// </summary>
public class GroupMovementController : MonoBehaviour
{
	#region Actions
	/// <summary>
	/// Evento che notifica che il gruppo si sta muovendo
	/// </summary>
	public Action OnGroupMove;
	#endregion

	[Header("Group Movement Settings")]
	//Variabile che identifica il moltiplicatore della velocità se è rimsato solo 1 agent
	[SerializeField]
	private float singleAgentSpeedMultipiler;
	//Variabile che identifica il moltiplicatore della velocità se il gruppo è in raggruppamento
	[SerializeField]
	private float groupSpeedMultiplier;
	//Tempo di ricarica dell'abilità di raggruppamento
	[SerializeField]
	private float groupCountdown;
	//Tempo della durata dell'abilità di raggruppamento
	[SerializeField]
	private float groupTime;

	/// <summary>
	/// Rifeirmento al Group controller
	/// </summary>
	private GroupController groupCtrl;
	/// <summary>
	/// Rifeirmento al Group Size Controller
	/// </summary>
	private GroupSizeController sizeCtrl;
	/// <summary>
	/// Riferimento al vettore di movimento
	/// </summary>
	private Vector3 movementVector;
	/// <summary>
	/// bool che identifica se il gruppo può muoversi
	/// </summary>
	private bool canMove = false;
	/// <summary>
	/// bool che identifica se il gruppo si sta raggruppando
	/// </summary>
	private bool grouping = false;
	/// <summary>
	/// bool che identifica se il gruppo può dashare
	/// </summary>
	private bool canDash = true;
	/// <summary>
	/// Riferiemento alla coroutine di boost
	/// </summary>
	private IEnumerator groupBoostCoroutine;
	/// <summary>
	/// Riferiemento alla coroutine di cooldown del boost
	/// </summary>
	private IEnumerator groupBoostCoolDownCoroutine;

	/// <summary>
	/// Funzione che esegue il Setup
	/// </summary>
	/// <param name="_groupCtrl"></param>
	public void Setup(GroupController _groupCtrl)
	{
		groupCtrl = _groupCtrl;
		sizeCtrl = groupCtrl.GetGroupSizeController();

		sizeCtrl.OnGroupPressed += HandleOnGroupPressed;
		canMove = true;
		canDash = true;
		grouping = false;
	}

	private void Update()
	{
		if (!groupCtrl.IsSetuppedAndEnabled() || !canMove)
			return;

		MoveAgents();
	}

	/// <summary>
	/// Funzione chiamata al movimento dal PlayerInput
	/// </summary>
	public void OnMove(InputValue _value)
	{
		Vector2 newMove = _value.Get<Vector2>();
		movementVector.x = newMove.x;
		movementVector.z = newMove.y;
	}

	/// <summary>
	/// Funzione che si occupa di muovere tutti gli agent
	/// </summary>
	private void MoveAgents()
	{
		List<AgentController> agents = groupCtrl.GetAgents();
		if (agents == null || agents.Count == 0)
			return;

		//Movimento
		if (movementVector == Vector3.zero)
			return;

		float speedMultiplier = (agents.Count == 1) ? singleAgentSpeedMultipiler : ((grouping && canDash) ? groupSpeedMultiplier : 1);

		foreach (AgentController agent in agents)
			agent.GetAgentMovementController().Move(movementVector.normalized, true, speedMultiplier);

		OnGroupMove?.Invoke();
	}

	/// <summary>
	/// Funzione che gestisce l'evento di raggruppamento
	/// </summary>
	/// <param name="_value"></param>
	private void HandleOnGroupPressed(bool _value)
	{
		grouping = _value;
		if (grouping)
		{
			if (canDash)
			{
				if (groupBoostCoroutine != null)
					StopCoroutine(groupBoostCoroutine);

				groupBoostCoroutine = GroupDashCoroutine();
				StartCoroutine(groupBoostCoroutine);
			}
		}
		else
		{
			if (groupBoostCoroutine != null)
				StopCoroutine(groupBoostCoroutine);

			if (groupBoostCoolDownCoroutine == null)
			{
				groupBoostCoolDownCoroutine = GroupDashCoolDownCoroutine();
				StartCoroutine(groupBoostCoolDownCoroutine);
			}
		}
	}

	/// <summary>
	/// Coroutine che conta il tempo dell'abilità di dash
	/// </summary>
	/// <returns></returns>
	private IEnumerator GroupDashCoroutine()
	{
		float timer = 0f;
		while (groupTime > timer)
		{
			timer += Time.deltaTime;
			yield return null;
		}

		groupBoostCoolDownCoroutine = GroupDashCoolDownCoroutine();
		StartCoroutine(groupBoostCoolDownCoroutine);

		groupBoostCoroutine = null;
	}

	/// <summary>
	/// Coroutine che conta il countdown di ricarica del dash
	/// </summary>
	/// <returns></returns>
	private IEnumerator GroupDashCoolDownCoroutine()
	{
		canDash = false;
		yield return new WaitForSeconds(groupCountdown);
		canDash = true;

		if (grouping)
		{
			groupBoostCoroutine = GroupDashCoroutine();
			StartCoroutine(groupBoostCoroutine);
		}

		groupBoostCoolDownCoroutine = null;
	}

	#region API
	/// <summary>
	/// Funzione che muove tutti gli agent nella direzione della posizione e alla velocità passati come parametro
	/// </summary>
	/// <param name="_movementDirection"></param>
	/// <param name="_movementSpeed"></param>
	public void MoveAgentsToPointDirection(Vector3 _movementPosition, float _movementSpeed)
	{
		//Prendo il riferimento agli agent
		List<AgentController> agents = groupCtrl.GetAgents();
		Vector3 movementDirection;

		for (int i = 0; i < agents.Count; i++)
		{
			movementDirection = (_movementPosition - agents[i].transform.position).normalized;
			movementDirection.y = 0;
			agents[i].GetAgentMovementController().Move(movementDirection, false, _movementSpeed);
		}
	}

	/// <summary>
	/// Funzione che resetta il vettore di moviemento a 0
	/// </summary>
	public void ResetMovementVelocity()
	{
		movementVector = Vector3.zero;
	}

	#region Setter
	/// <summary>
	/// Funzione che imposta la variabile can move con il valore passato come parametro
	/// </summary>
	/// <param name="_canMove"></param>
	public void SetCanMove(bool _canMove)
	{
		canMove = _canMove;
	}
	#endregion
	#endregion

	private void OnDisable()
	{
		if (sizeCtrl != null)
			sizeCtrl.OnGroupPressed -= HandleOnGroupPressed;
	}
}
