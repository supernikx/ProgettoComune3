using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickSceneSwitcher : MonoBehaviour
{
    [System.Serializable]
    public struct SceneKey
    {
        public Object scene;
        public KeyCode key;
    }
    public List<SceneKey> scenes;

    private void Update()
    {
        foreach (SceneKey s in scenes)
        {
            if (Input.GetKeyDown(s.key))
                SceneManager.LoadScene(s.scene.name);
        }
    }
}
