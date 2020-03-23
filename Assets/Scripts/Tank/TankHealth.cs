using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TankHealth : NetworkBehaviour
{
	private readonly float startingHealth = 100f;
	private readonly Color fullHealthColor = Color.green;
	private readonly Color zeroHealthColor = Color.red;

	public Image fillImage;
	public Slider slider;

	public Explosion explosion;

	[SyncVar] private float health;
	private bool dead;


	public void Awake() => this.explosion.Instantiate();


	public void OnEnable() {
		this.health = this.startingHealth;
		this.dead = false;

		this.explosion.StopExploding();
	}


	public void Update() => this.SetHealthUI();


	public void TakeDamage(float amount) {
		// Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.

		Debug.Assert(this.health > 0f);

		// Forbid clients managing their own health.
		if (this.isServer)
			this.health -= amount;

		if (this.isClient)
			if (this.health <= 0f && !this.dead)
				this.CmdOnDeath();

		// TODO: Maybe SetHealthUI could be called here when it is actually changed instead of on every Update.
		// NOTE: I tried a Command and then RPC call for SetHealthUI. It was actually working but was updating the UI before health was synced and was displaying the old value of health.
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

		Debug.Log("Calling OnDeath");

		Debug.Assert(!this.dead);
		Debug.Assert(!this.explosion.IsExploding());
		Debug.Assert(this.gameObject.activeSelf);

		this.dead = true;

		this.explosion.ExplodeAt(this.transform.position);
		this.gameObject.SetActive(false);
	}


	[Command]
	void CmdOnDeath() => this.RpcOnDeath();


	[ClientRpc]
	void RpcOnDeath() => this.OnDeath();
}