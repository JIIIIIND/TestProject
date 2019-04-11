using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

    [SerializeField] private GameObject sceneCanvas;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject manualPanel;
    [SerializeField] private Vector3 startPoint;
    [SerializeField] private GameObject[] checkPoint;
    [SerializeField] private GameObject endPoint;
    [SerializeField] private string nextSceneName;
    [SerializeField] private Vector3 playerScale;
    [SerializeField] private int collisionCounter;
    [SerializeField] private float timeLimit;
    private Transform lastYourPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private float modifiedMaxTorque;
    private int currentCollisionCounter;

	void Start ()
    {
        player = Instantiate(player, startPoint, Quaternion.identity);
        player.transform.localScale = playerScale;
        player.GetComponent<PlayerControl>().GetWheelControl().maxMotorTorque = modifiedMaxTorque;
        //기타 플레이어 설정 건드릴껀 여기서
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

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            currentCollisionCounter++;
            if(currentCollisionCounter >= collisionCounter)
            {

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {

        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
