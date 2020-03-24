using UnityEngine;
using UnityEngine.Networking;

public class TankManager : NetworkBehaviour
{
	[SyncVar] [HideInInspector] public uint playerNumber;
	[SyncVar] [HideInInspector] public Color playerColor;

	[HideInInspector] public string coloredPlayerText;
	[HideInInspector] public int wins;


	private GameObject canvasGameObject;

	private TankMovement movement;
	private TankShooting shooting;


	public void Awake() {
		this.shooting = this.GetComponent<TankShooting>();
		this.canvasGameObject = this.gameObject.GetComponentInChildren<Canvas>().gameObject;
		
		this.movement = this.GetComponent<TankMovement>();
		this.movement.Awake();
	}


	public override void OnStartLocalPlayer() => this.tag = "Player";


	public void Reset() {
		this.gameObject.SetActive(false);
		this.gameObject.SetActive(true);
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


	public void ActiveControl(bool enabled) {
		this.enabled = enabled;

		this.canvasGameObject.SetActive(enabled);
	}
}
