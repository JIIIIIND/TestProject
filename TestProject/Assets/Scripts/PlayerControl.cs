using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	[SerializeField] private float leftGripTime;
	[SerializeField] private float rightGripTime;

	[SerializeField] private Transform rightInitTransform;
	[SerializeField] private Transform leftInitTransform;
    
	private Vector3 rightInitPosition;
	private Vector3 rightMovingPosition;
	[SerializeField] private bool rightDirection;

	private Vector3 leftInitPosition;
	private Vector3 leftMovingPosition;
	[SerializeField] private bool leftDirection;

    [SerializeField] private Transform rayPosition;

	[SerializeField] private float timeRate;
	[SerializeField] private float speedRate;

	[SerializeField] private WheelControl wheelControl;
	
	[SerializeField] private float roadSaveTime;

	[SerializeField] private float priviousAngle = 0;
	[SerializeField] private float priviousSpeed = 0;

    [SerializeField] private AudioSource flipSound;

	private Vector3 lastRoadPosition;
	private Quaternion lastRoadRotation;
	private float currentTime;

	private Vector3 result;

    private Vector3 leftWheelVector;
    private Vector3 rightWheelVector;
	private bool isForward;
	public float currentSpeed;

	public float collisionRayLength;

	private IEnumerator leftGripMovement;
    private IEnumerator leftMainMovement;
    private IEnumerator leftGripCounter;

    private IEnumerator rightGripMovement;
    private IEnumerator rightMainMovement;
    private IEnumerator rightGripCounter;

    private IEnumerator flipSoundPlay;
	private void Awake()
	{
		rightInitPosition = rightInitTransform.localPosition;
        rightMovingPosition = rightInitTransform.localPosition + new Vector3(0, 0, 0.1f);

		leftInitPosition = leftInitTransform.localPosition;
		leftMovingPosition = leftInitTransform.localPosition + new Vector3(0, 0, 0.1f);
    }
	void Start()
    {
    }

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;

		Gizmos.DrawRay(this.transform.position, result);
		
	}

	public bool IsFlip()
	{
        /*
		if (Physics.Raycast(rayPosition.position, this.transform.TransformVector(new Vector3(0.3f, 1, 0)), collisionRayLength, 1 << LayerMask.NameToLayer("Floor")))
		{
			return true;
		}
		if (Physics.Raycast(rayPosition.position, this.transform.TransformVector(new Vector3(-0.3f, 1, 0)), collisionRayLength, 1<< LayerMask.NameToLayer("Floor")))
		{
			return true;
		}
		if (Physics.Raycast(rayPosition.position, this.transform.TransformVector(new Vector3(0, 1, 0.3f)), collisionRayLength, 1 << LayerMask.NameToLayer("Floor")))
		{
			return true;
		}
		if (Physics.Raycast(rayPosition.position, this.transform.TransformVector(new Vector3(0, 1, -0.3f)), collisionRayLength, 1 << LayerMask.NameToLayer("Floor")))
		{
			return true;
		}
		*/
		if (Physics.Raycast(rayPosition.position, this.transform.TransformVector(new Vector3(0, -1, 0)), collisionRayLength * 3, 1 << LayerMask.NameToLayer("Floor")))
		{
			return false;
		}
		else
        {
            return true;
        }
	}
	private IEnumerator SoundEnd(AudioSource source)
    {
        while (source.isPlaying)
            yield return true;
        this.GetComponent<Rigidbody>().Sleep();
        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.transform.position = lastRoadPosition;
        this.transform.rotation = lastRoadRotation;
        this.GetComponent<Rigidbody>().WakeUp();
        this.GetComponent<Rigidbody>().isKinematic = false;

        flipSoundPlay = null;
    }
	public void TransformInit()
	{
		Debug.Log("Flip and relocated");
        if (flipSoundPlay != null)
            return;
        else
        {
            flipSound.Play();
            flipSoundPlay = SoundEnd(flipSound);
            StartCoroutine(flipSoundPlay);
        }
	}

    IEnumerator CalculateMove(Wheel wheel, Vector3 direction, float timeValue, bool soundTrigger)
    {
		float speedTime = 0;
		bool isFirst = soundTrigger;
		priviousSpeed = Mathf.Lerp(direction.z / 0.797f, 0, speedTime);
		while (priviousSpeed != 0)
		{
			if (timeValue != 0)
				speedTime += (Time.deltaTime * timeValue);
			else
				speedTime += Time.deltaTime;
				
			if(speedTime > 1)
			{
				speedTime = 1;
			}
			priviousSpeed = Mathf.Lerp(direction.z / 0.797f, 0, speedTime);
            //일정 길이 이상이면 최대 속도로 취급, 짧게 움직였다면 그만큼 짧게 동작
            if (priviousSpeed > 1)
                priviousSpeed = 1;
			
			if(wheel == Wheel.LEFT)
			{
				wheelControl.SetLeftWheelMotorTorque(priviousSpeed * wheelControl.maxMotorTorque, isFirst);
			}
			else
			{
				wheelControl.SetRightWheelMotorTorque(priviousSpeed * wheelControl.maxMotorTorque, isFirst);
			}
			isFirst = false;
			yield return null;
		}
    }

	public void StartMoving(Wheel wheel)
	{
        if(wheel == Wheel.LEFT)
        {
            if (leftMainMovement != null)
			{
				StopCoroutine(leftMainMovement);
				wheelControl.SetLeftWheelMotorTorque(0, false);
			}
			Debug.Log("left move start");
			leftMainMovement = CalculateMove(wheel, leftWheelVector, leftGripTime, true);
			StartCoroutine(leftMainMovement);
        }
        else
        {
            if (rightMainMovement != null)
			{
				StopCoroutine(rightMainMovement);
				wheelControl.SetRightWheelMotorTorque(0, false);
			}
			Debug.Log("right move start");
            rightMainMovement = CalculateMove(wheel, rightWheelVector, leftGripTime, true);
			StartCoroutine(rightMainMovement);
		}
	}

	public void CalculateRightPoint(Vector3 currentPos)
	{
		float curDirection = currentPos.z - rightMovingPosition.z;
		
        if (curDirection > 0)
        {
            if (rightDirection)
            {
                //Debug.Log("curDirection > 0");
                SetRightMovingPosition(currentPos);
            }
            else
            {
                //Debug.Log("curDirection > 0 && Direction Change");
                SetRightInitPosition(rightMovingPosition);
                SetRightMovingPosition(currentPos);
                rightDirection = true;
            }
        }
        else if (curDirection < 0)
        {
            if (rightDirection)
            {
                //Debug.Log("curDirection < 0 && Direction Change");
                SetRightInitPosition(rightMovingPosition);
                SetRightMovingPosition(currentPos);
                rightDirection = false;
            }
            else
            {
                //Debug.Log("curDirection < 0");
                SetRightMovingPosition(currentPos);
            }
        }
        else
        {
            //Debug.Log("curDirection = 0");
            SetRightInitPosition(rightMovingPosition);
        }
	}
	public void CalculateLeftPoint(Vector3 currentPos)
	{
		float curDirection = currentPos.z - leftMovingPosition.z;
        if (curDirection > 0)
        {
            if (leftDirection)
            {
                //Debug.Log("Left: curDirection > 0");
                SetLeftMovingPosition(currentPos);
            }
            else
            {
                //Debug.Log("Left: curDirection > 0 && Direction Change");
                SetLeftInitPosition(leftMovingPosition);
                SetLeftMovingPosition(currentPos);
                leftDirection = true;
            }
        }
        else if (curDirection < 0)
        {
            if (leftDirection)
            {
                //Debug.Log("Left: curDirection < 0 && Direction Change");
                SetLeftInitPosition(leftMovingPosition);
                SetLeftMovingPosition(currentPos);
                leftDirection = false;
            }
            else
            {
                //Debug.Log("Left: curDirection < 0");
                SetLeftMovingPosition(currentPos);
            }
        }
        else
        {
            //Debug.Log("Left: curDirection = 0");
            SetLeftInitPosition(leftMovingPosition);
        }
	}

    public void MakeMoveVector(Wheel wheel)
    {
		//Debug.Log("MakeMoveVector()");
		float initYPosition = rightInitPosition.y;

		rightMovingPosition.y = initYPosition;
		leftInitPosition.y = initYPosition;
		leftMovingPosition.y = initYPosition;

        if(wheel == Wheel.LEFT)
        {
            leftWheelVector = leftMovingPosition - leftInitPosition;

            if(leftGripMovement != null)
			{
				StopCoroutine(leftGripMovement);
				wheelControl.SetLeftWheelMotorTorque(0, false);
			}
                
            leftGripMovement = CalculateMove(wheel, leftWheelVector, 0, false);
            StartCoroutine(leftGripMovement);
        }
        else
        {
            rightWheelVector = rightMovingPosition - rightInitPosition;

            if (rightGripMovement != null)
			{
				StopCoroutine(rightGripMovement);
				wheelControl.SetRightWheelMotorTorque(0, false);
			}
            
            rightGripMovement = CalculateMove(wheel, rightWheelVector, 0, false);
            StartCoroutine(rightGripMovement);
        }
	}

	private IEnumerator TimeCounter(bool isLeft)
	{
		while(true)
		{
			if (isLeft)
				leftGripTime += Time.deltaTime;
			else
				rightGripTime += Time.deltaTime;
			yield return null;
		}
	}
	public void StartTimeCoroutine(bool isLeft)
	{
		if(isLeft)
		{
			if (leftGripCounter == null)
				leftGripCounter = TimeCounter(true);
			StartCoroutine(leftGripCounter);
		}
		else
		{
			if (rightGripCounter == null)
				rightGripCounter = TimeCounter(false);
			StartCoroutine(rightGripCounter);
		}
	}

	public void SetLeftInitPosition(Vector3 pos)
	{
		leftInitPosition = pos;
		leftMovingPosition = pos;

		if (leftGripCounter != null)
		{
			StopCoroutine(leftGripCounter);
			leftGripTime = 0;
			leftGripCounter = TimeCounter(true);
			StartCoroutine(leftGripCounter);
		}
	}
	public void SetRightInitPosition(Vector3 pos)
	{
		rightInitPosition = pos;
		rightMovingPosition = pos;

		if(rightGripCounter != null)
		{
			StopCoroutine(rightGripCounter);
			rightGripTime = 0;
			rightGripCounter = TimeCounter(false);
			StartCoroutine(rightGripCounter);
		}
	}

	public void SetLeftMovingPosition(Vector3 pos)
	{ leftMovingPosition = pos; }
	public void SetRightMovingPosition(Vector3 pos)
	{ rightMovingPosition = pos;}

	public void RightPositionInitiate()
	{
		rightInitPosition = rightInitTransform.localPosition;
		rightMovingPosition = rightInitTransform.localPosition;
		//wheelControl.InitSteeringAngle();

		if(rightGripCounter != null)
			StopCoroutine(rightGripCounter);
	}
	public void LeftPositionInitiate()
	{
		leftInitPosition = leftInitTransform.localPosition;
		leftMovingPosition = leftInitTransform.localPosition;
		//wheelControl.InitSteeringAngle();

		if (leftGripCounter != null)
			StopCoroutine(leftGripCounter);
	}
    
	void Update ()
    {
		currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;
		currentTime += Time.deltaTime;
		if (roadSaveTime < currentTime)
		{
			RaycastHit hit;
			if(Physics.Raycast(rayPosition.position, this.transform.TransformVector(new Vector3(0,-1,0)),out hit, 3.5f, 1<<LayerMask.NameToLayer("Floor")))
			{
				lastRoadPosition = hit.point + new Vector3(0, 0.5f, 0);
				lastRoadRotation = this.transform.rotation;
                lastRoadRotation.x = 0;
                lastRoadRotation.z = 0;
			}
			currentTime = 0;
		}
        if (IsFlip())
        {
            LeftPositionInitiate();
            RightPositionInitiate();
            TransformInit();
        }
		//Debug.Log("direction X value: " + result.x/result.magnitude + "direction Z value: " + result.z/result.magnitude);
	}
    public SteamVR_TrackedObject GetLeftTrackedObject()
    {
        return leftInitTransform.gameObject.GetComponentInParent<SteamVR_TrackedObject>();
    }
    public SteamVR_TrackedObject GetRightTrackedObject()
    {
        return rightInitTransform.gameObject.GetComponentInParent<SteamVR_TrackedObject>();
    }
    public WheelControl GetWheelControl()
    {
        return wheelControl;
    }
    public void SetLastRoadPosition(Vector3 value)
    {
        lastRoadPosition = value;
    }
}
