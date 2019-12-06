using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossDamageable
{
    /// <summary>
    /// Funzione che toglie vita al boss
    /// </summary>
    /// <param name="_damage"></param>
    void TakeDamage(int _damage);
}
