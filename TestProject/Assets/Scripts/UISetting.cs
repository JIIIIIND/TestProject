using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISetting : MonoBehaviour {

    public bool isButton;
    public string eventName;

    [SerializeField] GameObject eventParameterObject;
    private GameObject manager;

	void Start ()
    {
        manager = GameObject.Find("GameManager");
        GameManager.instance.UIController().SetUI(gameObject);
        if(isButton)
        {
            SetEventButton(eventName);
        }
	}
	
	private void SetEventButton(string name)
    {
        UnityEngine.UI.Button button = null;
        if(gameObject.GetComponent<UnityEngine.UI.Button>() != null)
        {
            button = gameObject.GetComponent<UnityEngine.UI.Button>();
        }
        if(button != null)
        {
            switch (name)
            {
                case "SelectScene":
                    {
                        button.onClick.AddListener(() => GameManager.instance.SelectScene(eventParameterObject));
                        break;
                    }
                case "NextStage":
                    {
                        button.onClick.AddListener(() => GameManager.instance.UIController().NextStage());
                        break;
                    }
                case "PreviousStage":
                    {
                        button.onClick.AddListener(() => GameManager.instance.UIController().PreviousStage());
                        break;
                    }
                case "BackButton":
                    {
                        button.onClick.AddListener(() => GameManager.instance.UIController().PushBackButton(eventParameterObject));
                        break;
                    }
                case "NewGameButton":
                    {
                        //게임시작 버튼 매핑
                        break;
                    }
                case "ContinueGameButton":
                    {
                        //이어하기 버튼 매핑
                        break;
                    }
                case "Exit":
                    {
                        button.onClick.AddListener(() => GameManager.instance.Exit());
                        break;
                    }
                case "TreatmentButton":
                    {
                        button.onClick.AddListener(() => GameManager.instance.UIController().PushUIButton(eventParameterObject));
                        break;
                    }
                case "StoryButton":
                    {
                        button.onClick.AddListener(() => GameManager.instance.UIController().PushUIButton(eventParameterObject));
                        break;
                    }
            }
        }
    }

	void Update ()
    {
		
	}
}
