using System;
using UnityEngine;


public class Audio
{
	public AudioSource audioSource;


	public void ChangeCurrentSound(AudioClip audioClip) {
		this.audioSource.clip = audioClip;
		this.audioSource.Play();
	}

	public bool IsCurrentSound(AudioClip audioClip) => this.audioSource.clip == audioClip;
}


[Serializable]
public class EngineAudio : Audio
{
	public AudioClip engineIdling;
	public AudioClip engineDriving;
}


[Serializable]
public class ShootingAudio : Audio
{
	public AudioClip shotCharging;
	public AudioClip shotFiring;
}