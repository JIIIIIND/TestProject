using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject GameMenu;
    [SerializeField] private GameObject StoryUI;
    [SerializeField] private GameObject TreatmentUI;

    void Start () {
		
	}
	
    public void PushBackButton(GameObject gameObject)
    {
        gameObject.SetActive(false);
        MainUI.SetActive(true);
    }

    public void PushUIButton(GameObject gameObject)
    {
        gameObject.SetActive(true);
        MainUI.SetActive(false);
    }

    public void MenuAppear()
    {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu")
        {
            GameMenu.SetActive(true);
        }
    }
    public void MenuExit(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
