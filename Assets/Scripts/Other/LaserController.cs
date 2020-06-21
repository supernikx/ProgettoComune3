using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il laser
/// </summary>
public class LaserController : MonoBehaviour
{
	#region Actions
	/// <summary>
	/// Evento che notifica che un agent è stato colpito
	/// </summary>
	public Action<AgentController> OnAgentHit;
	#endregion

	[Header("Laser References")]
	//Layer con cui può collider il laser
	[SerializeField]
	private LayerMask laserColliderLayer;
	//Layer degli agent
	[SerializeField]
	private LayerMask agentLayer;
	//Riferimento al LineRenderer
	[SerializeField]
	private LineRenderer[] lineRenderer;

	[Header("Graphic References")]
	[SerializeField]
	private ParticleSystem shootLaserVFX;

	/// <summary>
	/// Raggio del laser
	/// </summary>
	private float laserRadius;
	/// <summary>
	/// Lunghezza massima del laser
	/// </summary>
	private float maxLaserRange;
	/// <summary>
	/// Bool che identifica se il laser è attivo
	/// </summary>
	private bool enable;
	/// <summary>
	/// Riferimento alla coroutine di spawn del laser
	/// </summary>
	private IEnumerator spawnRoutine;
	/// <summary>
	/// Riferimento alla coroutine di rotazione del laser
	/// </summary>
	private IEnumerator rotateRoutine;

	/// <summary>
	/// Funzione di Setup
	/// </summary>
	/// <param name="_maxLaserRange"></param>
	/// <param name="_laserRadius"></param>
	public void Setup(float _maxLaserRange, float _laserRadius)
	{
		maxLaserRange = _maxLaserRange;
		laserRadius = _laserRadius;

		StopLaser();
	}

	// Update is called once per frame
	private void Update()
	{
		if (enable)
		{
			Vector3 nextLaserPoint;
			float checkAgentDistance = maxLaserRange;
			RaycastHit hit;

			//Controllo se colpisco un ostacolo
			if (Physics.SphereCast(transform.position, laserRadius, transform.forward, out hit, maxLaserRange, laserColliderLayer))
			{
				if (hit.collider)
				{
					nextLaserPoint = hit.point;
					checkAgentDistance = hit.distance;
				}
				else
				{
					nextLaserPoint = transform.position + (transform.forward * maxLaserRange);
				}
			}
			else
			{
				nextLaserPoint = transform.position + (transform.forward * maxLaserRange);
			}

			//Controllo se colpisco un agent al max range dell'ostacolo
			RaycastHit[] agentHit = Physics.SphereCastAll(transform.position, laserRadius, transform.forward, checkAgentDistance, agentLayer);

			for (int i = 0; i < agentHit.Length; i++)
			{
				AgentController agent = agentHit[i].transform.gameObject.GetComponent<AgentController>();
				if (agent != null)
					OnAgentHit?.Invoke(agent);
			}

			for (int i = 0; i < lineRenderer.Length; i++)
			{
				lineRenderer[i].SetPosition(1, nextLaserPoint);
			}

			shootLaserVFX.transform.position = lineRenderer[0].GetPosition(0) + shootLaserVFX.transform.forward * 3f;
			shootLaserVFX.transform.rotation = Quaternion.LookRotation(lineRenderer[0].GetPosition(1));
		}
	}

	#region API
	/// <summary>
	/// Funzione che avvia la coroutine di spawn del laser
	/// </summary>
	/// <param name="_spawnTime"></param>
	/// <param name="_spawnDirection"></param>
	/// <param name="_onSpawnCallback"></param>
	public void SpawnLaser(float _spawnTime, float _spawnAngle, Action _onSpawnCallback)
	{
		transform.localRotation = Quaternion.Euler(new Vector3(0, _spawnAngle, 0));

		spawnRoutine = SpawnLaserCoroutine(_spawnTime, 0, _onSpawnCallback);
		StartCoroutine(spawnRoutine);
	}

	/// <summary>
	/// Funzione che avvia la coroutine di spawn del laser
	/// </summary>
	/// <param name="_spawnTime"></param>
	/// <param name="_spawnDirection"></param>
	/// <param name="_onSpawnCallback"></param>
	public void SpawnLaser(float _spawnTime, float _spawnDelay, Vector3 _targetPosition, Action _onSpawnCallback)
	{
		_targetPosition.y = transform.position.y;
		transform.localRotation = Quaternion.LookRotation((transform.position - _targetPosition).normalized, Vector3.up);

		spawnRoutine = SpawnLaserCoroutine(_spawnTime, _spawnDelay, _onSpawnCallback);
		StartCoroutine(spawnRoutine);
	}


	/// <summary>
	/// Funzione che esegue lo start del laser
	/// </summary>
	public void StartLaser(float _targetAngle, float _speed, Action _onRotateCallback)
	{
		rotateRoutine = RotateLaserCoroutine(_targetAngle, _speed, _onRotateCallback);
		StartCoroutine(rotateRoutine);
		shootLaserVFX.Play();
		enable = true;
	}

