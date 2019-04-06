using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ViveUIController : MonoBehaviour {

    private SteamVR_TrackedObject trackedObject;
    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 hitPoint;
    
    private SteamVR_Controller.Device Controller
    { get { return SteamVR_Controller.Input((int)(trackedObject.index)); } }

    private void Awake()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    private void ShowLaser(RaycastHit hit)
    {
        laser.SetActive(true);
        laserTransform.position = Vector3.Lerp(trackedObject.transform.position, hitPoint, 0.5f);
        laserTransform.LookAt(hitPoint);

        laserTransform.localScale = new Vector3(laserTransform.localScale.x,
            laserTransform.localScale.y, hit.distance);
    }
    void Start () {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(Controller.GetPress(SteamVR_Controller.ButtonMask.Trigger))
        {
            RaycastHit hit;
            
            if(Physics.Raycast(trackedObject.transform.position, transform.forward, out hit, 100))
            {
                hitPoint = hit.point;
                ShowLaser(hit);
            }
        }
        else
        {
            laser.SetActive(false);
        }
	}
}
