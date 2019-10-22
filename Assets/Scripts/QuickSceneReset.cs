using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickSceneReset : MonoBehaviour
{
    private static QuickSceneReset i;

    [SerializeField]
    private KeyCode resetSceneKey;
    [SerializeField]
    private KeyCode quitKey;

    private void Awake()
    {
        if (i == null)
        {
            i = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

    }

    void Update()
    {
        if (Input.GetKeyDown(quitKey))
            Application.Quit();

        if (Input.GetKeyDown(resetSceneKey))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
