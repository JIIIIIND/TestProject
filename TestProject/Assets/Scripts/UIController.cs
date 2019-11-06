using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    [SerializeField] private GameObject MainUI;
    [SerializeField] private GameObject StoryUI;
    [SerializeField] private GameObject TreatmentUI;
    [SerializeField] private GameObject Stages;

    [SerializeField] private GameObject[] stages;
    
    private int currentStage;

    [SerializeField] private AudioSource uiClick;

    void Start () {
        currentStage = 0;
	}

	public void Exit()
	{
        Application.Quit();
		////Debug.Log("Exit");
	}
	public void PushBackButton(GameObject gameObject)
    {
        uiClick.Play();
        gameObject.SetActive(false);
        MainUI.SetActive(true);
    }

    public void PushUIButton(GameObject gameObject)
    {
        uiClick.Play();
        currentStage = 0;
		////Debug.Log("ui button is pushing: "+gameObject);
        gameObject.SetActive(true);
        MainUI.SetActive(false);
    }

    public void SelectScene(string stageName)
    {
        uiClick.Play();
        GameManager.instance.LoadScene(stageName);
    }

	public void SelectScene(GameObject stage)
	{
        uiClick.Play();
        int selectStage = ((int)stage.GetComponent<RectTransform>().localPosition.x * -1) / 500;

		GameManager.instance.LoadScene(GetStageName(selectStage));
	}
	
    public void NextStage()
    {
        uiClick.Play();
        if (currentStage < stages.Length-1)
        {
            currentStage++;

            UpdateStageImage(true);
        }
        else
        {
            ////Debug.Log("this is last stage");
        }
    }

    public void PreviousStage()
    {
        uiClick.Play();
        if (currentStage > 0)
        {
            currentStage--;

            UpdateStageImage(false);
        }
        else
        {
            ////Debug.Log("this is first stage");
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
        ////Debug.Log("stages: " + stages[currentStage]);
        float imageSize = stages[currentStage].GetComponent<UnityEngine.UI.Image>().rectTransform.rect.width;
        Vector3 parentTransform = Stages.GetComponent<RectTransform>().localPosition;

        if (isNext == false)
            imageSize *= -1;
        parentTransform.Set(parentTransform.x - imageSize, parentTransform.y, parentTransform.z);
        Stages.GetComponent<RectTransform>().localPosition = parentTransform;
    }
	
    public void CurrentUIInit()
    {
        StoryUI.SetActive(false);
        TreatmentUI.SetActive(false);
    }

	void Update () {
		
	}

    public string GetStageName(int index) { return stages[index].name; }
}
