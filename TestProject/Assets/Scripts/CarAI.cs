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
    private GameObject rayPosition;

	void Start () {
		
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Physics.BoxCast(rayPosition.transform.position, transform.lossyScale / 2, transform.forward, out raycastHit, Quaternion.identity, rayRange))
        {
            Gizmos.DrawRay(transform.position, transform.forward * raycastHit.distance);
            Gizmos.DrawWireCube(transform.position + transform.forward * raycastHit.distance, transform.lossyScale);
        }
        else
            Gizmos.DrawRay(transform.position, transform.forward * rayRange);
    }
    private void ShootRay()
    {
        
        //if (Physics.Raycast(rayPosition.transform.position, rayPosition.transform.TransformDirection(Vector3.forward), out raycastHit, rayRange))
        if (Physics.BoxCast(rayPosition.transform.position, transform.lossyScale/2, transform.forward, out raycastHit, Quaternion.identity, rayRange))
        {
            Debug.DrawRay(rayPosition.transform.position, transform.forward * raycastHit.distance, Color.yellow);
            Debug.Log(raycastHit.collider + " :" + raycastHit.distance);
            RotationBody(raycastHit, speedValue);
            //SpeedDown(speedValue);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * rayRange, Color.blue);
            SpeedUP(speedValue);
        }
    }
    
    private void RotationBody(RaycastHit hit, float value)
    {
        Vector3 reflect = Vector3.Reflect(transform.TransformDirection(Vector3.forward), hit.normal);
        Debug.DrawRay(hit.transform.position, reflect, Color.red);
        SpeedUP(value / 2);
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
