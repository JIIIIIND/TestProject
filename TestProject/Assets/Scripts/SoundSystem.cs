using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour, Observer {

	[SerializeField] private AudioSource[] BGMArray;
	[SerializeField] private AudioSource[] EffectSoundArray;

	public void onNotify(float value, EVENTNAME eventName)
	{
		switch(eventName)
		{
			case EVENTNAME.MUSIC:
			{
				for(int i = 0; i < BGMArray.Length; i++)
				{
					BGMArray[i].volume = value;
				}
				break;
			}
			case EVENTNAME.EFFECT:
			{
				for (int i = 0; i < EffectSoundArray.Length; i++)
				{
					EffectSoundArray[i].volume = value;
				}
				break;
			}
			default:
				break;
		}
	}

	void Start ()
	{
		GameManager.instance.SoundEffectManager().AddObserver(this);
	}

	private void OnDestroy()
	{
		GameManager.instance.SoundEffectManager().DeleteObserver(this);
	}

	void Update () {
		
	}
}
