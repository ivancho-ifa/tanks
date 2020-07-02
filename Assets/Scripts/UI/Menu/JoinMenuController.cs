 using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;


[System.Obsolete]
public class JoinMenuController : MonoBehaviour
{
	private NetworkManager manager;

	public Button joinMatchButtonPrefab;
	public Dictionary<string, Button> joinMatchButtons = new Dictionary<string, Button>();

	 
	private void Awake() {
		this.manager = NetworkManager.singleton;
	}

	private void Update() {
		_ = this.manager.matchMaker.ListMatches(0, 20, "", filterOutPrivateMatchesFromResults: false, 0, 0, this.manager.OnMatchList);
		DisplayMatches();
	}

	public void DisplayMatches() {
		var destroyedMatchesKeys = new List<string>();
		foreach (var button in joinMatchButtons)
			if (!manager.matches.Exists((match) => button.Key == match.name))
				destroyedMatchesKeys.Add(button.Key);

		foreach (var key in destroyedMatchesKeys) {
			Destroy(joinMatchButtons[key].gameObject);
			joinMatchButtons.Remove(key);
		}

		foreach (MatchInfoSnapshot match in manager.matches)
			if (!this.joinMatchButtons.ContainsKey(match.name))
				this.DisplayMatch(match);
	}

	public void DisplayMatch(MatchInfoSnapshot match) {
		Button joinMatchButton = Instantiate(this.joinMatchButtonPrefab, parent: this.transform);
		Text buttonLabel = joinMatchButton.GetComponentInChildren<Text>();
		buttonLabel.text = "Join " + match.name;
		joinMatchButton.onClick.AddListener(() => this.JoinButton(match));

		this.joinMatchButtons[match.name] = joinMatchButton;
	}


	public void JoinButton(MatchInfoSnapshot match) {
		this.manager.matchName = match.name;
		_ = this.manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, this.manager.OnMatchJoined);
	}
}