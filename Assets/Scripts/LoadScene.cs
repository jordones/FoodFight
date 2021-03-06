﻿using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string scene = "Devland";

    public void Load()
    {
		StartCoroutine(AsyncLoadScene(scene));
    }

    public static IEnumerator AsyncLoadScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
        Debug.Log(asyncLoad.isDone);

        // Wait until the asynchronous scene fully loads
        yield return new WaitUntil(() => !asyncLoad.isDone);
    }
}