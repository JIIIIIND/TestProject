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

	[SerializeField] private SteamVR_Controller leftController;
	[SerializeField] private SteamVR_Controller rightController;

	[SerializeField] private float timeRate;

	private Vector3 result;
	private bool isForward;

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
		
		if(gripMovement != null)
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
            Debug.Log("right is big: " + result.magnitude);
			Debug.Log("left is " + leftWheel.magnitude + "right is " + rightWheel.magnitude);
		}
		else
		{
			forward = leftDirection;
			result = rightMovingPosition - leftModifiedMovingPoint;
            Debug.Log("left is big: " + result.magnitude);
			Debug.Log("left is " + leftWheel.magnitude + "right is " + rightWheel.magnitude);
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

    //*/
		/*
		//Vector3 right = rightInitTransform.position + this.transform.TransformDirection(Vector3.forward * rightWheel.magnitude);
		Vector3 right = rightInitTransform.position + (Vector3.forward * rightWheel.magnitude);
		//if (rightDirection == false)
		//right = rightInitTransform.position + this.transform.TransformDirection(-(Vector3.forward) * rightWheel.magnitude);
		//Vector3 left = leftInitTransform.position + this.transform.TransformDirection(Vector3.forward * leftWheel.magnitude);
		Vector3 left = leftInitTransform.position + (Vector3.forward * leftWheel.magnitude);
		//if (leftDirection == false)
		//left = leftInitTransform.position + this.transform.TransformDirection(-(Vector3.forward) * leftWheel.magnitude);

		Debug.DrawRay(this.transform.position, right, Color.yellow);
		Debug.DrawRay(this.transform.position, left, Color.green);

		if (rightWheel.magnitude > leftWheel.magnitude)
        {
            forward = rightDirection;
            result = left - right;
        }
        else
        {
            forward = leftDirection;
            result = right - left;
        }
        result = this.transform.TransformVector(result);
		Debug.DrawRay(this.transform.position, result, Color.blue);
        if (forward)
        {
            if(rightWheel.magnitude > leftWheel.magnitude)
            {
                result = Quaternion.Euler(0, -90, 0) * result;
            }
            else
            {
                result = Quaternion.Euler(0, 90, 0) * result;
            }
        }
        else
        {
            if(rightWheel.magnitude > leftWheel.magnitude)
            {
                result = Quaternion.Euler(0, 90, 0) * result;
            }
            else
            {
                result = Quaternion.Euler(0, -90, 0) * result;
            }
        }
		*/
        isForward = forward;

		return result;
    }

    IEnumerator CalculateMove(float speedValue)
    {
		while (true)
		{
			if (isForward)
				this.transform.position += this.transform.TransformDirection(Vector3.forward) * speedValue * Time.deltaTime;
			else
				this.transform.position += this.transform.TransformDirection(-Vector3.forward) * speedValue * Time.deltaTime;

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
			mainMovement = SpeedControl(leftGripTime, result);
		else
			mainMovement = SpeedControl(rightGripTime, result);
		StartCoroutine(mainMovement);
		RotationBody(result);
	}

	IEnumerator SpeedControl(float timeValue, Vector3 direction)
	{
		float speedValue = direction.magnitude;
		float angleVariable = 0.0f;
		
		while(speedValue > 0)
		{
			if(enumerator != null)
				StopCoroutine(enumerator);
			
			speedValue *= Mathf.Cos(angleVariable) * (timeValue * timeRate);
			enumerator = CalculateMove(speedValue);
			StartCoroutine(enumerator);
			angleVariable += (timeValue * timeRate);
			//값 수정 필요함. time과 속도 간에 비율 수정 필요
			if (angleVariable > (Mathf.PI / 2))
				angleVariable = (Mathf.PI / 2);
			yield return new WaitForSeconds(0.5f);
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
}
