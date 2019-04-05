using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour {

    [SerializeField] private float musicVolume;
    [SerializeField] private float effectVolume;
    [SerializeField] private bool isVibration;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public float GetMusicVolume()
    { return musicVolume; }
    public void SetMusicVolume(UnityEngine.UI.Slider slider)
    { musicVolume = slider.value; }

    public float GetEffectVolume()
    { return effectVolume; }
    public void SetEffectVolume(UnityEngine.UI.Slider slider)
    { effectVolume = slider.value; }

    public bool GetVibration()
    { return isVibration; }
    public void SetVibration(UnityEngine.UI.Toggle toggle)
    { isVibration = toggle.isOn; }
}
