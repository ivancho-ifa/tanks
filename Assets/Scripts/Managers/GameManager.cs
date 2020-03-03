﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public int numRoundsToWin = 5;
	public float startDelay = 3f;
	public float endDelay = 3f;
	public CameraControl cameraControl;
	public Text messageText;
	public GameObject tankPrefab;
	public TankManager[] tanks;


	private int roundNumber;
	private WaitForSeconds startWait;
	private WaitForSeconds endWait;
	private TankManager roundWinner;
	private TankManager gameWinner;

	private void Start() {
		this.startWait = new WaitForSeconds(this.startDelay);
		this.endWait = new WaitForSeconds(this.endDelay);

		// this.SpawnAllTanks();
		this.SetCameraTargets();

		this.StartCoroutine(this.GameLoop());
	}


	private void SpawnAllTanks() {
		for (int i = 0; i < this.tanks.Length; i++) {
			this.tanks[i].instance =
				Instantiate(this.tankPrefab, this.tanks[i].spawnPoint.position, this.tanks[i].spawnPoint.rotation);
			this.tanks[i].playerNumber = i + 1;
			this.tanks[i].Setup();
		}
	}


	private void SetCameraTargets() {
		var targets = new Transform[this.tanks.Length];

		for (int i = 0; i < targets.Length; i++)
			targets[i] = this.tanks[i].instance.transform;

		this.cameraControl.targets = targets;
	}


	private IEnumerator GameLoop() {
		yield return this.StartCoroutine(this.RoundStarting());
		yield return this.StartCoroutine(this.RoundPlaying());
		yield return this.StartCoroutine(this.RoundEnding());

		if (this.gameWinner != null)
			SceneManager.LoadScene(0);
		else
			this.StartCoroutine(this.GameLoop());
	}


	private IEnumerator RoundStarting() {
		this.ResetAllTanks();
		this.DisableTankControl();

		this.cameraControl.SetPositionAndSize();

		++this.roundNumber;
		this.messageText.text = "ROUND " + this.roundNumber;

		yield return this.startWait;
	}


	private IEnumerator RoundPlaying() {
		this.EnableTankControl();

		this.messageText.text = string.Empty;

		while (!this.OneTankLeft()) {
			yield return null;
		}
	}


	private IEnumerator RoundEnding() {
		this.DisableTankControl();
		
		this.roundWinner = this.GetRoundWinner();
		if (this.roundWinner != null)
			++this.roundWinner.wins;

		this.gameWinner = this.GetGameWinner();

		this.messageText.text = this.EndMessage();

		yield return this.endWait;
	}


	private bool OneTankLeft() {
		int numTanksLeft = 0;

		foreach (TankManager tank in this.tanks)
			if (tank.instance.activeSelf)
				numTanksLeft++;

		return numTanksLeft <= 1;
	}


	private TankManager GetRoundWinner() {
		foreach (TankManager tank in this.tanks)
			if (tank.instance.activeSelf)
				return tank;

		return null;
	}


	private TankManager GetGameWinner() {
		foreach (TankManager tank in this.tanks)
			if (tank.wins == this.numRoundsToWin)
				return tank;

		return null;
	}


	private string EndMessage() {
		string message = "DRAW!";

		if (this.roundWinner != null)
			message = this.roundWinner.coloredPlayerText + " WINS THE ROUND!";

		message += "\n\n\n\n";

		foreach (TankManager tank in this.tanks)
			message += tank.coloredPlayerText + ": " + tank.wins + " WINS\n";

		if (this.gameWinner != null)
			message = this.gameWinner.coloredPlayerText + " WINS THE GAME!";

		return message;
	}

	private void ResetAllTanks() {
		foreach (TankManager tank in this.tanks)
			tank.Reset();
	}


	private void EnableTankControl() {
		foreach (TankManager tank in this.tanks)
			tank.EnableControl();
	}


	private void DisableTankControl() {
		foreach (TankManager tank in this.tanks)
			tank.DisableControl();
	}
}