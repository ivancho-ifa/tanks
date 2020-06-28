using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


[System.Obsolete]
public class TankShooting : NetworkBehaviour
{
	public Rigidbody shell;
	public Transform shellSpawnPoint;
	public Slider aimUI;
	public ShootingAudio shootingAudio;

	readonly float chargeSpeed;
	readonly float maxChargeTime;
#if UNITY_WEBPLAYER || UNITY_STANDALONE
	readonly string fireButton;
#endif

	ClampedValue<float> launchForce;
	bool fired;

	 
	public TankShooting() {
		this.launchForce = new ClampedValue<float>(15f, 30f);
		this.maxChargeTime = .75f;
		this.fireButton = "Fire";
		this.chargeSpeed = (this.launchForce.max - this.launchForce.min) / this.maxChargeTime;
	}


	public void OnEnable() {
		this.launchForce.Value = this.launchForce.min;
		this.aimUI.value = this.launchForce.min;
	}


	public void Update() {
		// Track the current state of the fire button and make decisions based on the current launch force.

		if (this.isLocalPlayer) {
			this.aimUI.value = this.launchForce.min;

			if (this.launchForce.Value >= this.launchForce.max && !this.fired) {
				this.launchForce.Value = this.launchForce.max;
				this.CmdFire();
			}
			else if (this.InputBeginShot()) {
				this.fired = false;
				this.launchForce.Value = this.launchForce.min;

				this.shootingAudio.ChangeCurrentSound(this.shootingAudio.shotCharging);
			}
			else if (this.InputChargingShot() && !this.fired) {
				this.launchForce.Value += this.chargeSpeed * Time.deltaTime;
				this.aimUI.value = this.launchForce.Value;
			}
			else if (this.InputEndShot() && !this.fired) {
				this.CmdFire();
			}
		}
	}


	void Fire() {
		// Instantiate and launch the shell.

		this.fired = true;

		Rigidbody shellInstance = Instantiate(this.shell, this.shellSpawnPoint.position, this.shellSpawnPoint.rotation);
		shellInstance.velocity = this.launchForce.Value * this.shellSpawnPoint.forward;

		this.shootingAudio.ChangeCurrentSound(this.shootingAudio.shotFiring);

		this.launchForce.Value = this.launchForce.min;
	}


	[Command]
	void CmdFire() => this.RpcFire();


	[ClientRpc]
	void RpcFire() => this.Fire();


#if UNITY_ANDROID || UNITY_IOS
	bool InputBeginShot() {
		Touch? touch = this.GetTouch();

		return !touch.HasValue ? false : 
			touch.Value.phase == TouchPhase.Began;
	}


	bool InputChargingShot() {
		Touch? touch = this.GetTouch();

		return !touch.HasValue ? false :
			touch.Value.phase == TouchPhase.Stationary || touch.Value.phase == TouchPhase.Moved;
	}


	bool InputEndShot() {
		Touch? touch = this.GetTouch();

		return !touch.HasValue ? false :
			touch.Value.phase == TouchPhase.Ended || touch.Value.phase == TouchPhase.Canceled;
	}

	Touch? GetTouch() {
		for (int i = 0; i < UnityEngine.Input.touchCount; ++i) {
			Touch touch = UnityEngine.Input.GetTouch(i);

			if (touch.position.x > Screen.width / 2)
				return touch;
		}

		return null;
	}
#elif UNITY_WEBPLAYER || UNITY_STANDALONE
	bool InputBeginShot() => UnityEngine.Input.GetButtonDown(this.fireButton);

	bool InputChargingShot() => UnityEngine.Input.GetButton(this.fireButton);

	bool InputEndShot() => UnityEngine.Input.GetButtonUp(this.fireButton);
#endif
}