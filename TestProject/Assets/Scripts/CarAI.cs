using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour {

    public float rayRange;
    public float shootTime;
    public float speedValue;
    public float rotationValue;
    public float angle;
    public float speedRate;

    private float curTime;

    private RaycastHit raycastHit;

    [SerializeField]
    private float speedMax;
    [SerializeField]
    private float speedMin;
    [SerializeField]
    private GameObject rayPosition;

	void Start () {
		
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 vector = transform.forward;
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
        if (Physics.BoxCast(rayPosition.transform.position, transform.lossyScale/2, transform.forward, out raycastHit, Quaternion.identity, rayRange))
        {
            SpeedDown(speedRate);
            Debug.DrawRay(rayPosition.transform.position, transform.forward * raycastHit.distance, Color.yellow);
            Debug.Log(raycastHit.collider + " :" + raycastHit.distance);
            RotationBody(raycastHit, speedValue);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward * rayRange, Color.blue);
            SpeedUP(speedValue, speedRate);
            if (Physics.Raycast(transform.forward * raycastHit.distance, transform.right, out raycastHit, rayRange * 1.5f))
            {
                RotationBody(raycastHit, speedValue, Quaternion.Euler(0, -angle, 0) * transform.forward);
            }
            else if (Physics.Raycast(transform.forward * raycastHit.distance, -transform.right, out raycastHit, rayRange * 1.5f))
            {
                RotationBody(raycastHit, speedValue, Quaternion.Euler(0, angle, 0) * transform.forward);
            }
        }
    }
    
    private void RotationBody(RaycastHit hit, float value)
    {
        Vector3 reflect = Vector3.Reflect(transform.TransformDirection(Vector3.forward), hit.normal);
        Debug.DrawRay(hit.transform.position, reflect, Color.red);
        SpeedUP(value / 2);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(reflect), rotationValue * Time.deltaTime);
    }

    private void RotationBody(RaycastHit hit, float value, Vector3 direction)
    {
        Vector3 reflect = Vector3.Reflect(transform.TransformDirection(direction), hit.normal);
        Debug.DrawRay(hit.transform.position, reflect, Color.red);
        SpeedUP(value / 2);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(reflect), rotationValue * Time.deltaTime);
    }

    private void SpeedUP(float value)
    {
        transform.position += (transform.TransformDirection(Vector3.forward) * value) * Time.deltaTime;
    }

    private void SpeedUP(float value, float rate)
    {
        transform.position += (transform.TransformDirection(Vector3.forward) * value) * Time.deltaTime;
        if(speedValue < speedMax)
            speedValue *= (1 + rate);
    }

    private void SpeedDown(float rate)
    {
        if(speedValue > speedMin)
            speedValue *= (1 - rate);
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
