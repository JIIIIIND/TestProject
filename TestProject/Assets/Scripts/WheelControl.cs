using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Wheel { LEFT, RIGHT }
public class WheelControl : MonoBehaviour {
	
	private float rotationAngle;

	[SerializeField] private float brakeRate;

	[SerializeField] private WheelCollider leftWheel;
	[SerializeField] private WheelCollider rightWheel;

	//메인이 되는 함수의 회전/속도 관련 값에 접근하여 값에 따라 바퀴를 회전시킴
	//속도나 회전 값이 0이라면 바퀴는 회전하지 않고 멈춰있는 상태 유지

	void Start() {

	}

	private void RotationWheel()
	{

	}

	private void effectPlay()
	{

	}

	private void groundEffectPlay()
	{

	}

	public void BrakeWheel(Wheel wheel)
	{
		WheelCollider wheelCollider;

		if (wheel == Wheel.LEFT)
			wheelCollider = leftWheel;
		else
			wheelCollider = rightWheel;

		wheelCollider.brakeTorque += brakeRate;
	}

	public void InitBrakeTorque(Wheel wheel)
	{
		WheelCollider wheelCollider;

		if (wheel == Wheel.LEFT)
			wheelCollider = leftWheel;
		else
			wheelCollider = rightWheel;

		wheelCollider.brakeTorque = 0.0f;
	}
	void Update () {
		
	}
}
