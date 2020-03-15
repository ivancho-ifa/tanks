using System;
using UnityEngine;
using UnityEngine.UI;

public class TankShooting
{
	public class ShootingAudio : Audio
	{
		public ShootingAudio
			(AudioSource source, params AudioClip[] sounds) : base(source, sounds) {
			if (sounds.Length != 2)
				throw new ArgumentException("ShootingAudio requires 2 sounds!");
		}

		public enum SoundID
		{
			Charging,
			Fire
		}

		public void ChangeCurrentSound(SoundID soundID) {
			this.ChangeCurrentSound((int)soundID);
			this.PlayCurrentSound();
		}

		public bool IsCurrentSound(SoundID soundID) => this.IsCurrentSound((int)soundID);
	}


	public uint playerNumber = 1;
	public Rigidbody shell;
	public Transform fireTransform;
	public Slider aimSlider;
	public float minLaunchForce = 15f;
	public float maxLaunchForce = 30f;
	public float maxChargeTime = 0.75f;


	private readonly GameObject tank;
	private readonly ShootingAudio shootingAudio;
	private string fireButton;
	private float currentLaunchForce;
	private float chargeSpeed;
	private bool fired;


	public TankShooting(GameObject tank, ShootingAudio shootingAudio, Slider aimSlider, Rigidbody shell, Transform shellSpawnPoint) {
		this.tank = tank;
		this.shootingAudio = shootingAudio;
		this.aimSlider = aimSlider;
		this.shell = shell;
		this.fireTransform = shellSpawnPoint;
	}

	public void Start() {
		this.fireButton = "Fire" + this.playerNumber;

		this.chargeSpeed = (this.maxLaunchForce - this.minLaunchForce) / this.maxChargeTime;
	}


	public void OnEnable() {
		this.currentLaunchForce = this.minLaunchForce;
		this.aimSlider.value = this.minLaunchForce;
	}


	public void Update() {
		// Track the current state of the fire button and make decisions based on the current launch force.

		this.aimSlider.value = this.minLaunchForce;

		if (this.currentLaunchForce >= this.maxLaunchForce && !this.fired) {
			this.currentLaunchForce = this.maxLaunchForce;
			this.Fire();
		}
		else if (this.InputBeginShot()) {
			this.fired = false;
			this.currentLaunchForce = this.minLaunchForce;

			this.shootingAudio.ChangeCurrentSound(ShootingAudio.SoundID.Charging);
		}
		else if (this.InputChargingShot() && !this.fired) {
			this.currentLaunchForce += this.chargeSpeed * Time.deltaTime;
			this.aimSlider.value = this.currentLaunchForce;
		}
		else if (this.InputEndShot() && !this.fired) {
			this.Fire();
		}
	}


	private void Fire() {
		// Instantiate and launch the shell.

		this.fired = true;

		Rigidbody shellInstance = UnityEngine.Object.Instantiate(this.shell, this.fireTransform.position, this.fireTransform.rotation);
		shellInstance.velocity = this.currentLaunchForce * this.fireTransform.forward;

		this.shootingAudio.ChangeCurrentSound(ShootingAudio.SoundID.Fire);

		this.currentLaunchForce = this.minLaunchForce;
	}


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
		for (int i = 0; i < Input.touchCount; ++i) {
			Touch touch = Input.GetTouch(i);

			if (touch.position.x > Screen.width / 2)
				return touch;
		}

		return null;
	}
#elif UNITY_WEBPLAYER || UNITY_STANDALONE
	private bool InputBeginShot() => Input.GetButtonDown(this.fireButton);

	private bool InputChargingShot() => Input.GetButton(this.fireButton);

	private bool InputEndShot() => Input.GetButtonUp(this.fireButton);
#endif

}