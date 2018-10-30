using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Example : MonoBehaviour
{
    void Load()
    {
		StartCoroutine(LoadYourAsyncScene());
    }

    IEnumerator LoadYourAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Devland");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}