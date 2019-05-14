using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {
    
	[SerializeField] private GameObject player;
    [SerializeField] private GameObject sceneCanvas;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject manualPanel;

    [SerializeField] private Vector3 startPoint;
    [SerializeField] private GameObject[] checkPoint;
    [SerializeField] private GameObject endPoint;
    private Vector3 lastPlayerPoint;
    private Quaternion setPlayerRotation;
    [SerializeField] private int currentCollisionCounter;
    private int saveCollisionCounter = 0;

    [SerializeField] private string nextSceneName;

    [SerializeField] private Vector3 playerScale;
    [SerializeField] private float modifiedMaxTorque;

    [SerializeField] private int collisionLimit;
    [SerializeField] private float timeLimit;
    private float currentTime;
     
	void Start ()
    {
        GameManager.instance.FadeOutStart();
        player.transform.localScale = playerScale;
        player.GetComponent<PlayerControl>().GetWheelControl().maxMotorTorque = modifiedMaxTorque;
        //기타 플레이어 설정 건드릴껀 여기서
        GameManager.instance.FadeInStart();
        lastPlayerPoint = player.transform.position;
        player.GetComponent<PlayerControl>().SetLastRoadPosition(startPoint);
        CanvasOn(manualPanel);
	}
	
    private void CanvasOn(GameObject item)
    {
        sceneCanvas.SetActive(true);
        item.SetActive(true);
        StartCoroutine(CanvasItemAppearTime(5f, item));
    }
    IEnumerator CanvasItemAppearTime(float time, GameObject item)
    {
        yield return new WaitForSeconds(time);
        CanvasOff(item);
    }
    private void CanvasOff(GameObject item)
    {
        item.SetActive(false);
        sceneCanvas.SetActive(false);
    }

    public void CollisionDetect()
    {
        currentCollisionCounter++;
        if(currentCollisionCounter >= collisionLimit)
        {
            ReturnToCheckPoint();
        }
    }
    
    private void ReturnToCheckPoint()
    {
        GameManager.instance.FadeOutStart();
        CanvasOn(gameOverText);
        player.transform.position = lastPlayerPoint;
        player.transform.rotation = setPlayerRotation;
        currentCollisionCounter = saveCollisionCounter;
        GameManager.instance.FadeInStart();
    }

    public void CheckPointEntry(GameObject checkpoint, Vector3 playerPosition, Quaternion playerRotation)
    {
        lastPlayerPoint = playerPosition;
        setPlayerRotation = playerRotation;
        saveCollisionCounter = currentCollisionCounter;
    }

    

    public void EndPointEntry()
    {
        Debug.Log("EndPointEntry");
        //클리어 소리 출력 이후 LoadScene 되도록 수정 필요
        GameManager.instance.LoadScene(nextSceneName);
    }
    // Update is called once per frame
    void Update ()
    {
        currentTime += Time.deltaTime;

	}
}
