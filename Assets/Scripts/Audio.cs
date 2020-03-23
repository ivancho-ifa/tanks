using System;
using UnityEngine;

public class Audio
{
	private readonly AudioSource source;
	private readonly AudioClip[] sounds;


	public Audio(AudioSource source, params AudioClip[] sounds) {
		this.source = source;
		this.sounds = sounds;
	}

	public void PlayCurrentSound() => this.source.Play();

	public void ChangeCurrentSound(int index) => this.source.clip = this.sounds[index];

	public bool IsCurrentSound(int index) => this.source.clip == this.sounds[index];
}


public class EngineAudio : Audio
{
	public enum SoundID
	{
		Idling,
		Driving
	}


	public EngineAudio(AudioSource source, params AudioClip[] sounds) : base(source, sounds) {
		if (sounds.Length != 2)
			throw new ArgumentException("EngineAudio requires 2 sounds!");
	}

	public void ChangeCurrentSound(SoundID soundID) {
		this.ChangeCurrentSound((int)soundID);
		this.PlayCurrentSound();
	}

	public bool IsCurrentSound(SoundID engineSoundID) => this.IsCurrentSound((int)engineSoundID);
}


public class ShootingAudio : Audio
{
	public enum SoundID
	{
		Charging,
		Fire
	}


	public ShootingAudio
		(AudioSource source, params AudioClip[] sounds) : base(source, sounds) {
		if (sounds.Length != 2)
			throw new ArgumentException("ShootingAudio requires 2 sounds!");
	}

	public void ChangeCurrentSound(SoundID soundID) {
		this.ChangeCurrentSound((int)soundID);
		this.PlayCurrentSound();
	}

	public bool IsCurrentSound(SoundID soundID) => this.IsCurrentSound((int)soundID);
}