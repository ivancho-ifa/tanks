using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class TankManager : NetworkBehaviour
{
	[SyncVar] [HideInInspector] public uint playerNumber;
	[SyncVar] [HideInInspector] public Color playerColor;

	[HideInInspector] public string coloredPlayerText;
	[HideInInspector] public int wins;


	GameObject canvasGameObject;


	public void Awake() => this.canvasGameObject = this.gameObject.GetComponentInChildren<Canvas>().gameObject;


	public override void OnStartLocalPlayer() => this.tag = "Player";


	public void Reset() {
		this.gameObject.SetActive(false);
		this.gameObject.SetActive(true);
	}


	public void Setup() {
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
