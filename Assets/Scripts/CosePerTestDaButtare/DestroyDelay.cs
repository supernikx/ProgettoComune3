using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelay : MonoBehaviour
{
    // Start is called before the first frame update
    public void Setup(float _delay)
    {
        StartCoroutine(DestroyCoroutine(_delay));
    }

    IEnumerator DestroyCoroutine(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        Destroy(gameObject);
    }
}
