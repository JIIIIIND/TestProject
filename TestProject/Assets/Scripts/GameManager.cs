using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //각종 옵션 값을 저장하고 관리
    //씬 관리 등등 전반적인 게임의 운영 관련한 것들 모음
    public static GameManager instance;
    private bool isSave = false;

	[SerializeField] private GameObject LaserBeam;

    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private GameObject fadeObject;
    private UnityEngine.UI.Image fadeImage;
    [SerializeField] private float fadePlayTime;

    private bool fadeIsPlaying;
    private float fadeStartValue = 0.0f;
    private float fadeEndValue = 1.0f;
    private float fadeTime = 0.0f;
    //
    private int storyModeCurrentStage;
    //startStage 이후의 Index에 대해서 실행 할 수 없도록
    private int startStage;

    [SerializeField] private SoundEffectManager soundEffectManager;
    [SerializeField] private UIController uiController;
    
    private GameManager()
    {}
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(transform.gameObject);

        fadeImage = fadeObject.GetComponent<UnityEngine.UI.Image>();
        fadeCanvas.SetActive(false);
    }
    void Start () {
        
	}
	public void Exit()
    {
        StartCoroutine("SaveAndExit");
        Debug.Log("Exit");
    }
    public void Save()
    {
        //플레이어 진행 상황 저장
        //기타 뭐 필요한 것들 저장
        //시간 좀 흐름
        Debug.Log("Save is complete");
        isSave = true;
    }

    IEnumerator SaveAndExit()
    {
        Save();
        yield return new WaitUntil(() => isSave);
        Application.Quit();

        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void SelectScene(GameObject stage)
    {
        int selectStage = ((int)stage.GetComponent<RectTransform>().localPosition.x * -1)/500;

        LoadScene(uiController.GetStageName(selectStage));
    }

    public void LoadScene(string name)
    {
        if (fadeIsPlaying == true)
            return;
        else
        {
            fadeCanvas.SetActive(true);
        }
        StartCoroutine(FadeOut(name, true));
        
    }
    public void FadeOutStart()
    {
        if (fadeIsPlaying == true)
            return;
        else
            fadeCanvas.SetActive(true);
        StartCoroutine(FadeOut("", false));
    }
    public void FadeInStart()
    {
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeOut(string name, bool isLoad)
    {
        fadeIsPlaying = true;
        Color color = fadeImage.color;
        fadeTime = 0;
        color.a = Mathf.Lerp(fadeStartValue, fadeEndValue, fadeTime);

        while(color.a < 1f)
        {
            fadeTime += Time.deltaTime / fadePlayTime;
            color.a = Mathf.Lerp(fadeStartValue, fadeEndValue, fadeTime);
            fadeImage.color = color;

            yield return null;
        }
        fadeIsPlaying = false;
        if(isLoad)
        {
            uiController.MenuExit();
            UnityEngine.SceneManagement.SceneManager.LoadScene(name);
            StartCoroutine(FadeIn());

        }
    }

    IEnumerator FadeIn()
    {
        fadeIsPlaying = true;
        Color color = fadeImage.color;
        fadeTime = 0;
        color.a = Mathf.Lerp(fadeEndValue, fadeStartValue, fadeTime);

        while(color.a > 0.0f)
        {
            Debug.Log("Fade iN");
            fadeTime += Time.deltaTime / fadePlayTime;
            color.a = Mathf.Lerp(fadeEndValue, fadeStartValue, fadeTime);
            fadeImage.color = color;

            yield return null;
        }
        fadeIsPlaying = false;
        fadeCanvas.SetActive(false);
    }

    public SoundEffectManager SoundEffectManager() { return soundEffectManager; }
    public UIController UIController() { return uiController; }
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC Key Down");
            if (uiController.GameMenuActive() == false)
                uiController.MenuAppear();
            else
                uiController.MenuExit();
        }
	}
}
