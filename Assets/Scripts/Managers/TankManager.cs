using System;
using UnityEngine;

[Serializable]
public class TankManager
{
	public Color playerColor;
	public Transform spawnPoint;
	[HideInInspector] public int playerNumber;
	[HideInInspector] public string coloredPlayerText;
	[HideInInspector] public GameObject instance;
	[HideInInspector] public int wins;


	private TankMovement movement;
	private TankShooting shooting;
	private GameObject canvasGameObject;


	public void Setup() {
		this.movement = this.instance.GetComponent<TankMovement>();
		this.shooting = this.instance.GetComponent<TankShooting>();
		this.canvasGameObject = this.instance.GetComponentInChildren<Canvas>().gameObject;

		this.movement.playerNumber = this.playerNumber;
		this.shooting.playerNumber = this.playerNumber;

		this.coloredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(this.playerColor) + ">PLAYER " + this.playerNumber + "</color>";

		MeshRenderer[] renderers = this.instance.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer renderer in renderers)
			renderer.material.color = this.playerColor;
	}


	public void DisableControl() {
		this.movement.enabled = false;
		this.shooting.enabled = false;

		this.canvasGameObject.SetActive(false);
	}


	public void EnableControl() {
		this.movement.enabled = true;
		this.shooting.enabled = true;

		this.canvasGameObject.SetActive(true);
	}


	public void Reset() {
		this.instance.transform.position = this.spawnPoint.position;
		this.instance.transform.rotation = this.spawnPoint.rotation;

		this.instance.SetActive(false);
		this.instance.SetActive(true);
	}
}
