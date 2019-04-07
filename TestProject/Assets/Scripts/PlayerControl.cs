using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	[SerializeField] private float leftGripTime;
	[SerializeField] private float rightGripTime;

	[SerializeField] private Transform rightInitTransform;
	[SerializeField] private Transform leftInitTransform;

	[SerializeField] private float rotationSpeed;

	[SerializeField] private Vector3 rightInitPosition;
	[SerializeField] private Vector3 rightMovingPosition;
	[SerializeField] private bool rightDirection;

	[SerializeField] private Vector3 leftInitPosition;
	[SerializeField] private Vector3 leftMovingPosition;
	[SerializeField] private bool leftDirection;

    [SerializeField] private Transform rayPosition;

	[SerializeField] private float timeRate;
	[SerializeField] private float speedRate;

	[SerializeField] private WheelControl wheelControl;
	
	[SerializeField] private float roadSaveTime;
	private Vector3 lastRoadPosition;
	private Quaternion lastRoadRotation;
	private float currentTime;

	private Vector3 result;
	private bool isForward;
	public float currentSpeed;

	public float collisionRayLength;

	private IEnumerator gripMovement;
	private IEnumerator mainMovement;
	private IEnumerator leftGripCounter;
	private IEnumerator rightGripCounter;
	private IEnumerator enumerator;

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

		Gizmos.DrawRay(leftInitTransform.TransformPoint(leftInitPosition), leftMovingPosition - leftInitPosition);
		Gizmos.DrawRay(rightInitTransform.TransformPoint(rightInitPosition), rightMovingPosition - rightInitPosition);

		Ray ray = new Ray(this.transform.position, this.transform.TransformVector(new Vector3(1, 1, 0)) * collisionRayLength);
		Gizmos.DrawRay(ray);
		
	}

	public bool IsFlip()
	{
		if (Physics.Raycast(rayPosition.position, this.transform.TransformVector(new Vector3(0.3f, 1, 0)), collisionRayLength, 1>>0))
		{
			return true;
		}
		if (Physics.Raycast(rayPosition.position, this.transform.TransformVector(new Vector3(-0.3f, 1, 0)), collisionRayLength, 1>>0))
		{
			return true;
		}
		return false;
	}
	
	public void TransformInit()
	{
		Debug.Log("Flip and relocated");
		this.GetComponent<Rigidbody>().Sleep();
		this.GetComponent<Rigidbody>().isKinematic = true;
		this.GetComponent<Rigidbody>().velocity = Vector3.zero;
		this.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		this.transform.position = lastRoadPosition;
		this.transform.rotation = lastRoadRotation;
		this.GetComponent<Rigidbody>().WakeUp();
		this.GetComponent<Rigidbody>().isKinematic = false;
		/*
		this.transform.position = lastRoadPosition.position;
		this.transform.rotation = lastRoadPosition.rotation;
		*/
	}

	private void RotationBody(Vector3 target)
    {
		//target이 향하는 방향으로 회전
		//회전 속도는 target의 크기로 결정
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(target), rotationSpeed * Time.deltaTime);

    }

    private void MoveToward(Vector3 rightWheel, Vector3 leftWheel)
    {
        //컨트롤러 쥔 상태로 뒤에서 앞으로, 앞에서 뒤로
        Vector3 targetVector = CalculateVector(rightWheel, leftWheel);
		
        RotationBody(targetVector);
		if (gripMovement != null)
		{
			StopCoroutine(gripMovement);
		}
		gripMovement = CalculateMove(targetVector.magnitude);
		StartCoroutine(gripMovement);
		
        //targetVector의 크기가 속도 결정
        //
        //이동은 Coroutine으로 지속적으로 속도가 감소하면서 이동함
        //새로운 Coroutine이 호출되기 전에 이전 Coroutine은 제거하고 호출
    }

    private Vector3 CalculateVector(Vector3 rightWheel, Vector3 leftWheel)
    {
		//result에 오른쪽과 왼쪽 바퀴의 벡터를 연산하여 나온 값 할당
		//매개변수 값이 나타내는 것은 컨트롤러를 쥐고 이동한 값이다.
		//해당되는 값의 끝 점을 서로 이은 벡터와 수직인 벡터가 result가 된다.
		//rightWheel과 leftWheel은 각각 최대 값이 정해져 있어야 한다.

		bool forward;

		///*
		float initZPosition = rightInitPosition.z;

		Vector3 leftModifiedInitPoint = new Vector3(leftInitPosition.x, leftInitPosition.y, initZPosition);
		Vector3 leftModifiedMovingPoint = leftModifiedInitPoint + leftWheel;

		if (rightWheel.magnitude > leftWheel.magnitude)
		{
			forward = rightDirection;
			result = leftModifiedMovingPoint - rightMovingPosition;
		}
		else
		{
			forward = leftDirection;
			result = rightMovingPosition - leftModifiedMovingPoint;
        }
		result = this.transform.TransformVector(result);
		if (forward)
		{
			//왼쪽이 크면 반시계, 오른쪽이 크면 시계
			if(rightWheel.magnitude > leftWheel.magnitude)
			{
				result = Quaternion.Euler(0, 90, 0) * result;
				Debug.Log("전진 오른쪽");
			}
			else
			{
				result = Quaternion.Euler(0, -90, 0) * result;
				Debug.Log("전진 왼쪽");
			}
		}
		else
		{
			//오른쪽이 크면 반시계, 왼쪽이 크면 시계
			if(rightWheel.magnitude > leftWheel.magnitude)
			{
				result = Quaternion.Euler(0, 90, 0) * result;
				Debug.Log("후진 오른쪽");
			}
			else
			{
				result = Quaternion.Euler(0, -90, 0) * result;
				Debug.Log("후진 왼쪽");
			}
		}
        isForward = forward;

		if (result.magnitude < speedRate)
		{
			result *= 3;
			Debug.Log(result.magnitude);
		}

		return result;
    }

    IEnumerator CalculateMove(float speedValue)
    {
		while (true)
		{
			if (isForward)
            {
				//this.transform.position += this.transform.TransformDirection(Vector3.forward) * speedValue * Time.deltaTime;
				wheelControl.MotorTorque(Wheel.LEFT, speedValue * 3);
				wheelControl.MotorTorque(Wheel.RIGHT, speedValue * 3);

			}
			else
            {
				//this.transform.position += this.transform.TransformDirection(-Vector3.forward) * speedValue * Time.deltaTime;
				wheelControl.MotorTorque(Wheel.LEFT, -speedValue * 3);
				wheelControl.MotorTorque(Wheel.RIGHT, -speedValue * 3);
			}
			
			yield return null;
		}
    }

	public void StartMoving(bool isLeft)
	{
		if (mainMovement != null)
		{
			Debug.Log("breaking_inside");
			StopCoroutine(enumerator);
			StopCoroutine(mainMovement);
		}
		if (isLeft)
		{
			if(leftGripTime > 0.2f)
				result = result * ((timeRate) / leftGripTime);
			
			mainMovement = SpeedControl(leftGripTime, result);
		}
		else
		{
			if (rightGripTime > 0.2f)
				result = result * ((timeRate) / rightGripTime);
			mainMovement = SpeedControl(rightGripTime, result);
		}
		StartCoroutine(mainMovement);
		
	}

	IEnumerator SpeedControl(float timeValue, Vector3 direction)
	{
		float speedValue = direction.magnitude;
		float angleVariable = 0.0f;
		
		while(speedValue > 0)
		{
			if(enumerator != null)
				StopCoroutine(enumerator);

			speedValue *= Mathf.Cos(angleVariable);
			
			enumerator = CalculateMove(speedValue);
			StartCoroutine(enumerator);
			RotationBody(direction);
			angleVariable += ((Mathf.PI / 2)/5) * Time.deltaTime;
			//값 수정 필요함. time과 속도 간에 비율 수정 필요
			if (angleVariable > (Mathf.PI / 2))
				angleVariable = (Mathf.PI / 2);
			yield return null;
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

    public void MakeMoveVector()
    {
        Vector3 rightWheel;
        Vector3 leftWheel;

		float initYPosition = rightInitPosition.y;

		rightMovingPosition.y = initYPosition;
		leftInitPosition.y = initYPosition;
		leftMovingPosition.y = initYPosition;

		rightWheel = rightMovingPosition - rightInitPosition;
		
		leftWheel = leftMovingPosition - leftInitPosition;

		MoveToward(rightWheel, leftWheel);
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

		if(rightGripCounter != null)
			StopCoroutine(rightGripCounter);
	}
	public void LeftPositionInitiate()
	{
		leftInitPosition = leftInitTransform.localPosition;
		leftMovingPosition = leftInitTransform.localPosition;

		if(leftGripCounter != null)
			StopCoroutine(leftGripCounter);
	}

	public IEnumerator GetGripMovement() { return gripMovement; }
	public IEnumerator GetLeftGripCounter() { return leftGripCounter; }
	public IEnumerator GetRightGripCounter() { return rightGripCounter; }
	public IEnumerator GetMainMovement() { return mainMovement; }
	public IEnumerator GetEnumerator() { return enumerator; }

	void Update ()
    {
		currentSpeed = GetComponent<Rigidbody>().velocity.magnitude;
		currentTime += Time.deltaTime;
		if (roadSaveTime < currentTime)
		{
			RaycastHit hit;
			if(Physics.Raycast(this.transform.position, this.transform.TransformVector(new Vector3(0,-1.5f,0)),out hit))
			{
				if(hit.collider.tag == "Road")
				{
					Debug.Log("Save road position");
					lastRoadPosition = hit.point + new Vector3(0,5,0);
					lastRoadRotation = this.transform.rotation;
				}
			}
			currentTime = 0;
		}
		
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			wheelControl.BrakeWheel(Wheel.LEFT);
		}
		if(Input.GetKey(KeyCode.RightArrow))
		{
			wheelControl.BrakeWheel(Wheel.RIGHT);
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			wheelControl.InitBrakeTorque(Wheel.LEFT);
		}
		if (Input.GetKeyUp(KeyCode.RightArrow))
		{
			wheelControl.InitBrakeTorque(Wheel.RIGHT);
		}
		/*
		if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveToward(this.transform.TransformDirection(Vector3.forward * 0.1f), Vector3.zero);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveToward(Vector3.zero, this.transform.TransformDirection(Vector3.forward * 0.1f));
        }
        if(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            MoveToward(this.transform.TransformDirection(Vector3.forward * 0.1f), this.transform.TransformDirection(Vector3.forward * 0.1f));
        }
        if(!(Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow)))
        {
            StopCoroutine(gripMovement);
        }
		*/
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

}
