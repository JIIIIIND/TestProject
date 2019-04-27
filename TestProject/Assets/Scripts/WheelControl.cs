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

    [SerializeField] private ParticleSystem leftBrakeEffect;
    [SerializeField] private ParticleSystem rightBrakeEffect;
    [SerializeField] private ParticleSystem leftDustEffect;
    [SerializeField] private ParticleSystem rightDustEffect;


    //메인이 되는 함수의 회전/속도 관련 값에 접근하여 값에 따라 바퀴를 회전시킴
    //속도나 회전 값이 0이라면 바퀴는 회전하지 않고 멈춰있는 상태 유지

    void Start() {

	}
    
	private void BrakeEffectPlay(Wheel wheel)
	{
        //TRIGGER 눌릴때만 재생되도록
        BrakeEffectSetting(wheel);
        if (wheel == Wheel.LEFT)
            leftBrakeEffect.Play();
        else
            rightBrakeEffect.Play();
	}

    private void BrakeEffectPause(Wheel wheel)
    {
        //TRIGGER 눌릴때만 재생되도록
        
        if (wheel == Wheel.LEFT)
            leftBrakeEffect.Stop();
        else
            rightBrakeEffect.Stop();
    }

    public void BrakeEffectSetting(Wheel wheel)
    {
        if(wheel == Wheel.LEFT)
        {
            var emission = leftBrakeEffect.emission;
            leftBrakeEffect.Emit((int)leftWheelCollider.rpm / 10);
            //emission.burstCount = (int)leftWheelCollider.rpm/10;
        }
        else
        {
            var emission = rightBrakeEffect.emission;
            rightBrakeEffect.Emit((int)rightWheelCollider.rpm / 10);
            //emission.burstCount = (int)rightWheelCollider.rpm/10;
        }
    }

    private void DustEffectSetting(Wheel wheel)
    {
        if (wheel == Wheel.LEFT)
        {
            if(LeftWheelIsGround())
            {
                leftDustEffect.Emit((int)leftWheelCollider.rpm / 50);
            }
        }
        else
        {
            if (RightWheelIsGround())
            {
                rightDustEffect.Emit((int)rightWheelCollider.rpm / 50);
            }
        }
    }

	public void BrakeWheel(Wheel wheel)
	{
		WheelCollider wheelCollider;

		if (wheel == Wheel.LEFT)
        {
            wheelCollider = leftWheelCollider;
            leftBrakeEffect.Play();
        }
		else
        {
            wheelCollider = rightWheelCollider;
            rightBrakeEffect.Play();
        }
		wheelCollider.brakeTorque = brakeValue;
	}

	public void InitBrakeTorque(Wheel wheel)
	{
		WheelCollider wheelCollider;

		if (wheel == Wheel.LEFT)
        {
            wheelCollider = leftWheelCollider;
            leftBrakeEffect.Stop();
        }	
		else
        {
            wheelCollider = rightWheelCollider;
            rightBrakeEffect.Stop();
        }
		wheelCollider.brakeTorque = 0.0f;
	}
	void Update ()
	{
		leftWheel.transform.Rotate(0.0f, -leftWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f);
		rightWheel.transform.Rotate(0.0f, rightWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f);
		leftFrontWheel.transform.Rotate(0.0f, -leftWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f);
		rightFrontWheel.transform.Rotate(0.0f, rightWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f);

        DustEffectSetting(Wheel.LEFT);
        DustEffectSetting(Wheel.RIGHT);

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            leftWheelCollider.motorTorque = maxMotorTorque;
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            leftWheelCollider.motorTorque = 0;
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            rightWheelCollider.motorTorque = maxMotorTorque;
        }
        if(Input.GetKeyUp(KeyCode.RightArrow))
        {
            rightWheelCollider.motorTorque = 0;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            leftWheelCollider.brakeTorque = brakeValue;
            BrakeEffectPlay(Wheel.LEFT);
        }
        if(Input.GetKey(KeyCode.A))
        {
            BrakeEffectSetting(Wheel.LEFT);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            leftWheelCollider.brakeTorque = 0;
            BrakeEffectPause(Wheel.LEFT);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rightWheelCollider.brakeTorque = brakeValue;
            BrakeEffectPlay(Wheel.RIGHT);
        }
        if (Input.GetKey(KeyCode.D))
        {
            BrakeEffectSetting(Wheel.RIGHT);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            rightWheelCollider.brakeTorque = 0;
            BrakeEffectPause(Wheel.RIGHT);
        }
        
		Debug.Log("left: " + leftWheelCollider.motorTorque + " right: " + rightWheelCollider.motorTorque);
	}

    public bool LeftWheelIsGround()
    {
		return leftWheelCollider.isGrounded;
	}
    public bool RightWheelIsGround()
    {
		return rightWheelCollider.isGrounded;
	}

	public void SetLeftWheelMotorTorque(float value) { leftWheelCollider.motorTorque = value; }
	public void SetRightWheelMotorTorque(float value) { rightWheelCollider.motorTorque = value; }

	public void SetLeftWheelSteering(float value) { leftFrontWheelCollider.steerAngle = value; }
	public void SetRightWheelSteering(float value) { rightFrontWheelCollider.steerAngle = value; }
}
