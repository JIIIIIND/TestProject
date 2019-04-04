#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class UnityToUE4Coordinates : Editor {
	private static GameObject go;
	private static string Pos;
	private static string Rot;
	private static string Scale;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	[MenuItem ("NOT_Lonely/Copy Position in UE4 type %#_x")]
	static void CopyPos(){
		go = Selection.activeGameObject;
		Pos = "(" + "X=" + go.transform.position.x * -100 + ",Y=" + go.transform.position.z * 100 + ",Z=" + go.transform.position.y * 100 + ")";
		GUIUtility.systemCopyBuffer = Pos;
		Debug.Log (go + " converted position = " + Pos);
	}
	/*
	[MenuItem ("NOT_Lonely/Copy Rotation in UE4 type %#_x")]
	static void CopyRot(){
		go = Selection.activeGameObject;
		Rot = "(" + "Pitch=" + "0" + ",Yaw=" + go.transform.eulerAngles.y + ",Roll=" + "0" + ")";
		GUIUtility.systemCopyBuffer = Rot;
		Debug.Log (go + " converted rotation = " + Rot);
	}
	*/

	[MenuItem ("NOT_Lonely/Copy Rotation in UE4 type %#_z")]
	static void CopyRot(){
		go = Selection.activeGameObject;
		if (go.transform.eulerAngles.x >= 0) {
			Rot = "(" + "Pitch=" + go.transform.eulerAngles.z * -1 + ",Yaw=" + go.transform.eulerAngles.y + ",Roll=" + go.transform.eulerAngles.x + ")";
		} else {
			Rot = "(" + "Pitch=" + go.transform.eulerAngles.z * -1 + ",Yaw=" + go.transform.eulerAngles.y + ",Roll=" + go.transform.eulerAngles.x * -1 + ")";
		}

		GUIUtility.systemCopyBuffer = Rot;
		Debug.Log (go + " converted rotation = " + Rot);
	}
	[MenuItem ("NOT_Lonely/Copy Rotation (X = forward) in UE4 type %#_q")]


	static void CopyRot2(){
		go = Selection.activeGameObject;
		if (go.transform.eulerAngles.x >= 0) {
			float RotY = go.transform.eulerAngles.y + 90; 
			Rot = "(" + "Pitch=" + go.transform.eulerAngles.z * -1 + ",Yaw=" + RotY + ",Roll=" + go.transform.eulerAngles.x + ")";
		} else {
			float RotY = go.transform.eulerAngles.y + 90; 
			Rot = "(" + "Pitch=" + go.transform.eulerAngles.z * -1 + ",Yaw=" + RotY + ",Roll=" + go.transform.eulerAngles.x * -1 + ")";
		}

		GUIUtility.systemCopyBuffer = Rot;
		Debug.Log (go + " converted rotation = " + Rot);
	}

	[MenuItem ("NOT_Lonely/Copy Scale in UE4 type %#_e")]
	static void CopyScale(){
		go = Selection.activeGameObject;
		Scale = "(" + "X=" +go.transform.localScale.x + ",Y=" + go.transform.localScale.z + ",Z=" + go.transform.localScale.y + ")";
		GUIUtility.systemCopyBuffer = Scale;
		Debug.Log (go + " converted scale = " + Scale);
	}
}
#endif
