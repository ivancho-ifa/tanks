using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
	public readonly float startingHealth = 100f;
	public readonly Color fullHealthColor = Color.green;
	public readonly Color zeroHealthColor = Color.red;

	public GameObject explosionPrefab;
	public Image fillImage;
	public Slider slider;


	private GameObject explosion;
	private AudioSource explosionAudio;
	private ParticleSystem explosionParticles;
	private float currentHealth;
	private bool dead;


	private void Awake() {
		this.explosion = Instantiate(this.explosionPrefab);

		this.explosionParticles = this.explosion.GetComponent<ParticleSystem>();
		this.explosionAudio = this.explosion.GetComponent<AudioSource>();
	}


	private void OnEnable() {
		this.currentHealth = this.startingHealth;
		this.dead = false;

		this.explosion.SetActive(false);

		this.SetHealthUI();
	}


	public void TakeDamage(float amount) {
		// Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.

		Debug.Assert(this.currentHealth > 0f);

		this.currentHealth -= amount;

		this.SetHealthUI();

		if (this.currentHealth <= 0f && !this.dead)
			this.OnDeath();
	}


	private float GetHealthPercentage() {
		// return (this.slider.value - this.slider.minValue) / (this.slider.maxValue - this.slider.minValue);

		Debug.Assert(this.slider.minValue == 0);
		Debug.Assert((0 <= this.currentHealth && this.slider.value == this.currentHealth) || this.currentHealth < 0);
		Debug.Assert(this.slider.maxValue == this.startingHealth);

		// We know this.slider.minValue is 0. For performance we simplify the arithmetics.
		return this.currentHealth / this.startingHealth;
	}


	private void SetHealthUI() {
		// Adjust the value and colour of the slider.

		this.slider.value = this.currentHealth;

		this.fillImage.color = Color.Lerp(this.zeroHealthColor, this.fullHealthColor, this.GetHealthPercentage());
	}


	private void OnDeath() {
		// Play the effects for the death of the tank and deactivate it.

		Debug.Assert(!this.dead);
		Debug.Assert(!this.explosion.activeSelf);
		Debug.Assert(!this.explosionParticles.isPlaying);
		Debug.Assert(!this.explosionAudio.isPlaying);
		Debug.Assert(this.gameObject.activeSelf);

		this.dead = true;

		this.explosionParticles.transform.position = this.transform.position;
		this.explosion.SetActive(true);
		this.explosionParticles.Play();

		this.explosionAudio.Play();

		this.gameObject.SetActive(false);
	}
}