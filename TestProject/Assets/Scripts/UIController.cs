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
        currentStage = 0;
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
    public void UpdateImage()
    {
        float imageSize = stages[currentStage].GetComponent<UnityEngine.UI.Image>().rectTransform.rect.width;
        Vector3 parentTransform = Stages.GetComponent<RectTransform>().localPosition;

        parentTransform.Set(parentTransform.x - imageSize * currentStage, parentTransform.y, parentTransform.z);
        Stages.GetComponent<RectTransform>().localPosition = parentTransform;
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

    public void SetUI(GameObject gameObject)
    {
        string uiName = gameObject.name;
        switch(uiName)
        {
            case "Main":
                {
                    MainUI = gameObject;
                    break;
                }
            case "StoryMenu":
                {
                    StoryUI = gameObject;
                    StoryUI.SetActive(false);
                    break;
                }
            case "TreatmentMenu":
                {
                    TreatmentUI = gameObject;
                    TreatmentUI.SetActive(false);
                    break;
                }
            case "Stages":
                {
                    Stages = gameObject;
                    break;
                }
            case "Stage1":
                {
                    stages[0] = gameObject;
                    break;
                }
            case "Stage2":
                {
                    stages[1] = gameObject;
                    break;
                }
            case "Stage3":
                {
                    stages[2] = gameObject;
                    break;
                }
            case "Stage4":
                {
                    stages[3] = gameObject;
                    break;
                }
            case "Stage5":
                {
                    stages[4] = gameObject;
                    break;
                }
            default:
                break;
        }
        
    }

    public void CurrentUIInit()
    {
        StoryUI.SetActive(false);
        TreatmentUI.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
		
	}

    public string GetStageName(int index) { return stages[index].name; }
    public bool GameMenuActive() { return GameMenu.activeInHierarchy; }
}
