using System.Collections;
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
	[SerializeField] private GameObject leftFrontWheel;
	[SerializeField] private GameObject rightFrontWheel;

	[SerializeField] private WheelCollider leftFrontWheelCollider;
	[SerializeField] private WheelCollider rightFrontWheelCollider;
	[SerializeField] private WheelCollider leftWheelCollider;
	[SerializeField] private WheelCollider rightWheelCollider;

    private IEnumerator wheelRotate;


    //메인이 되는 함수의 회전/속도 관련 값에 접근하여 값에 따라 바퀴를 회전시킴
    //속도나 회전 값이 0이라면 바퀴는 회전하지 않고 멈춰있는 상태 유지

    void Start() {

	}

	public void MotorTorque(Wheel wheel, Vector3 direction)
	{
        float speed = 0;
        if(direction.magnitude >= 1)
        {
            direction.Normalize();
        }
		Debug.Log("direction X value: "+direction.x+"direction Z value: "+direction.z);
		speed = direction.z / direction.magnitude;
        if(speed > 1.0f)
        {
            Debug.Log("speedValue is over 1.0f");
			speed = 1.0f;
        }
		StartCoroutine(SetMotorTorque(wheel, direction));
		/*
		if (wheel == Wheel.LEFT)
		{
			leftWheelCollider.motorTorque = speed * maxMotorTorque;
		}
		else
		{
			rightWheelCollider.motorTorque = speed * maxMotorTorque;
		}
		*/
	}

	public void SteeringWheel(Vector3 direction)
	{
        direction.Normalize();
		float angle = 0;
		angle = (direction.x / direction.magnitude) * maxSteeringAngle;
		Debug.Log("direction X value: " + direction.x + "direction Z value: " + direction.z);
		//Debug.Log("steering: "+(direction.x / direction.magnitude) * maxSteeringAngle);
		if (leftWheelCollider.steerAngle > maxSteeringAngle)
		{
			leftWheelCollider.steerAngle = angle;
			rightWheelCollider.steerAngle = angle;
		}
		
		//leftWheelCollider.steerAngle = angle;
		//rightWheelCollider.steerAngle = angle;
	}
	
	public void InitSteeringAngle()
	{
		leftWheelCollider.steerAngle = 0;
		rightWheelCollider.steerAngle = 0;
	}
	
    IEnumerator SetMotorTorque(Wheel wheel, Vector3 direction)
    {
        float curTime = Time.deltaTime;
        float curSpeed = 0;

        while(curTime < 1)
        {
            Vector3 currenDirection = Vector3.Lerp(player.transform.forward, direction, curTime);
            curSpeed = (currenDirection.z / currenDirection.magnitude) * maxMotorTorque;
            if (wheel == Wheel.LEFT)
            {
                leftWheelCollider.motorTorque = curSpeed;
            }
            else
            {
                rightWheelCollider.motorTorque = curSpeed;
            }
            curTime += Time.deltaTime * 2;
            yield return null;
        }
		Debug.Log("MotorTorque End");
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
		leftWheel.transform.Rotate(0.0f, -leftWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f);
		rightWheel.transform.Rotate(0.0f, rightWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f);
		leftFrontWheel.transform.Rotate(0.0f, -leftWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f);
		rightFrontWheel.transform.Rotate(0.0f, rightWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f);

		float speed = Input.GetAxis("Vertical");
		float angle = Input.GetAxis("Horizontal");

		Debug.Log("speed: " + speed + "angle : " + angle);

		leftWheelCollider.motorTorque = speed * maxMotorTorque;
		rightWheelCollider.motorTorque = speed * maxMotorTorque;
		leftFrontWheelCollider.steerAngle = angle * maxSteeringAngle;
		rightFrontWheelCollider.steerAngle = angle * maxSteeringAngle;
		
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

	public void SetLeftWheelMotorTorque(float value) { leftWheelCollider.motorTorque = value; }
	public void SetRightWheelMotorTorque(float value) { rightWheelCollider.motorTorque = value; }

	public void SetLeftWheelSteering(float value) { leftFrontWheelCollider.steerAngle = value; }
	public void SetRightWheelSteering(float value) { rightFrontWheelCollider.steerAngle = value; }
}
