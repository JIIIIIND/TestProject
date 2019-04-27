using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInitialize : MonoBehaviour {

    public Vector3 initializeVector;
    private Vector3 playerInitPos;
    [SerializeField] private GameObject player = null;
    
	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
            playerInitPos = player.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(player != null)
        {
            Vector3 direction = this.transform.position - player.transform.position;
            //this.transform.LookAt(player.transform);
            direction.y = 0;
            
            Quaternion angle = Quaternion.LookRotation(direction.normalized);
            this.transform.rotation = angle;

            Vector3 difference = player.transform.position - playerInitPos;

            GetComponent<RectTransform>().transform.position = player.transform.TransformVector(initializeVector);
            GetComponent<RectTransform>().transform.position += difference;
        }
    }
}
