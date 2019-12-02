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
    //Range alla distanza di espansione del gruppo
    [SerializeField]
    private Vector2 expandDistanceRange;

    [Header("Obstacles Settings")]
    //Layer degli ostacoli
    [SerializeField]
    private LayerMask obstaclesLayer;
    //Lunghezza del ray per controllare gli ostacoli
    [SerializeField]
    private float obstacleCheckRayLenght;

    /// <summary>
    /// Distanza di espansione
    /// </summary>
    private float expandDistance;
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
        expandDistance = UnityEngine.Random.Range(expandDistanceRange.x, expandDistanceRange.y);
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
    /// Funzione che ritorna la distanza da tenere quando il gruppo è in espansione
    /// </summary>
    /// <returns></returns>
    public float GetExpandDistance()
    {
        return expandDistance;
    }

    /// <summary>
    /// Funzione che controlla se ci sono ostacoli nel tragitto di espansione
    /// </summary>
    /// <param name="_groupCenter"></param>
    public bool CheckExpandDistance(Vector3 _groupCenter)
    {
        Vector3 rayDirection = (transform.position - _groupCenter).normalized;
        Vector3 origin = transform.position;

        origin.y += 0.5f;
        rayDirection.y = 0;

        Debug.DrawRay(origin, rayDirection * obstacleCheckRayLenght, Color.red);

        Ray ray = new Ray(origin, rayDirection);
        return !Physics.Raycast(ray, obstacleCheckRayLenght, obstaclesLayer);
    }

    /// <summary>
    /// Funzione che controlla se ci sono ostacoli nel tragitto di raggruppamento
    /// </summary>
    /// <param name="_groupCenter"></param>
    public bool CheckGroupDistance(Vector3 _groupCenter)
    {
        Vector3 rayDirection = (_groupCenter - transform.position).normalized;
        Vector3 origin = transform.position;

        origin.y += 0.5f;
        rayDirection.y = 0;

        Debug.DrawRay(origin, rayDirection * obstacleCheckRayLenght, Color.blue);

        Ray ray = new Ray(origin, rayDirection);
        return !Physics.Raycast(ray, obstacleCheckRayLenght, obstaclesLayer);
    }
}
