using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSceneInstantiator : MonoBehaviour
{
    [Header("Core Objects")]
    [SerializeField]
    private List<GameObject> coreObjects;

    public void Setup()
    {
        InstantiateObjects();
    }

    private void InstantiateObjects()
    {
        for (int i = 0; i < coreObjects.Count; i++)
        {
            if (!GameObject.Find(coreObjects[i].name))
            {
                GameObject newObj = Instantiate(coreObjects[i]);
                newObj.name = coreObjects[i].name;
                DontDestroyOnLoad(newObj);
            }
        }
    }
}
