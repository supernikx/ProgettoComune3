using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce le distanze dell'agent
/// </summary>
public class AgentDistanceController : MonoBehaviour
{
    [Header("Agent Group Settings")]
    //Range alla distanza di raggruppamento del gruppo
    [SerializeField]
    private Vector2 groupDistanceRange;

    [Header("Obstacles Settings")]
    //Layer degli ostacoli
    [SerializeField]
    private LayerMask obstaclesLayer;
    //Lunghezza del ray per controllare gli ostacoli
    [SerializeField]
    private float obstacleCheckRayLenght;

    /// <summary>
    /// Distanza di raggruppamento
    /// </summary>
    private float groupDistance;

    /// <summary>
    /// Funzione che setup
    /// </summary>
    public void Setup()
    {
        CalculateDistances();
    }

    /// <summary>
    /// Funzione che calcola dei valori random per il raggruppamento e l'espansione
    /// </summary>
    public void CalculateDistances()
    {
        groupDistance = UnityEngine.Random.Range(groupDistanceRange.x, groupDistanceRange.y);
    }

    /// <summary>
    /// Funzione che ritorna la distanza da tenere quando il gruppo è in raggruppamento
    /// </summary>
    /// <returns></returns>
    public float GetGroupDistance()
    {
        return groupDistance;
    }

    /// <summary>
    /// Funzione che controlla se ci sono ostacoli nel tragitto di raggruppamento
    /// </summary>
    /// <param name="_groupCenter"></param>
    public bool CheckGroupDistance(Vector3 _groupCenter)
    {
        Vector3 rayDirection = (_groupCenter - transform.position).normalized;
        Vector3 origin = transform.position;

        origin.y += 0.1f;
        rayDirection.y = 0;

        Debug.DrawRay(origin, rayDirection * obstacleCheckRayLenght, Color.blue);

        Ray ray = new Ray(origin, rayDirection);
        return !Physics.Raycast(ray, obstacleCheckRayLenght, obstaclesLayer);
    }
}
