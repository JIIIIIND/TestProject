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
			
            if (mDevice.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
            {
				
				Debug.Log("Grip Down");
                if(playerControl.GetWheelControl().RightWheelIsGround())
                {
					isControllerGrip = true;

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
                    playerControl.CalculateRightPoint(rightTrackedObject.transform.localPosition);
                    playerControl.MakeMoveVector(Wheel.RIGHT);
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
                    playerControl.StartMoving(Wheel.RIGHT);
                }
                else if (playerControl.IsFlip() == true)
                {
                    playerControl.RightPositionInitiate();
                }
                Debug.Log("Grip Up");
            }
            if(mDevice.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                if(GameManager.instance.GameMenuActive() == true)
                {
                    GameManager.instance.MenuExit();
                }
                else
                {
                    GameManager.instance.MenuAppear();
                }

                //게임 메뉴가 실행 중이라면
                //게임메뉴 비활성화
                //일시정지 해제
            }
			if(mDevice.GetHairTrigger())
			{
					playerControl.GetWheelControl().BrakeWheel(Wheel.RIGHT);
			}
			if(mDevice.GetHairTriggerUp())
			{
				playerControl.GetWheelControl().InitBrakeTorque(Wheel.RIGHT);
			}
			if (mDevice.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
			{
				Debug.Log(mDevice.index + " " + mDevice.GetAxis().x * playerControl.GetWheelControl().maxSteeringAngle);
				playerControl.GetWheelControl().SetRightWheelSteering(mDevice.GetAxis().x * playerControl.GetWheelControl().maxSteeringAngle);
				playerControl.GetWheelControl().SetLeftWheelSteering(mDevice.GetAxis().x * playerControl.GetWheelControl().maxSteeringAngle);
			}
			if(mDevice.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
			{
				playerControl.GetWheelControl().SetRightWheelSteering(0);
				playerControl.GetWheelControl().SetLeftWheelSteering(0);
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
                    isControllerGrip = true;
                    playerControl.CalculateLeftPoint(leftTrackedObject.transform.localPosition);
                    playerControl.MakeMoveVector(Wheel.LEFT);
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
                    playerControl.StartMoving(Wheel.LEFT);
                    Debug.Log("Grip Up");
                }
                else if (playerControl.IsFlip() == true)
                {
                    playerControl.LeftPositionInitiate();
                }
            }
			if(mDevice.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
			{
				if (GameManager.instance.GameMenuActive() == true)
				{
					GameManager.instance.MenuExit();
				}
				else
				{
					GameManager.instance.MenuAppear();
				}
			}
			if (mDevice.GetHairTrigger())
			{
				playerControl.GetWheelControl().BrakeWheel(Wheel.LEFT);
			}
			if (mDevice.GetHairTriggerUp())
			{
				playerControl.GetWheelControl().InitBrakeTorque(Wheel.LEFT);
			}
			if (mDevice.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
			{
				Debug.Log(mDevice.index + " " + mDevice.GetAxis().y * playerControl.GetWheelControl().maxMotorTorque);
				playerControl.GetWheelControl().SetRightWheelMotorTorque(mDevice.GetAxis().y * playerControl.GetWheelControl().maxMotorTorque);
				playerControl.GetWheelControl().SetLeftWheelMotorTorque(mDevice.GetAxis().y * playerControl.GetWheelControl().maxMotorTorque);
			}
			if (mDevice.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
			{
				playerControl.GetWheelControl().SetRightWheelMotorTorque(0);
				playerControl.GetWheelControl().SetLeftWheelMotorTorque(0);
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
        if(isControllerGrip == false)
		{
			playerControl.GetWheelControl().SetLeftWheelMotorTorque(0);
			playerControl.GetWheelControl().SetRightWheelMotorTorque(0);
		}
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
