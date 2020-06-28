using UnityEngine;

// TODO: Use a pool of explosions.

public class ShellExplosion : MonoBehaviour
{
	public LayerMask tankMask;
	public GameObject explosionPrefab;
	public float maxDamage = 100f;
	public float explosionForce = 1000f;
	public float maxLifeTime = 2f;
	public float explosionRadius = 5f;

	private GameObject explosion;
	private AudioSource explosionAudio;
	private ParticleSystem explosionParticles;
	private float explosionDestroyAfter;


	private void Awake() {
		this.explosion = Instantiate(this.explosionPrefab);

		this.explosionParticles = this.explosion.GetComponent<ParticleSystem>();
		this.explosionAudio = this.explosion.GetComponent<AudioSource>();

		this.explosionDestroyAfter = Mathf.Max(this.explosionParticles.main.duration, this.explosionAudio.clip.length);
	}


	private void OnEnable() => this.explosion.SetActive(false);


	private void Start() {
		Destroy(this.explosion, this.maxLifeTime);
		Destroy(this.gameObject, this.maxLifeTime);
	}


	[System.Obsolete]
	private void OnTriggerEnter(Collider _) {
		// Find all the tanks in an area around the shell and damage them.

		Collider[] colliders = Physics.OverlapSphere(this.transform.position, this.explosionRadius, this.tankMask);
		foreach (Collider collider in colliders) {
			Rigidbody targetRigidbody = collider.GetComponent<Rigidbody>();
			TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();

			if (targetRigidbody != null && targetHealth != null) {
				targetRigidbody.AddExplosionForce(this.explosionForce, this.transform.position, this.explosionRadius);

				float damage = this.CalculateDamage(targetHealth.gameObject.transform.position);
				targetHealth.TakeDamage(damage);
			}
		}

		this.explosion.transform.position = this.transform.position;
		this.explosion.SetActive(true);

		this.explosionParticles.Play();
		this.explosionAudio.Play();

		Destroy(this.gameObject);
		Destroy(this.explosion, this.explosionDestroyAfter);
	}


	private float CalculateDamage(Vector3 targetPosition) {
		// Calculate the amount of damage a target should take based on it's position.

		Vector3 explosionToTarget = targetPosition - this.transform.position;
		float explosionDistance = explosionToTarget.magnitude;
		float relativeDistance = (this.explosionRadius - explosionDistance) / this.explosionRadius;
		float damage = Mathf.Max(0f, relativeDistance * this.maxDamage);

		return damage; 
	}
}