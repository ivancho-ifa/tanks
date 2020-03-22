using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TankManager : NetworkBehaviour
{
	[SyncVar] [HideInInspector] public uint playerNumber;
	[SyncVar] [HideInInspector] public Color playerColor;

	[HideInInspector] public string coloredPlayerText;
	[HideInInspector] public int wins;


	[Header("Tank Health")]
	public GameObject explosionPrefab;
	public Image fillImage;
	public Slider slider;

	[Header("Tank Movement")]
	public AudioSource engineAudio;
	public AudioClip engineIdling;
	public AudioClip engineDriving;

	[Header("Tank Shooting")]
	public Rigidbody shell;
	public Transform fireTransform;
	public Slider aimSlider;
	public AudioSource shootingAudio;
	public AudioClip chargingClip;
	public AudioClip fireClip;


	private GameObject canvasGameObject;

	private TankMovement movement;
	private TankShooting shooting;
	public TankHealth health;


	public void Awake() {
		this.health = new TankHealth(this.gameObject, this.fillImage, this.slider);
		this.movement = new TankMovement(this.gameObject, new TankMovement.EngineAudio(this.engineAudio, this.engineIdling, this.engineDriving));
		this.shooting = new TankShooting(this.gameObject, new TankShooting.ShootingAudio(this.shootingAudio, this.chargingClip, this.fireClip), this.aimSlider, this.shell, this.fireTransform);
		this.canvasGameObject = this.gameObject.GetComponentInChildren<Canvas>().gameObject;

		this.health.SetupExplosion(this.explosionPrefab);
		this.movement.Awake();
	}


	public void FixedUpdate() => this.movement.FixedUpdate();


	public void OnDisable() => this.movement.OnDisable();


	public void OnEnable() {
		this.health.OnEnable();
		this.movement.OnEnable();
		this.shooting.OnEnable();
	}


	public void Reset() {
		this.gameObject.SetActive(false);
		this.gameObject.SetActive(true);
	}


	public void Start() => this.shooting.Start();


	public void Update() {
		if (this.isLocalPlayer) {
			this.movement.Update();
			this.shooting.Update();
		}
	}


	public void Setup() {
		this.movement.playerNumber = this.playerNumber;
		this.shooting.playerNumber = this.playerNumber;

		this.playerColor = this.playerNumber % 2 == 0 ? Color.green : Color.red;

		this.coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(this.playerColor) + ">PLAYER " + this.playerNumber + "</color>";

		MeshRenderer[] renderers = this.gameObject.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer renderer in renderers)
			renderer.material.color = this.playerColor;
	}


	public void DisableControl() {
		this.enabled = false;

		this.canvasGameObject.SetActive(false);
	}


	public void EnableControl() {
		this.enabled = true;

		this.canvasGameObject.SetActive(true);
	}
}
