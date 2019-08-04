﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLoadNextLevel()
    {
        animator.SetTrigger(FADEOUT);
    }

    public void OnReloadLevel()
    {
        OnReloadLevel(null);
    }

    public void OnReloadLevel(Action callback)
    {
        animator.SetTrigger(FADEOUT);

        var activeScene = SceneManager.GetActiveScene();

        this.levelOverride = activeScene.name;

        this.callback = callback;
    }

    private void OnFadeOutCompleted()
    {
        if(this.callback != null)
        {
            this.callback();
            this.callback = null;
        }

        if(string.IsNullOrEmpty(this.levelOverride))
        {
            var activeScene = SceneManager.GetActiveScene();
            var currentBuildIndex = activeScene.buildIndex;
            var nextBuildIndex = (currentBuildIndex + 1) % SceneManager.sceneCountInBuildSettings;
            SceneManager.LoadScene(nextBuildIndex);
        }
        else
        {
            SceneManager.LoadScene(this.levelOverride);
        }
    }

    private Animator animator;
    private string levelOverride;
    private Action callback;
    private const string FADEOUT = "FadeOut";
}
