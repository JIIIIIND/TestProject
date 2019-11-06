using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelSound : MonoBehaviour {

    [SerializeField] private PlayerControl playerControl;

    [SerializeField] private AudioSource brakeStartSound;
    [SerializeField] private AudioSource brakeMidSound;
    private IEnumerator brakeLooping;
    [SerializeField] private AudioSource brakeEndSound;

    [SerializeField] private AudioSource wheelStartSound;
    private IEnumerator wheelSound;
    [SerializeField] private AudioSource wheelEndSound;

    [SerializeField] private AudioSource flipSound;
    [SerializeField] private AudioSource collisionSound;

    // Use this for initialization
    void Start ()
    {
        brakeStartSound.loop = false;
        brakeMidSound.loop = true;
        brakeEndSound.loop = false;

        wheelStartSound.loop = false;
        wheelEndSound.loop = false;

        flipSound.loop = false;
        collisionSound.loop = false;
	}


    public void BrakeStart()
    {
        if (brakeEndSound.isPlaying)
            brakeEndSound.Stop();
        brakeStartSound.Play();
        if (brakeLooping != null)
        {
            StopCoroutine(brakeLooping);
            brakeLooping = null;
        }
        brakeLooping = BrakeLooping();
        StartCoroutine(brakeLooping);
    }

    private IEnumerator BrakeLooping()
    {
        while (brakeStartSound.isPlaying)
        {
            yield return null;
        }
        brakeMidSound.loop = true;
        brakeMidSound.Play();
    }

    public void BrakeEnd() //브레이크가 동작하고 있을 때, RPM이 특정 값 이하로 떨어진 경우 실행  + 특정 값 이상이더라도 브레이크 키를 놓은 경우
    {
        if(brakeLooping != null)
        {
            StopCoroutine(brakeLooping);
            brakeLooping = null;
        }
        StartCoroutine(WaitForSoundClipEnd(brakeMidSound, brakeEndSound));
        //Debug.Log("EndSound is Playing... maybe");
    }

    private IEnumerator WaitForSoundClipEnd(AudioSource source, AudioSource nextSource)
    {
        source.loop = false;
        while(source.isPlaying)
        {
            yield return null;
        }
        ////Debug.Log(source);
        source.Stop();
        nextSource.Play();
    }

    public void WheelSoundStart()
    {
        if (wheelSound != null)
        {
            StopCoroutine(wheelSound);
            wheelSound = null;
        }
        wheelStartSound.Play();
        wheelSound = WaitForSoundClipEnd(wheelStartSound, wheelEndSound);
        StartCoroutine(wheelSound);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
