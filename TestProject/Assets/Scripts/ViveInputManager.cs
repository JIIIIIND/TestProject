using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveInputManager : MonoBehaviour
{

    [SerializeField] private SteamVR_TrackedObject leftTrackedObject;
    [SerializeField] private SteamVR_TrackedObject rightTrackedObject;
    [SerializeField] private PlayerControl playerControl;

    private SteamVR_Controller.Device mDevice;

    [SerializeField] private bool isControllerGrip;

    void Start()
    {
    }

    void Update()
    {
        if ((int)rightTrackedObject.index != -1)
        {
            mDevice = SteamVR_Controller.Input((int)rightTrackedObject.index);

            //Debug.Log(mDevice.GetPressDown(SteamVR_Controller.ButtonMask.Grip));
            if (mDevice.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
				
				Debug.Log("Grip Down");
                if(playerControl.GetWheelControl().RightWheelIsGround())
                {
					isControllerGrip = true;
					if (playerControl.GetMainMovement() != null)
                    {
                        Debug.Log("breaking Right");
                        playerControl.GetWheelControl().BrakeWheel(Wheel.RIGHT);
                    }
                    playerControl.RightPositionInitiate();
                    playerControl.SetRightInitPosition(rightTrackedObject.transform.localPosition);

                    playerControl.StartTimeCoroutine(false);
                }
                else if (playerControl.IsFlip() == true)
                {
                    playerControl.RightPositionInitiate();
                }
            }
            if (mDevice.GetPress(SteamVR_Controller.ButtonMask.Grip))
            {
                if (playerControl.GetWheelControl().RightWheelIsGround())
                {
					isControllerGrip = true;
					if (GameManager.instance.SoundEffectManager().GetVibration() == true)
                    {
                        mDevice.TriggerHapticPulse(1000);
                    }
                    if (playerControl.GetMainMovement() != null)
                    {
                        Debug.Log("breaking Right");
                        playerControl.GetWheelControl().BrakeWheel(Wheel.RIGHT);
                    }
                    playerControl.CalculateRightPoint(rightTrackedObject.transform.localPosition);
                    Debug.Log("Griping");
                }
                else if (playerControl.IsFlip() == true)
                {
                    playerControl.RightPositionInitiate();
                }
            }
            if (mDevice.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
				isControllerGrip = false;
				if (playerControl.GetWheelControl().RightWheelIsGround())
                {
                    playerControl.GetWheelControl().InitBrakeTorque(Wheel.RIGHT);
                    
                    playerControl.StartMoving(false);
                }
                else if (playerControl.IsFlip() == true)
                {
                    playerControl.RightPositionInitiate();
                }
                Debug.Log("Grip Up");
            }
            if(mDevice.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                if(GameManager.instance.UIController().GameMenuActive() == true)
                {
                    GameManager.instance.UIController().MenuExit();
                }
                else
                {
                    GameManager.instance.UIController().MenuAppear();
                }

                //게임 메뉴가 실행 중이라면
                //게임메뉴 비활성화
                //일시정지 해제
            }
        }

        if ((int)leftTrackedObject.index != -1)
        {
            mDevice = SteamVR_Controller.Input((int)leftTrackedObject.index);

            if (mDevice.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
                Debug.Log("Grip Down");
				
				if (playerControl.GetWheelControl().LeftWheelIsGround())
                {
					isControllerGrip = true;
					if (playerControl.GetMainMovement() != null)
                    {
                        Debug.Log("breaking left");
                        playerControl.GetWheelControl().BrakeWheel(Wheel.LEFT);
                    }
                    playerControl.LeftPositionInitiate();
                    playerControl.SetLeftInitPosition(leftTrackedObject.transform.localPosition);

                    playerControl.StartTimeCoroutine(true);
                }
                else if (playerControl.IsFlip() == true)
                {
                    playerControl.LeftPositionInitiate();
                }
            }
            if (mDevice.GetPress(SteamVR_Controller.ButtonMask.Grip))
            {
                
                if(playerControl.GetWheelControl().LeftWheelIsGround())
                {
                    if (GameManager.instance.SoundEffectManager().GetVibration() == true)
                    {
                        mDevice.TriggerHapticPulse(1000);
                    }
                    if (playerControl.GetMainMovement() != null)
                    {
                        Debug.Log("breaking left");
                        playerControl.GetWheelControl().BrakeWheel(Wheel.LEFT);
                    }
                    isControllerGrip = true;
                    playerControl.CalculateLeftPoint(leftTrackedObject.transform.localPosition);
                    Debug.Log("Griping");
                }
                else if (playerControl.IsFlip() == true)
                {
                    playerControl.LeftPositionInitiate();
                }
            }
            if (mDevice.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
				isControllerGrip = false;
				if (playerControl.GetWheelControl().LeftWheelIsGround())
                {
                    playerControl.GetWheelControl().InitBrakeTorque(Wheel.LEFT);
                    playerControl.StartMoving(true);
                    Debug.Log("Grip Up");
                }
                else if (playerControl.IsFlip() == true)
                {
                    playerControl.LeftPositionInitiate();
                }
            }
        }

        if (isControllerGrip == true)
		{
			playerControl.MakeMoveVector();
		}
		else
        {
			if (playerControl.GetGripMovement() != null)
			{
				//playerControl.StopCoroutine(playerControl.GetGripMovement());
			}

		}


        if ((int)rightTrackedObject.index != -1)
        {
            mDevice = SteamVR_Controller.Input((int)rightTrackedObject.index);
            if (mDevice.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
                playerControl.RightPositionInitiate();
            }
        }
        if ((int)leftTrackedObject.index != -1)
        {
            mDevice = SteamVR_Controller.Input((int)leftTrackedObject.index);
            if (mDevice.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
            {
                playerControl.LeftPositionInitiate();
            }
        }
        isControllerGrip = false;
        if (playerControl.IsFlip() == true)
        {
            playerControl.LeftPositionInitiate();
            playerControl.RightPositionInitiate();
            playerControl.TransformInit();
        }
    }
    private void InitPlayerController()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            Debug.Log("init");
            playerControl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
            leftTrackedObject = playerControl.GetLeftTrackedObject();
            rightTrackedObject = playerControl.GetRightTrackedObject();
        }
        else
        {
            Debug.Log("player is missing");
        }
    }

    public SteamVR_TrackedObject GetController(Wheel wheel)
    {
        if (wheel == Wheel.LEFT)
            return leftTrackedObject;
        else
            return rightTrackedObject;
    }
}
