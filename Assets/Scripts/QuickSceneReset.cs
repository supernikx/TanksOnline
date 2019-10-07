using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickSceneReset : MonoBehaviour
{
    [SerializeField]
    private KeyCode resetSceneKey;
    [SerializeField]
    private KeyCode quitKey;

    void Update()
    {
        if (Input.GetKeyDown(quitKey))
            Application.Quit();

        if (Input.GetKeyDown(resetSceneKey))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
