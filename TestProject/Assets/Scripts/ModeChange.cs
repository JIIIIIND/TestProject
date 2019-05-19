using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MODE { MANUAL, ELECTRIC}

public class ModeChange : MonoBehaviour {

	public static ModeChange instance;
	[SerializeField] private MODE mode;
	[SerializeField] private UnityEngine.UI.Text manualText;
	[SerializeField] private UnityEngine.UI.Text electricText;

	private ModeChange()
	{ }
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(transform.gameObject);
		
	}
	void Start ()
	{
		mode = MODE.MANUAL;
	}
	
	public void ModeSwap()
	{
		if(mode == MODE.MANUAL)
		{
			mode = MODE.ELECTRIC;
			manualText.gameObject.SetActive(false);
			electricText.gameObject.SetActive(true);
		}
		else
		{
			mode = MODE.MANUAL;
			manualText.gameObject.SetActive(true);
			electricText.gameObject.SetActive(false);
		}
	}
	
	public MODE getMode() { return mode; }
}
