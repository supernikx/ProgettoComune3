using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce le collisioni dell'agent
/// </summary>
public class AgentCollisionController : MonoBehaviour
{
    [Header("Collision Settings")]
    [SerializeField]
    //Layer per le collisione con il terreno
    private LayerMask groundLayer;

    #region Ground Collision
    /// <summary>
    /// Numero di raycast per controllare la collisione con il terreno
    /// </summary>
    private int rayCount = 4;
    /// <summary>
    /// Spazio tra un ray e l'altro
    /// </summary>
    private float raySpacing;
    /// <summary>
    /// Offset del bound del collider
    /// </summary>
    private float collisionOffset = 0.015f;
    /// <summary>
    /// Lunghezza del ray
    /// </summary>
    private float rayLenght = 0.02f;
    /// <summary>
    /// Bool che indentifica se c'è una collision con il ground o no
    /// </summary>
    private bool groundCollision;
    #endregion

    /// <summary>
    /// Riferimento al collider
    /// </summary>
    private new Collider collider;
    /// <summary>
    /// Riferimento all'agent controller
    /// </summary>
    private AgentController agentCtrl;

    /// <summary>
    /// Funzione che esegue il Setup
    /// </summary>
    /// <param name="_agentCtrl"></param>
    public void Setup(AgentController _agentCtrl)
    {
        agentCtrl = _agentCtrl;

        collider = GetComponent<Collider>();

        CalculateRaySpacing();
    }

    /// <summary>
    /// Funzione che calcola lo spazio tra i raycast sia verticali che orrizontali
    /// </summary>
    private void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(collisionOffset * -2);
        raySpacing = bounds.size.x / (rayCount - 1);
    }

    #region API
    /// <summary>
    /// Funzione che controlla se c'è una collisione con il terreno
    /// </summary>
    public bool CheckGroundCollision(float _jumpVelocity = 0f)
    {
        if (Mathf.Sign(_jumpVelocity) > 0)
            return false;

        float rayLenghtToUse = Mathf.Abs(_jumpVelocity) + rayLenght;

        //Calcolo i bounds del collider
        Bounds bounds = collider.bounds;
        bounds.Expand(collisionOffset * -2);        

        //Imposto a false le collisioni
        groundCollision = false;

        //Cicla tutti i punti da cui deve partire un raycast sull'asse verticale
        for (int i = 0; i < rayCount; i++)
        {
            //Calcolo l'origine del ray
            Vector3 rayOrigin = new Vector3(bounds.min.x, bounds.min.y, transform.position.z);
            rayOrigin += Vector3.right * (raySpacing * i);

            //Crea il ray
            Ray ray = new Ray(rayOrigin, Vector3.down);
            RaycastHit hit;

            //Eseguo il raycast
            if (Physics.Raycast(ray, out hit, rayLenghtToUse, groundLayer))
                return true;

            Debug.DrawRay(rayOrigin, Vector3.down * rayLenghtToUse, Color.red);
        }

        return false;
    }

    #region Getter
    /// <summary>
    /// Funzione che ritorna se si è in collisione con il terreno
    /// </summary>
    /// <returns></returns>
    public bool IsGroundCollision()
    {
        return CheckGroundCollision();
    }
    #endregion
    #endregion
}
