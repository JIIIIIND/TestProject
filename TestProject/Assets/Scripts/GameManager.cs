using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //각종 옵션 값을 저장하고 관리
    //씬 관리 등등 전반적인 게임의 운영 관련한 것들 모음
    public static GameManager instance;
    private bool isSave = false;
	
    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private GameObject fadeObject;
    private UnityEngine.UI.Image fadeImage;
    [SerializeField] private float fadePlayTime;

    private bool fadeIsPlaying;
    private float fadeStartValue = 0.0f;
    private float fadeEndValue = 1.0f;
    private float fadeTime = 0.0f;
    
    [SerializeField] private SoundEffectManager soundEffectManager;
    [SerializeField] private GameObject gameMenu;
    [SerializeField] private AudioSource uiClick;
    
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
    public void LoadMainMenu()
    {
        uiClick.Play();
        LoadScene("MainMenu");
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
            MenuExit();

            LoadManager.LoadScene(name);
            StartCoroutine(FadeIn());

        }
    }
	public void MenuAppear()
	{
        uiClick.Play();
		if (SceneManager.GetActiveScene().name != "MainMenu")
		{
			gameMenu.SetActive(true);
		}
	}
	public void MenuExit()
	{
        uiClick.Play();
		gameMenu.SetActive(false);
	}
	IEnumerator FadeIn()
    {
        fadeIsPlaying = true;
        Color color = fadeImage.color;
        fadeTime = 0;
        color.a = Mathf.Lerp(fadeEndValue, fadeStartValue, fadeTime);

        while(color.a > 0.0f)
        {
            ////Debug.Log("Fade iN");
            fadeTime += Time.deltaTime / fadePlayTime;
            color.a = Mathf.Lerp(fadeEndValue, fadeStartValue, fadeTime);
            fadeImage.color = color;

            yield return null;
        }
        fadeIsPlaying = false;
        fadeCanvas.SetActive(false);
    }

    public SoundEffectManager SoundEffectManager() { return soundEffectManager; }
	public bool GameMenuActive() { return gameMenu.activeInHierarchy; }

	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            ////Debug.Log("ESC Key Down");
            if (gameMenu.activeInHierarchy == false)
                MenuAppear();
            else
                MenuExit();
        }
	}
}
