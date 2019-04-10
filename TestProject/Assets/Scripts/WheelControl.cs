﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Wheel { LEFT, RIGHT }
public class WheelControl : MonoBehaviour {
	
	private float rotationAngle;

	public float maxMotorTorque;
	public float maxSteeringAngle;

	[SerializeField] private GameObject player;

	[SerializeField] private float brakeValue;

    [SerializeField] private GameObject leftWheel;
    [SerializeField] private GameObject rightWheel;

	[SerializeField] private WheelCollider leftFrontWheelCollider;
	[SerializeField] private WheelCollider rightFrontWheelCollider;
	[SerializeField] private WheelCollider leftWheelCollider;
	[SerializeField] private WheelCollider rightWheelCollider;

    private IEnumerator wheelRotate;


    //메인이 되는 함수의 회전/속도 관련 값에 접근하여 값에 따라 바퀴를 회전시킴
    //속도나 회전 값이 0이라면 바퀴는 회전하지 않고 멈춰있는 상태 유지

    void Start() {

	}

	public void MotorTorque(Wheel wheel, float speed)
	{
		if (speed > maxMotorTorque)
			speed = maxMotorTorque;
		if(wheel == Wheel.LEFT)
		{
			leftWheelCollider.motorTorque = speed;
		}
		else
		{
			rightWheelCollider.motorTorque = speed;
		}
	}

	public void SteeringWheel(Vector3 direction)
	{
        direction.Normalize();
		float angle = 0;
		angle = (direction.x / direction.magnitude) * maxSteeringAngle;
		Debug.Log(direction.x / direction.magnitude);
		leftWheelCollider.steerAngle = angle;
		rightWheelCollider.steerAngle = angle;
		//leftWheelCollider.steerAngle = angle;
		//rightWheelCollider.steerAngle = angle;
	}
	
	public void InitSteeringAngle()
	{
		leftWheelCollider.steerAngle = 0;
		rightWheelCollider.steerAngle = 0;
	}

    IEnumerator RotateWheel(GameObject wheel, float rotateSpeed)
    {
        float angle = 0.0f;
        
        while(rotateSpeed != 0)
        {
            angle += Mathf.Cos(Mathf.PI / 10) * Time.deltaTime;
			//rotateSpeed *= Mathf.Sin(angle);
			if (angle > (Mathf.PI / 2))
			{
				angle = Mathf.PI / 2;
				rotateSpeed = 0;
			}
			
            wheel.transform.rotation *= Quaternion.Euler(new Vector3(0, rotateSpeed * Mathf.Cos(angle), 0));
            yield return null;
        }
    }
	
	private void brakeEffectPlay()
	{

	}

	public void BrakeWheel(Wheel wheel)
	{
		WheelCollider wheelCollider;

		if (wheel == Wheel.LEFT)
			wheelCollider = leftWheelCollider;
		else
			wheelCollider = rightWheelCollider;

		wheelCollider.brakeTorque = brakeValue;
	}

	public void InitBrakeTorque(Wheel wheel)
	{
		WheelCollider wheelCollider;

		if (wheel == Wheel.LEFT)
			wheelCollider = leftWheelCollider;
		else
			wheelCollider = rightWheelCollider;

		wheelCollider.brakeTorque = 0.0f;
	}
	void Update ()
	{
		leftWheel.transform.Rotate(0.0f, leftWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f);
		rightWheel.transform.Rotate(0.0f, rightWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f);

		float speed = Input.GetAxis("Vertical") * maxMotorTorque;
		float angle = Input.GetAxis("Horizontal") * maxSteeringAngle;
		/*
		leftWheelCollider.motorTorque = speed;
		rightWheelCollider.motorTorque = speed;
		leftWheelCollider.steerAngle = angle;
		rightWheelCollider.steerAngle = angle;
		*/
	}

    public bool LeftWheelIsGround()
    {
		Debug.Log("Left: "+leftWheelCollider.isGrounded);
		return leftWheelCollider.isGrounded;
		//return true;
	}
    public bool RightWheelIsGround()
    {
		Debug.Log("Right: " + rightWheelCollider.isGrounded);
		return rightWheelCollider.isGrounded;
		//return true;
	}

}
