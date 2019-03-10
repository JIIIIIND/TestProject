using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	private float gripTime;

	[SerializeField] private float rotationSpeed;

	[SerializeField] private Vector3 rightInitPosition;
	[SerializeField] private Vector3 rightMovingPosition;
	[SerializeField] private bool rightDirection;
	
	private Vector3 leftInitPosition;
	private Vector3 leftMovingPosition;
	private bool leftDirection;

	[SerializeField] private SteamVR_Controller leftController;
	[SerializeField] private SteamVR_Controller rightController;

	private Vector3 result;
	void Start()
    {
        
    }

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;

		Gizmos.DrawRay(this.transform.position, result * 10000);
	}

	private void RotationBody(Vector3 target)
    {
		//target이 향하는 방향으로 회전
		//회전 속도는 target의 크기로 결정
		//회전은 Slerp나 Coroutine을 사용하여 서서히 회전
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(target), rotationSpeed * Time.deltaTime);

    }

    private void MoveToward(Vector3 rightWheel, Vector3 leftWheel)
    {
        //컨트롤러 쥔 상태로 뒤에서 앞으로, 앞에서 뒤로
        Vector3 targetVector = CalculateVector(rightWheel, leftWheel);
        RotationBody(targetVector);
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
		if (rightWheel.magnitude > leftWheel.magnitude)
		{
			forward = rightDirection;
			result = leftWheel - rightWheel;
		}
		else
		{
			forward = leftDirection;
			result = rightWheel - leftWheel;
		}
		
		if(forward)
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
				result = Quaternion.Euler(0, -90, 0) * result;
				Debug.Log("후진 오른쪽");
			}
			else
			{
				result = Quaternion.Euler(0, 90, 0) * result;
				Debug.Log("후진 왼쪽");
			}
		}
		
		return result;
    }

    IEnumerator CalculateMove(float speedValue)
    {
        //speedValue가 양수나 음수에 따라 동작 다르게
        //양수인 경우 전진, 0이 될 때 까지 지속적으로 감소
        //음수인 경우에 후진, 0이 될 때 까지 지속적으로 증가
        yield return null;
    }
	
	public void CalculateRightPoint(Vector3 currentPos)
	{
		float curDirection = currentPos.x - rightMovingPosition.x;
		Debug.Log("Rotation: " + this.transform.rotation.y);
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
		float curDirection = currentPos.x - leftMovingPosition.x;
        if (curDirection > 0)
        {
            if (leftDirection)
            {
                Debug.Log("Left: curDirection > 0");
                SetLeftMovingPosition(currentPos);
            }
            else
            {
                Debug.Log("Left: curDirection > 0 && Direction Change");
                SetLeftInitPosition(leftMovingPosition);
                SetLeftMovingPosition(currentPos);
                leftDirection = true;
            }
        }
        else if (curDirection < 0)
        {
            if (leftDirection)
            {
                Debug.Log("Left: curDirection < 0 && Direction Change");
                SetLeftInitPosition(leftMovingPosition);
                SetLeftMovingPosition(currentPos);
                leftDirection = false;
            }
            else
            {
                Debug.Log("Left: curDirection < 0");
                SetLeftMovingPosition(currentPos);
            }
        }
        else
        {
            Debug.Log("Left: curDirection = 0");
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

	public void SetLeftInitPosition(Vector3 pos)
	{
		leftInitPosition = pos;
		leftMovingPosition = pos;
		gripTime = 0;
	}
	public void SetRightInitPosition(Vector3 pos)
	{
		rightInitPosition = pos;
		rightMovingPosition = pos;
		gripTime = 0;
	}

	public void SetLeftMovingPosition(Vector3 pos)
	{ leftMovingPosition = pos; }
	public void SetRightMovingPosition(Vector3 pos)
	{ rightMovingPosition = pos;}

	void Update ()
    {
		gripTime += Time.deltaTime;
    }
}
