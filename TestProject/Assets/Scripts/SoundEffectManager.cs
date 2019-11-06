using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectManager : MonoBehaviour {

    [SerializeField] private float musicVolume;
    [SerializeField] private float effectVolume;
    [SerializeField] private bool isVibration;

	private List<Observer> observers;

	private void Awake()
	{
		observers = new List<Observer>();
	}
	void Start () {
		
	}
	
	void Update () {
		
	}

	public void AddObserver(Observer observer)
	{
		//Debug.Log(observer);
		observers.Add(observer);
		for (int i = 0; i < observers.Count; i++)
		{
			observers[i].onNotify(musicVolume, EVENTNAME.MUSIC);
			observers[i].onNotify(effectVolume, EVENTNAME.EFFECT);
		}
	}
	public void DeleteObserver(Observer observer) { observers.Remove(observer); }

    public float GetMusicVolume()
    { return musicVolume; }
    public void SetMusicVolume(UnityEngine.UI.Slider slider)
    {
		musicVolume = slider.value;
		for (int i = 0; i < observers.Count; i++)
		{
			observers[i].onNotify(musicVolume, EVENTNAME.MUSIC);
		}
	}

    public float GetEffectVolume()
    { return effectVolume; }
    public void SetEffectVolume(UnityEngine.UI.Slider slider)
    {
		effectVolume = slider.value;
		for (int i = 0; i < observers.Count; i++)
		{
			observers[i].onNotify(effectVolume, EVENTNAME.EFFECT);
		}
	}

    public bool GetVibration()
    { return isVibration; }
    public void SetVibration(UnityEngine.UI.Toggle toggle)
    { isVibration = toggle.isOn; }
}
