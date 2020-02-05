using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che si occupa di gestire il moviemento dell'agent
/// </summary>
public class AgentMovementController : MonoBehaviour
{
    [Header("Agent Movement Settings")]
    //Range della velocità di movimento dell'agent
    [SerializeField]
    private Vector2 movementSpeedRange;
    [SerializeField]
    private LayerMask obstacleLayer;

    /// <summary>
    /// Velocità di movimento
    /// </summary>
    private float movementSpeed;

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_agentCtrl"></param>
    public void Setup()
    {
        movementSpeed = Random.Range(movementSpeedRange.x, movementSpeedRange.y);
    }

    /// <summary>
    /// Funzione che controlla se ci sono collisioni nel nuovo punto di moviemento
    /// </summary>
    /// <param name="_newPos"></param>
    /// <returns></returns>
    private Vector3 CheckCollision(Vector3 _newPos, Vector3 _dir, float _speed)
    {
        float distance = Vector3.Distance(_newPos, transform.position);
        Vector3 fiexdPos = transform.position;
        fiexdPos.y += 0.5f;
        Ray ray = new Ray(fiexdPos, _dir);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance + 0.2f, obstacleLayer))
        {
            Vector3 newDir = (_dir.normalized + hitInfo.normal).normalized;
            return Vector3.MoveTowards(transform.position, transform.position + newDir, _speed);
        }

        return _newPos;
    }

    #region API
    /// <summary>
    /// Funzione che esegue il moviemento dell'agent
    /// </summary>
    /// <param name="_movementDirection"></param>
    /// <param name="_lookAtDirection"></param>
    /// <param name="_speedMultiplier"></param>
    public void Move(Vector3 _movementDirection, bool _lookAtDirection, float _speedMultiplier = 1f)
    {
        float speed = movementSpeed * _speedMultiplier * Time.deltaTime;

        //Se devo guardare la direzione in cui sto andando ruoto
        if (_lookAtDirection)
        {
            Vector3 directionToLook = _movementDirection;
            directionToLook.y = 0f;
            if (directionToLook != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(directionToLook);
        }

        //Calcolo la nuova posizione e controllo le collisioni, se non c'è niente mi muovo
        Vector3 newPos = Vector3.MoveTowards(transform.position, transform.position + _movementDirection, speed);
        transform.position = CheckCollision(newPos, _movementDirection, speed);
    }

    #region Getter
    #endregion
    #endregion
}
