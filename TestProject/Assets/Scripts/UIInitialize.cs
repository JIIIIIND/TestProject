using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInitialize : MonoBehaviour {

    public Vector3 initializeVector;
    [SerializeField] private GameObject player;
    
	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 direction = this.transform.position - player.transform.position;
        //this.transform.LookAt(player.transform);
        Quaternion angle = Quaternion.LookRotation(direction.normalized);
        this.transform.rotation = angle;

        Vector3 forwardVector = player.transform.TransformDirection(Vector3.forward);

        this.transform.position = forwardVector * initializeVector.normalized.magnitude;
        this.transform.position.Set(this.transform.position.x, this.transform.position.y + 7, this.transform.position.z);
	}
}
