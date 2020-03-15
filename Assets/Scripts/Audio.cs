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

	protected void ChangeCurrentSound(int index) => this.source.clip = this.sounds[index];

	protected bool IsCurrentSound(int index) => this.source.clip == this.sounds[index];
}