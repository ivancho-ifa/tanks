using UnityEngine;
using UnityEngine.UI;

public class TankHealth
{
	public readonly GameObject tank;


	private readonly float startingHealth = 100f;
	private readonly Color fullHealthColor = Color.green;
	private readonly Color zeroHealthColor = Color.red;

	private readonly Image fillImage;
	private readonly Slider slider;

	private Explosion explosion;
	private float health;
	private bool dead;


	public TankHealth(GameObject tank, Image fillImage, Slider slider) {
		this.tank = tank;
		this.fillImage = fillImage;
		this.slider = slider;
	}

	public void SetupExplosion(GameObject explosionPrefab) => this.explosion.Prefab = explosionPrefab;


	public void OnEnable() {
		this.health = this.startingHealth;
		this.dead = false;

		this.explosion.GameObject.SetActive(false);

		this.SetHealthUI();
	}


	public void TakeDamage(float amount) {
		// Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.

		Debug.Assert(this.health > 0f);

		this.health -= amount;

		this.SetHealthUI();

		if (this.health <= 0f && !this.dead)
			this.OnDeath();
	}


	private float GetHealthPercentage() {
		// return (this.slider.value - this.slider.minValue) / (this.slider.maxValue - this.slider.minValue);

		Debug.Assert(this.slider.minValue == 0);
		Debug.Assert((0 <= this.health && this.slider.value == this.health) || this.health < 0);
		Debug.Assert(this.slider.maxValue == this.startingHealth);

		// We know this.slider.minValue is 0. For performance we simplify the arithmetics.
		return this.health / this.startingHealth;
	}


	private void SetHealthUI() {
		// Adjust the value and colour of the slider.

		this.slider.value = this.health;

		this.fillImage.color = Color.Lerp(this.zeroHealthColor, this.fullHealthColor, this.GetHealthPercentage());
	}


	private void OnDeath() {
		// Play the effects for the death of the tank and deactivate it.

		Debug.Assert(!this.dead);
		Debug.Assert(!this.explosion.GameObject.activeSelf);
		Debug.Assert(!this.explosion.Particles.isPlaying);
		Debug.Assert(!this.explosion.Audio.isPlaying);
		Debug.Assert(this.tank.activeSelf);

		this.dead = true;

		this.explosion.Particles.transform.position = this.tank.transform.position;
		this.explosion.GameObject.SetActive(true);
		this.explosion.Particles.Play();
		this.explosion.Audio.Play();

		this.tank.SetActive(false);
	}
}