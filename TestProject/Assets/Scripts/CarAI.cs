using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour {

    public float rayRange;
    public float shootTime;
    public float speedValue;
    public float rotationValue;

    private float curTime;

    private RaycastHit raycastHit;

    [SerializeField]
    private List<GameObject> rayPosition;

	void Start () {
		
	}

    private void ShootRay()
    {
        foreach(GameObject rayPos in rayPosition)
        if (Physics.Raycast(rayPos.transform.position, rayPos.transform.TransformDirection(Vector3.forward), out raycastHit, rayRange))
        {
            Debug.DrawRay(rayPos.transform.position, rayPos.transform.TransformDirection(Vector3.forward) * raycastHit.distance, Color.yellow);
            Debug.Log(raycastHit.collider + " :" + raycastHit.distance);
            RotationBody(raycastHit, speedValue);
            //SpeedDown(speedValue);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * rayRange, Color.blue);
            SpeedUP(speedValue);
        }
    }
    
    private void RotationBody(RaycastHit hit, float value)
    {
        Vector3 reflect = Vector3.Reflect(transform.TransformDirection(Vector3.forward), hit.normal);
        Debug.DrawRay(hit.transform.position, reflect, Color.red);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(reflect), rotationValue * Time.deltaTime);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.FromToRotation(transform.TransformDirection(Vector3.forward), reflect), rotationValue);
    }
    
    private void SpeedUP(float value)
    {
        transform.position += (transform.TransformDirection(Vector3.forward) * value) * Time.deltaTime;

    }

    private void SpeedDown(float value)
    {
        transform.position += (transform.TransformDirection(Vector3.forward) * -value) * Time.deltaTime;
    }

	void Update ()
    {
        curTime += Time.deltaTime;
        if (shootTime < curTime)
        {
            ShootRay();
            curTime = 0;
        }
        
	}
}
