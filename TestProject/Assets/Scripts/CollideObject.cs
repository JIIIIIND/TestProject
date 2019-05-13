using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ObjectType { WALL, CHECKPOINT, ENDPOINT }
public class CollideObject : MonoBehaviour {

    [SerializeField] private StageManager stageManager;
    [SerializeField] private Vector3 rotationVector;
    [SerializeField] private ObjectType type;
    private bool isActived = false;
    private Quaternion playerRotation;
    [SerializeField] private AudioSource collisionSound;

	void Start () {
        playerRotation = Quaternion.Euler(rotationVector.x, rotationVector.y, rotationVector.z);
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        if(isActived == false)
        {
            if ((other.gameObject.tag == "Player") || (other.gameObject.tag == "GameController"))
            {
                if (type == ObjectType.ENDPOINT)
                {
                    Debug.Log("endPoint");
                    stageManager.EndPointEntry();
					isActived = true;

				}

                else if(type == ObjectType.CHECKPOINT)
                {
                    Debug.Log("checkPoint");
                    stageManager.CheckPointEntry(this.transform.gameObject, this.gameObject.transform.position, playerRotation);
					isActived = true;
				}
                
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isActived == false)
        {
            if ((collision.gameObject.tag == "Player") || (collision.gameObject.tag == "GameController"))
            {
                //충돌 이펙트 띄워줌
                Debug.Log("Collision");
                stageManager.CollisionDetect();
                collisionSound.Play();
            }
        }
    }

    void Update () {
		
	}
}
