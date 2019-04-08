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
        //게임 일시정지
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "MainMenu")
        {
            GameMenu.SetActive(true);
        }
    }
    public void MenuExit()
    {
        GameMenu.SetActive(false);
        //일시정지 해제
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
        Debug.Log("stages: " + stages[currentStage]);
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

    public string GetStageName(int index) { return stages[index].name; }
    public bool GameMenuActive() { return GameMenu.activeInHierarchy; }
}
