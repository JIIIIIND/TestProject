using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject GameMenu;
    [SerializeField] private GameObject StoryUI;
    [SerializeField] private GameObject TreatmentUI;
    [SerializeField] private GameObject Stages;

    [SerializeField] private GameObject[] stages;
    
    private int currentStage;

    void Start () {
        currentStage = 0;
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

    public void NextStage()
    {
        if (currentStage < stages.Length-1)
        {
            currentStage++;

            UpdateStageImage(true);
        }
        else
        {
            Debug.Log("this is last stage");
        }
    }

    public void PreviousStage()
    {
        if(currentStage > 0)
        {
            currentStage--;

            UpdateStageImage(false);
        }
        else
        {
            Debug.Log("this is first stage");
        }
    }

    private void UpdateStageImage(bool isNext)
    {
        float imageSize = stages[currentStage].GetComponent<UnityEngine.UI.Image>().rectTransform.rect.width;
        Vector3 parentTransform = Stages.GetComponent<RectTransform>().localPosition;

        if (isNext == false)
            imageSize *= -1;
        parentTransform.Set(parentTransform.x - imageSize, parentTransform.y, parentTransform.z);
        Stages.GetComponent<RectTransform>().localPosition = parentTransform;
        
    }

	// Update is called once per frame
	void Update () {
		
	}
}
