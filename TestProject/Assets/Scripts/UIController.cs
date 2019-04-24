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

    void Start () {
        currentStage = 0;
		SetClearStageImage();
	}

	public void Exit()
	{
		StartCoroutine("SaveAndExit");
		Debug.Log("Exit");
	}
	public void PushBackButton(GameObject gameObject)
    {
        gameObject.SetActive(false);
        MainUI.SetActive(true);
    }

    public void PushUIButton(GameObject gameObject)
    {
        currentStage = 0;
		Debug.Log("ui button is pushing: "+gameObject);
        gameObject.SetActive(true);
        MainUI.SetActive(false);
    }

	public void SelectScene(GameObject stage)
	{
		int selectStage = ((int)stage.GetComponent<RectTransform>().localPosition.x * -1) / 500;

		GameManager.instance.LoadScene(GetStageName(selectStage));
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
		SetClearStageImage();
        Debug.Log("stages: " + stages[currentStage]);
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
	// Update is called once per frame
	public void SetClearStageImage()
	{
		int lastClearStage = 0;
		for(int i = 0; i <= stages.Length; i++)
		{
			if(PlayerPrefs.HasKey("ClearStage"+(i+1)))
			{
				lastClearStage = i;
			}
		}
		Debug.Log("save Stage: " + lastClearStage);
	}

	void Update () {
		
	}

    public string GetStageName(int index) { return stages[index].name; }
}
