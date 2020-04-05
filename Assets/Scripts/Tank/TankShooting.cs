﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TankShooting : NetworkBehaviour
{
	public uint playerNumber = 1;
	public float minLaunchForce = 15f;
	public float maxLaunchForce = 30f;
	public float maxChargeTime = 0.75f;

	public Rigidbody shell;
	public Transform fireTransform;
	public Slider aimSlider;
	public ShootingAudio shootingAudio;

	private float currentLaunchForce;
	private float chargeSpeed;
	private bool fired;
#if UNITY_WEBPLAYER || UNITY_STANDALONE
	private string fireButton;
#endif


	public void Start() => this.chargeSpeed = (this.maxLaunchForce - this.minLaunchForce) / this.maxChargeTime;


	public void OnEnable() {
		this.currentLaunchForce = this.minLaunchForce;
		this.aimSlider.value = this.minLaunchForce;
	}


	public void Update() {
		// Track the current state of the fire button and make decisions based on the current launch force.

		if (this.isLocalPlayer) {
			this.aimSlider.value = this.minLaunchForce;

			if (this.currentLaunchForce >= this.maxLaunchForce && !this.fired) {
				this.currentLaunchForce = this.maxLaunchForce;
				this.CmdFire();
			}
			else if (this.InputBeginShot()) {
				this.fired = false;
				this.currentLaunchForce = this.minLaunchForce;

				this.shootingAudio.ChangeCurrentSound(this.shootingAudio.shotCharging);
			}
			else if (this.InputChargingShot() && !this.fired) {
				this.currentLaunchForce += this.chargeSpeed * Time.deltaTime;
				this.aimSlider.value = this.currentLaunchForce;
			}
			else if (this.InputEndShot() && !this.fired) {
				this.CmdFire();
			}
		}
	}


	private void Fire() {
		// Instantiate and launch the shell.

		this.fired = true;

		Rigidbody shellInstance = Instantiate(this.shell, this.fireTransform.position, this.fireTransform.rotation);
		shellInstance.velocity = this.currentLaunchForce * this.fireTransform.forward;

		this.shootingAudio.ChangeCurrentSound(this.shootingAudio.shotFiring);

		this.currentLaunchForce = this.minLaunchForce;
	}


	[Command]
	private void CmdFire() => this.RpcFire();


	[ClientRpc]
	private void RpcFire() => this.Fire();


#if UNITY_ANDROID || UNITY_IOS
	private bool InputBeginShot() {
		Touch? touch = this.GetTouch();

		return !touch.HasValue ? false : 
			touch.Value.phase == TouchPhase.Began;
	}


	private bool InputChargingShot() {
		Touch? touch = this.GetTouch();

		return !touch.HasValue ? false :
			touch.Value.phase == TouchPhase.Stationary || touch.Value.phase == TouchPhase.Moved;
	}


	private bool InputEndShot() {
		Touch? touch = this.GetTouch();

		return !touch.HasValue ? false :
			touch.Value.phase == TouchPhase.Ended || touch.Value.phase == TouchPhase.Canceled;
	}

	private Touch? GetTouch() {
		for (int i = 0; i < UnityEngine.Input.touchCount; ++i) {
			Touch touch = UnityEngine.Input.GetTouch(i);

			if (touch.position.x > Screen.width / 2)
				return touch;
		}

		return null;
	}
#elif UNITY_WEBPLAYER || UNITY_STANDALONE
	public override void OnStartLocalPlayer() => this.fireButton = "Fire" + this.playerNumber;

	private bool InputBeginShot() => UnityEngine.Input.GetButtonDown(this.fireButton);

	private bool InputChargingShot() => UnityEngine.Input.GetButton(this.fireButton);

	private bool InputEndShot() => UnityEngine.Input.GetButtonUp(this.fireButton);
#endif

}