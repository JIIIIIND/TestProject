using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveInputManager : MonoBehaviour {

	[SerializeField] private SteamVR_TrackedObject leftTrackedObject;
	[SerializeField] private SteamVR_TrackedObject rightTrackedObject;
	[SerializeField] private PlayerControl playerControl;

	private SteamVR_Controller.Device mDevice;

	void Start ()
	{}
	
	void Update () {
		if((int)rightTrackedObject.index != -1)
		{
			mDevice = SteamVR_Controller.Input((int)rightTrackedObject.index);

			//Debug.Log(mDevice.GetPressDown(SteamVR_Controller.ButtonMask.Grip));
			if (mDevice.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
			{
				Debug.Log("Grip Down");
				playerControl.SetRightInitPosition(rightTrackedObject.transform.position);
			}
			if (mDevice.GetPress(SteamVR_Controller.ButtonMask.Grip))
			{
				playerControl.CalculateRightPoint(rightTrackedObject.transform.position);
				Debug.Log("Griping");
			}
			if (mDevice.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
			{
				Debug.Log("Grip Up");
			}
		}

		if ((int)leftTrackedObject.index != -1)
		{
			mDevice = SteamVR_Controller.Input((int)leftTrackedObject.index);
			
			if (mDevice.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
			{
				Debug.Log("Grip Down");
				playerControl.SetLeftInitPosition(leftTrackedObject.transform.position);
			}
			if (mDevice.GetPress(SteamVR_Controller.ButtonMask.Grip))
			{
				playerControl.CalculateLeftPoint(leftTrackedObject.transform.position);
				Debug.Log("Griping");
			}
			if (mDevice.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
			{
				Debug.Log("Grip Up");
			}
		}
		playerControl.MakeMoveVector();

	}

}
