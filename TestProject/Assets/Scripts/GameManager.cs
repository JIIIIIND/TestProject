using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //각종 옵션 값을 저장하고 관리
    //씬 관리 등등 전반적인 게임의 운영 관련한 것들 모음
    private bool isSave = false;
    private float count = 0;

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

    public void LoadScene(string name)
    {
        //화면 전환용 효과 좀 넣어주고(Fade Out)
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
        //여기서도 효과 좀 넣어주고(Fade In)
    }
	void Update () {
		
	}
}
