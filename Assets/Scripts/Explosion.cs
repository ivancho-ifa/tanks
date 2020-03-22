using System;
using UnityEngine;

[Serializable]
public struct Explosion
{
	public AudioSource audio;
	public ParticleSystem particles;
	public GameObject prefab;

	private GameObject gameObject;


	public void Instantiate() => this.gameObject = UnityEngine.Object.Instantiate(this.prefab);


	public void ExplodeAt(Vector3 position) {
		this.gameObject.SetActive(true);
		this.audio.Play();

		this.particles.transform.position = position;
		this.particles.Play();
	}


	public void StopExploding() {
		this.gameObject.SetActive(false);
		this.audio.Stop();
		this.particles.Stop();
	}


	public bool IsExploding() {
		bool isExploding = this.gameObject.activeSelf || this.particles.isPlaying || this.audio.isPlaying;

#if DEBUG
		bool allComponentsActive = this.gameObject.activeSelf && this.particles.isPlaying && this.audio.isPlaying;
		if (isExploding)
			Debug.Assert(allComponentsActive);
#endif

		return isExploding;
	}
}