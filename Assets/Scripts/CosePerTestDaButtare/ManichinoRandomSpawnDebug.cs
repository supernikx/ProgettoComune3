using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManichinoRandomSpawnDebug : MonoBehaviour
{
    [SerializeField]
    private Transform topLeft;
    [SerializeField]
    private Transform bottomRight;

    [SerializeField]
    private GameObject objectToMove;
    [SerializeField]
    private float delayMove;

    IEnumerator spawnRoutine;

    private void Start()
    {
        spawnRoutine = SpawnCoroutine();
        StartCoroutine(spawnRoutine);
    }

    private IEnumerator SpawnCoroutine()
    {
        Vector3 newPos = objectToMove.transform.position;
        while (true)
        {
            newPos.x = Random.Range(topLeft.position.x, bottomRight.position.x);
            newPos.z = Random.Range(bottomRight.position.z, topLeft.position.z);
            objectToMove.transform.position = newPos;
            objectToMove.SetActive(true);
            yield return new WaitForSeconds(delayMove);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (spawnRoutine != null)
            {
                StopCoroutine(spawnRoutine);
                spawnRoutine = null;
            }
            else
            {
                spawnRoutine = SpawnCoroutine();
                StartCoroutine(spawnRoutine);
            }
        }

    }
}
