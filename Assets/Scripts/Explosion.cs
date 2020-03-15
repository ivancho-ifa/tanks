using UnityEngine;

public struct Explosion
{
	public AudioSource Audio { get; private set; }
	public GameObject GameObject { get; private set; }
	public ParticleSystem Particles { get; private set; }
	public GameObject Prefab {
		set {
			this.GameObject = Object.Instantiate(value);
			this.Audio = this.GameObject.GetComponent<AudioSource>();
			this.Particles = this.GameObject.GetComponent<ParticleSystem>();
		}
	}
}