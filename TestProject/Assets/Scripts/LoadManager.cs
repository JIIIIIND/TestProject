using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour {

    public static string nextSceneName;
    [SerializeField] private Image loadingBar;

	void Start ()
    {
        StartCoroutine(LoadScene());
	}

    public static void LoadScene(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("Loading");
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation async = SceneManager.LoadSceneAsync(nextSceneName);
        async.allowSceneActivation = false;

        float timer = 0f;

        while(async.isDone != true)
        {
            yield return null;
            timer += Time.deltaTime;

            if(async.progress >= 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1, timer);
                if(loadingBar.fillAmount >= 1)
                {
                    async.allowSceneActivation = true;
                }
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, async.progress, timer);
                if(loadingBar.fillAmount >= async.progress)
                {
                    timer = 0;
                }
            }
        }
    }

}
