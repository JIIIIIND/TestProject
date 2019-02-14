using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public float speedRate;

    private float speedValue;
    private float currentTime;

    [SerializeField] private float rotationAngle;
    [SerializeField] private float cognitionTime;
    [SerializeField] private float maxSpeed;


    void Start() {

    }

    private void RotationBody(float angle)
    {
        Vector3 rotation;
        if (angle == 1)
            rotation = transform.right;
        else
            rotation = -transform.right;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(rotation), rotationAngle * Time.deltaTime);
    }

    private void SpeedUp()
    {
        if (speedValue <= 0)
        {
            speedValue += (1 + speedRate);
        }
        if (maxSpeed > speedValue)
        {
            speedValue *= (1 + speedRate);
        }
    }

    private void SpeedDown()
    {
        
    }

	void Update ()
    {
        currentTime += Time.deltaTime;
        if (currentTime > cognitionTime)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                SpeedUp();
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                SpeedDown();
            }
            
            currentTime = 0;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            RotationBody(1);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            RotationBody(-1);
        }
        transform.position += (transform.TransformDirection(Vector3.forward) * speedValue) * Time.deltaTime;
    }
}
