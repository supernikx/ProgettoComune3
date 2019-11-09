using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSceneSetupper : MonoBehaviour
{
    private void Start()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm == null)
        {
            PoolManager poolMng = FindObjectOfType<PoolManager>();
            poolMng.Setup();

            GroupController groupCtrl = FindObjectOfType<GroupController>();
            groupCtrl.Setup();

            LevelManager lvlMgn = FindObjectOfType<LevelManager>();
            lvlMgn.Setup();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
