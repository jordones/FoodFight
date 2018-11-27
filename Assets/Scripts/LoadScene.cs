using System.Collections;
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

        // Wait until the asynchronous scene fully loads
        yield return new WaitUntil(() => !asyncLoad.isDone);
    }
}