using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashSceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(LoadFirstScene), 4f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadFirstScene()
    {
        SceneManager.LoadScene(1);
    }
}
