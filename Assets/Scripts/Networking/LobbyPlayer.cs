using System;
using UnityEngine.Networking;
using UnityEngine.UI;


[Obsolete]
public class LobbyPlayer : NetworkLobbyPlayer
{
	public Toggle playerReadyPrefab;

	[SyncVar] bool isOn;
	private Toggle playerReady;
	private LobbyManager lobbyManager;
	private Text playerReadyLabel;


	private void Awake() {
		lobbyManager = NetworkManager.singleton as LobbyManager;
		
		playerReady = Instantiate(playerReadyPrefab, parent: this.lobbyManager.currentMenu.transform);
		playerReady.isOn = isOn = false;
		playerReady.onValueChanged.AddListener(TogglePlayerReady);

		playerReadyLabel = playerReady.GetComponentInChildren<Text>();
	}

	private void Update() {
		if (!this.isLocalPlayer)
			playerReady.interactable = false;

		playerReady.isOn = this.isOn;
		playerReadyLabel.text = string.Format("Player {0} ready", this.netId.Value);
	}


	private void OnDestroy() {
		Destroy(playerReady.gameObject);
	}


	private void TogglePlayerReady(bool isOn) {
		this.isOn = isOn;

		if (this.isOn)
			SendReadyToBeginMessage();
		else
			SendNotReadyToBeginMessage();
	}
}
