using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject GameMenu;
    [SerializeField] private GameObject StoryUI;
    [SerializeField] private GameObject TreatmentUI;

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
        if (currentStage < stages.Length)
        {
            currentStage++;
        }
        else
        {
            Debug.Log("this is last stage");
        }
        UpdateStageImage();
    }

    public void PreviousStage()
    {
        if(currentStage > 0)
        {
            currentStage--;
        }
        else
        {
            Debug.Log("this is first stage");
        }
        UpdateStageImage();
    }

    private void UpdateStageImage()
    {
        float imageSize = stages[currentStage].GetComponent<UnityEngine.UI.Image>().rectTransform.rect.width;
        Vector3 parentTransform = stages[currentStage].GetComponentInParent<RectTransform>().localPosition;

        parentTransform.Set(parentTransform.x - imageSize * currentStage, parentTransform.y, parentTransform.z);
        stages[currentStage].GetComponentInParent<RectTransform>().localPosition = parentTransform;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
