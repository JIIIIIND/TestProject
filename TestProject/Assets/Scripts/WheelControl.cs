using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Wheel { LEFT, RIGHT }
public class WheelControl : MonoBehaviour {
	
	private float rotationAngle;

	public float maxMotorTorque;
	public float maxSteeringAngle;

    private bool brakeSound;
    [SerializeField] private WheelSound wheelSoundSystem;
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

    [SerializeField] private float brakeSoundEndValue;
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
        if((leftWheelCollider.rpm > 0) || (rightWheelCollider.rpm  > 0))
        {
            //start sound
            brakeSound = true;
            wheelSoundSystem.BrakeStart();
        }
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

        if(brakeSound == true)
        {
            //EndSound 출력
            Debug.Log("brake key is up");
            wheelSoundSystem.BrakeEnd();
        }
	}
	void Update ()
	{
		
		leftWheel.transform.Rotate(0.0f, -leftWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f);
		rightWheel.transform.Rotate(0.0f, rightWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f);
		leftFrontWheel.transform.Rotate(0.0f, -leftWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f);
		rightFrontWheel.transform.Rotate(0.0f, rightWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f);
		/*
		leftWheel.transform.Rotate(leftWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f, 0.0f);
		rightWheel.transform.Rotate(rightWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f, 0.0f);
		leftFrontWheel.transform.Rotate(leftWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f, 0.0f);
		rightFrontWheel.transform.Rotate(rightWheelCollider.rpm / 60 * 360 * Time.deltaTime, 0.0f, 0.0f);
		*/
		DustEffectSetting(Wheel.LEFT);
        DustEffectSetting(Wheel.RIGHT);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetLeftWheelMotorTorque(maxMotorTorque, true);
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow))
        {
            SetLeftWheelMotorTorque(0, false);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetRightWheelMotorTorque(maxMotorTorque, true);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            leftWheelCollider.motorTorque = -leftWheelCollider.motorTorque;
            rightWheelCollider.motorTorque = -rightWheelCollider.motorTorque;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            SetRightWheelMotorTorque(0, false);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            BrakeWheel(Wheel.LEFT);
        }
        if(Input.GetKey(KeyCode.A))
        {
            BrakeEffectSetting(Wheel.LEFT);
        }
        //left brake
        if (Input.GetKeyUp(KeyCode.A))
        {
            InitBrakeTorque(Wheel.LEFT);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            BrakeWheel(Wheel.RIGHT);
        }
        //right brake
        if (Input.GetKey(KeyCode.D))
        {
            BrakeEffectSetting(Wheel.RIGHT);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            InitBrakeTorque(Wheel.RIGHT);
        }
		
        if(brakeSound)
        {
            if((leftWheelCollider.rpm < brakeSoundEndValue) || (rightWheelCollider.rpm < brakeSoundEndValue))
            {
                brakeSound = false;
                //endSound 출력
                Debug.Log("I'm Calling Brake End");
                wheelSoundSystem.BrakeEnd();
            }
        }
        //브레이크를 잡고 있지만 rpm이 일정 값 이하로 떨어지거나 특정 값 이상이지만 브레이크 키에서 손을 놓은 경우 brakeEnd 호출
	}

    public bool LeftWheelIsGround()
    {
		return leftWheelCollider.isGrounded;
	}
    public bool RightWheelIsGround()
    {
		return rightWheelCollider.isGrounded;
	}

	public void SetLeftWheelMotorTorque(float value, bool isFirst)
    {
        leftWheelCollider.motorTorque = value;
		rightWheelCollider.motorTorque = value * 0.8f;
        if((value != 0) && isFirst)
        {
			Debug.Log("left sound");
            wheelSoundSystem.WheelSoundStart();
        }
    }
	public void SetRightWheelMotorTorque(float value, bool isFirst)
    {
        rightWheelCollider.motorTorque = value;
		leftWheelCollider.motorTorque = value * 0.8f;
		if ((value != 0) && isFirst)
        {
			Debug.Log("right sound");
			wheelSoundSystem.WheelSoundStart();
        }
    }
	public void SetFrontWheelColliderTorque(float value)
	{
		rightFrontWheelCollider.motorTorque = value;
		leftFrontWheelCollider.motorTorque = value;
	}
    public float GetLeftWheelMotorTorque() { return leftWheelCollider.motorTorque; }
    public float GetRightWheelMotorTorque() { return rightWheelCollider.motorTorque; }

    public void SetLeftWheelSteering(float value) { leftFrontWheelCollider.steerAngle = value; }
	public void SetRightWheelSteering(float value) { rightFrontWheelCollider.steerAngle = value; }
    public void SetBrakeSoundToggle(bool value) { brakeSound = value; }
}
