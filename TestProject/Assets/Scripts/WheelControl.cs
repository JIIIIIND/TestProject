using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Wheel { LEFT, RIGHT }
public class WheelControl : MonoBehaviour {
	
	private float rotationAngle;

	[SerializeField] private float brakeRate;

    [SerializeField] private GameObject leftWheel;
    [SerializeField] private GameObject rightWheel;

	[SerializeField] private WheelCollider leftWheelCollider;
	[SerializeField] private WheelCollider rightWheelCollider;

    private IEnumerator leftWheelRotate;
    private IEnumerator rightWheelRotate;

    //메인이 되는 함수의 회전/속도 관련 값에 접근하여 값에 따라 바퀴를 회전시킴
    //속도나 회전 값이 0이라면 바퀴는 회전하지 않고 멈춰있는 상태 유지

    void Start() {

	}

	public void RotationWheel(Wheel wheel, float speed)
	{
        if(wheel == Wheel.LEFT)
        {
            if (leftWheelRotate != null)
                StopCoroutine(leftWheelRotate);
            leftWheelRotate = RotateWheel(leftWheel, speed * 100);

            StartCoroutine(leftWheelRotate);
        }
        else
        {
            if (rightWheelRotate != null)
                StopCoroutine(rightWheelRotate);
            rightWheelRotate = RotateWheel(rightWheel, speed * 100);

            StartCoroutine(rightWheelRotate);
        }
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

		wheelCollider.brakeTorque = brakeRate;
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
	void Update () {
		
	}

    public bool LeftWheelIsGround()
    { return leftWheelCollider.isGrounded; }
    public bool RightWheelIsGround()
    { return rightWheelCollider.isGrounded; }

}