	/// <summary>
	/// Funzione che esegue lo start del laser
	/// </summary>
	public void StartLaser(Transform _targetTransform, float _duaration, float _speed, Action _onRotateCallback)
	{
		rotateRoutine = RotateLaserCoroutine(_targetTransform, _duaration, _speed, _onRotateCallback);
		StartCoroutine(rotateRoutine);
		shootLaserVFX.Play();
		enable = true;
	}

	/// <summary>
	/// Funzione che stoppa il laser
	/// </summary>
	public void StopLaser()
	{
		for (int i = 0; i < lineRenderer.Length; i++)
			lineRenderer[i].enabled = false;
		enable = false;

		if (rotateRoutine != null)
			StopCoroutine(rotateRoutine);

		if (spawnRoutine != null)
			StopCoroutine(spawnRoutine);

		shootLaserVFX.Stop();
	}

	#region Getter
	/// <summary>
	/// Funzione che ritorna se il laser è attivo
	/// </summary>
	/// <returns></returns>
	public bool IsEnable()
	{
		return enable;
	}
	#endregion
	#endregion

	/// <summary>
	/// Coroutine che fa spawnare il laser
	/// </summary>
	/// <param name="_spawnTime"></param>
	/// <param name="_onSpawnCallback"></param>
	/// <returns></returns>
	private IEnumerator SpawnLaserCoroutine(float _spawnTime, float _spawnDelay, Action _onSpawnCallback)
	{
		if (_spawnDelay != 0)
			yield return new WaitForSeconds(_spawnDelay);

		for (int i = 0; i < lineRenderer.Length; i++)
		{
			lineRenderer[i].enabled = true;
			lineRenderer[i].positionCount = 2;
			lineRenderer[i].SetPosition(0, transform.position);
			lineRenderer[i].startWidth = lineRenderer[i].startWidth = laserRadius;
		}

		float laserRange = 0;
		float rangeOffset;
		WaitForFixedUpdate wffu = new WaitForFixedUpdate();

		if (_spawnTime != 0)
			rangeOffset = maxLaserRange / _spawnTime;
		else
			rangeOffset = maxLaserRange;


		while (laserRange < maxLaserRange)
		{
			laserRange += rangeOffset * Time.deltaTime;

			Vector3 nextLaserPoint;
			float checkAgentDistance = laserRange;
			RaycastHit hit;

			//Controllo se colpisco un ostacolo
			if (Physics.SphereCast(transform.position, laserRadius, transform.forward, out hit, laserRange, laserColliderLayer))
			{
				if (hit.collider)
				{
					nextLaserPoint = hit.point;
					checkAgentDistance = hit.distance;
				}
				else
				{
					nextLaserPoint = transform.position + (transform.forward * laserRange);
				}
			}
			else
			{
				nextLaserPoint = transform.position + (transform.forward * laserRange);
			}

			//Controllo se colpisco un agent al max range dell'ostacolo
			RaycastHit[] agentHit = Physics.SphereCastAll(transform.position, laserRadius, transform.forward, checkAgentDistance, agentLayer);

			for (int i = 0; i < agentHit.Length; i++)
			{
				AgentController agent = agentHit[i].transform.gameObject.GetComponent<AgentController>();
				if (agent != null)
					OnAgentHit?.Invoke(agent);
			}

			for (int i = 0; i < lineRenderer.Length; i++)
				lineRenderer[i].SetPosition(1, nextLaserPoint);
			yield return wffu;
		}

		_onSpawnCallback?.Invoke();
	}

	/// <summary>
	/// Coroutine che fa ruotare il laser
	/// </summary>
	/// <param name="_targetAngle"></param>
	/// <param name="_speed"></param>
	/// <param name="_onRotateCallback"></param>
	/// <returns></returns>
	private IEnumerator RotateLaserCoroutine(float _targetAngle, float _speed, Action _onRotateCallback)
	{
		for (int i = 0; i < lineRenderer.Length; i++)
			lineRenderer[i].enabled = true;
		WaitForFixedUpdate wffu = new WaitForFixedUpdate();
		Quaternion targetRotation = Quaternion.Euler(new Vector3(0, _targetAngle, 0));

		while (transform.localRotation != targetRotation)
		{
			transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, _speed * Time.deltaTime);
			yield return wffu;
		}

		_onRotateCallback?.Invoke();
	}

	/// <summary>
	/// Coroutine che fa ruotare il laser
	/// </summary>
	/// <param name="_targetAngle"></param>
	/// <param name="_speed"></param>
	/// <param name="_onRotateCallback"></param>
	/// <returns></returns>
	private IEnumerator RotateLaserCoroutine(Transform _targetTransform, float _duration, float _speed, Action _onRotateCallback)
	{
		for (int i = 0; i < lineRenderer.Length; i++)
			lineRenderer[i].enabled = true;
		WaitForFixedUpdate wffu = new WaitForFixedUpdate();
		Quaternion targetRotation = Quaternion.LookRotation((transform.position - _targetTransform.position).normalized, Vector3.up);
		float timer = 0f;

		while (timer < _duration)
		{
			timer += Time.deltaTime;
			transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, _speed * Time.deltaTime);
			targetRotation = Quaternion.LookRotation((transform.position - _targetTransform.position).normalized, Vector3.up);
			yield return wffu;
		}

		_onRotateCallback?.Invoke();
	}
}
