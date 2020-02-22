using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
	public int playerNumber = 1;
	public Rigidbody shell;
	public Transform fireTransform;
	public Slider aimSlider;
	public AudioSource shootingAudio;
	public AudioClip chargingClip;
	public AudioClip fireClip;
	public float minLaunchForce = 15f;
	public float maxLaunchForce = 30f;
	public float maxChargeTime = 0.75f;


	private string fireButton;
	private float currentLaunchForce;
	private float chargeSpeed;
	private bool fired;


	private void Start() {
		this.fireButton = "Fire" + this.playerNumber;

		this.chargeSpeed = (this.maxLaunchForce - this.minLaunchForce) / this.maxChargeTime;
	}


	private void OnEnable() {
		this.currentLaunchForce = this.minLaunchForce;
		this.aimSlider.value = this.minLaunchForce;
	}


	private void Update() {
		// Track the current state of the fire button and make decisions based on the current launch force.

		this.aimSlider.value = this.minLaunchForce;

		if (this.currentLaunchForce >= this.maxLaunchForce && !this.fired) {
			this.currentLaunchForce = this.maxLaunchForce;
			this.Fire();
		}
		else if (this.InputBeginShot()) {
			this.fired = false;
			this.currentLaunchForce = this.minLaunchForce;

			this.shootingAudio.clip = this.chargingClip;
			this.shootingAudio.Play();
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

		Rigidbody shellInstance = Instantiate(this.shell, this.fireTransform.position, this.fireTransform.rotation);
		shellInstance.velocity = this.currentLaunchForce * this.fireTransform.forward;

		this.shootingAudio.clip = this.fireClip;
		this.shootingAudio.Play();

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
	private bool InputBeginShot() {
		return Input.GetButtonDown(this.fireButton);
	}


	private bool InputChargingShot() {
		return Input.GetButton(this.fireButton);
	}


	private bool InputEndShot() {
		return Input.GetButtonUp(this.fireButton);
	}
#endif

}