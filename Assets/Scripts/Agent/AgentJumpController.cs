using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe che gestisce il salto dell'agent
/// </summary>
public class AgentJumpController : MonoBehaviour
{
    [Header("Jump Settings")]
    // Altezza massima raggiungibile in unity unit
    [SerializeField]
    private float jumpHeight;
    // Tempo in secondi che ci vuole per raggiungere l'altezza massima del salto
    [SerializeField]
    private float jumpTimeToReachTop;
    // Moltiplicatore per la gravità di caduta
    [SerializeField]
    private float fallingSpeedMultiplier;
    //Delay dalla pressione del tasto al salto effettivo
    [SerializeField]
    private Vector2 jumpDelayRange;

    /// <summary>
    /// Riferimento all'agent controller
    /// </summary>
    private AgentController agentCtrl;
    /// <summary>
    /// Riferimento all'agent collision ctrl
    /// </summary>
    private AgentCollisionController agentCollisionCtrl;
    /// <summary>
    /// Gravità del salto
    /// </summary>
    private float jumpGravity;
    /// <summary>
    /// Gravità del salto
    /// </summary>
    private float jumpFallingGravity;
    /// <summary>
    /// Velocity del salto
    /// </summary>
    private float jumpVelocity;
    /// <summary>
    /// Velocity del salto
    /// </summary>
    private float currentJumpVelocity;
    /// <summary>
    /// Bool che identifica se l'agent può saltare
    /// </summary>
    private bool canJump;
    /// <summary>
    /// Riferimento alla coroutine di salto
    /// </summary>
    private IEnumerator jumpRoutine;

    /// <summary>
    /// Funzione che segue il setup
    /// </summary>
    /// <param name="_agentCtrl"></param>
    public void Setup(AgentController _agentCtrl)
    {
        agentCtrl = _agentCtrl;
        agentCollisionCtrl = agentCtrl.GetAgentCollisionController();

        jumpGravity = -(2 * jumpHeight) / Mathf.Pow(jumpTimeToReachTop, 2);
        jumpFallingGravity = jumpGravity * fallingSpeedMultiplier;
        jumpVelocity = Mathf.Abs(jumpGravity) * jumpTimeToReachTop;

        canJump = true;
    }

    /// <summary>
    /// Funzione che fa partire il salto
    /// </summary>
    public void Jump()
    {
        if (canJump)
        {
            if (jumpRoutine != null)
                StopCoroutine(jumpRoutine);

            jumpRoutine = JumpCoroutine();
            StartCoroutine(jumpRoutine);
        }
    }

    /// <summary>
    /// Coroutine che esegue il salto
    /// </summary>
    /// <returns></returns>
    private IEnumerator JumpCoroutine()
    {
        canJump = false;

        yield return new WaitForSeconds(Random.Range(jumpDelayRange.x, jumpDelayRange.y));

        WaitForFixedUpdate wffu = new WaitForFixedUpdate();
        currentJumpVelocity = jumpVelocity;

        while (!agentCollisionCtrl.CheckGroundCollision(currentJumpVelocity * Time.deltaTime))
        {
            if (currentJumpVelocity < 0)
                currentJumpVelocity += jumpFallingGravity * Time.deltaTime;
            else
                currentJumpVelocity += jumpGravity * Time.deltaTime;

            transform.Translate(Vector3.up * currentJumpVelocity * Time.deltaTime);
            yield return wffu;
        }

        currentJumpVelocity = 0;
        canJump = true;
    }

    private void OnDisable()
    {
        if (jumpRoutine != null)
            StopCoroutine(jumpRoutine);
    }
}
