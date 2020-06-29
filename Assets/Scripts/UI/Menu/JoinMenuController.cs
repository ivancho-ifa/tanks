using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;

public class JoinMenuController : MonoBehaviour
{
	private NetworkManager manager;

	public Button joinMatchButtonPrefab;
	public Dictionary<string, Button> matchButtons;


	JoinMenuController() {
		matchButtons = new Dictionary<string, Button>();
	}


	private void Awake() {
		this.manager = NetworkManager.singleton;
		this.manager.matchMaker.ListMatches(0, 20, "", filterOutPrivateMatchesFromResults: false, 0, 0, this.OnMatchList);
	}

	private void Update() {
	}

	private void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList) {
		this.matchButtons.Clear();

		if (success) {
			foreach (MatchInfoSnapshot match in matchList) {
				Button joinMatchButton = Instantiate(this.joinMatchButtonPrefab, parent: this.transform);
				Text buttonLabel = joinMatchButton.GetComponentInChildren<Text>();
				buttonLabel.text = "Join " + match.name;
				joinMatchButton.onClick.AddListener(() => this.JoinButton(match));

				this.matchButtons[match.name] = joinMatchButton;
			}
		}
	}


	public void JoinButton(MatchInfoSnapshot match) {
		this.manager.matchName = match.name;
		_ = this.manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, this.manager.OnMatchJoined);
	}
}