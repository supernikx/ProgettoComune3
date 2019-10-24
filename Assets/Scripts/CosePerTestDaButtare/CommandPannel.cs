using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandPannel : MonoBehaviour
{
    public GameObject commandPannel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            commandPannel.SetActive(!commandPannel.activeSelf);
    }
}
