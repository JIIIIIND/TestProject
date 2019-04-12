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
    private Quaternion lastPlayerRotation;
    private int currentCollisionCounter;
    private int saveCollisionCounter = 0;

    [SerializeField] private string nextSceneName;

    [SerializeField] private Vector3 playerScale;
    [SerializeField] private float modifiedMaxTorque;

    [SerializeField] private int collisionLimit;
    [SerializeField] private float timeLimit;
    
	void Start ()
    {
        player = Instantiate(player, startPoint, Quaternion.identity);
        player.transform.localScale = playerScale;
        player.GetComponent<PlayerControl>().GetWheelControl().maxMotorTorque = modifiedMaxTorque;
        //기타 플레이어 설정 건드릴껀 여기서

        lastPlayerPoint = player.transform.position;
        lastPlayerRotation = player.transform.rotation;
	}
	
    private void CanvasOn(GameObject item)
    {
        sceneCanvas.SetActive(true);
        item.SetActive(true);
        CanvasItemAppearTime(10f, item);
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
            //게임실패 함수 호출
        }
    }
    
    private void ReturnToCheckPoint()
    {
        //페이드아웃
        CanvasOn(gameOverText);
        player.transform.position = lastPlayerPoint;
        player.transform.rotation = lastPlayerRotation;
        currentCollisionCounter = saveCollisionCounter;
        //페이드인
    }

    public void CheckPointEntry(GameObject checkpoint, Transform playerTransform)
    {
        lastPlayerPoint = playerTransform.position;
        lastPlayerRotation = playerTransform.rotation;
        saveCollisionCounter = currentCollisionCounter;
    }

    public void EndPointEntry()
    {
        GameManager.instance.LoadScene(nextSceneName);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
