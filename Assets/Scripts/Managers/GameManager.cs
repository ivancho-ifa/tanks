using System.Collections;
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


	// NOTE: This is here because on Start() not every player is ready.
	// TODO: There HAS to be a better solution to get all the tanks.
	private void FixedUpdate() {
		this.SetSpawnedTanks();
		this.SetupSpawnedTanks();
		this.SetCameraTargets();
	}


	private void Start() {
		this.startWait = new WaitForSeconds(this.startDelay);
		this.endWait = new WaitForSeconds(this.endDelay);

		this.SetSpawnedTanks();
		this.SetupSpawnedTanks();
		this.SetCameraTargets();

		this.StartCoroutine(this.GameLoop());
	}


	private void SetSpawnedTanks() {
		GameObject[] tanks = GameObject.FindGameObjectsWithTag("Player");
		Debug.Log("Player objects found: " + tanks.Length);
		this.tanks = new TankManager[tanks.Length];
		for (int i = 0; i < tanks.Length; ++i)
			this.tanks[i] = tanks[i].GetComponent<TankManager>();
	}

	private void SetupSpawnedTanks() {
		Debug.Log("Tank managers count:  " + this.tanks.Length);
		foreach (TankManager tank in this.tanks)
			tank.Setup();
	}

	private void SetCameraTargets() {
		this.cameraControl.targets = new Transform[this.tanks.Length];
		for (int i = 0; i < this.tanks.Length; i++)
			this.cameraControl.targets[i] = this.tanks[i].gameObject.transform;
	}


	private IEnumerator GameLoop() {
		yield return this.StartCoroutine(this.RoundStarting());
		yield return this.StartCoroutine(this.RoundPlaying());
		//yield return this.StartCoroutine(this.RoundEnding());

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

		//while (!this.OneTankLeft())
		while (true)
			yield return null;
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
			if (tank.gameObject.activeSelf)
				numTanksLeft++;

		return numTanksLeft <= 1;
	}


	private TankManager GetRoundWinner() {
		foreach (TankManager tank in this.tanks)
			if (tank.gameObject.activeSelf)
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