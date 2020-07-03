 using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.UI;


[System.Obsolete]
public class JoinMenuController : Menu
{
	public Button joinMatchButtonPrefab;
	public Dictionary<string, Button> joinMatchButtons = new Dictionary<string, Button>();

	public GameObject joinMatchMenu;
	public GameObject matchGuestMenu;


	protected override void Awake() {
		base.Awake();

		this.joinMatchMenu.SetActive(false);
		this.matchGuestMenu.SetActive(false);

		this.CurrentMenu = this.joinMatchMenu;
	}

	private void Update() {
		_ = this.lobbyManager.matchMaker.ListMatches(0, 20, "", filterOutPrivateMatchesFromResults: false, 0, 0, this.lobbyManager.OnMatchList);
		DisplayMatches();
	}

	public void DisplayMatches() {
		var destroyedMatchesKeys = new List<string>();
		foreach (var button in joinMatchButtons)
			if (!lobbyManager.matches.Exists((match) => button.Key == match.name))
				destroyedMatchesKeys.Add(button.Key);

		foreach (var key in destroyedMatchesKeys) {
			Destroy(joinMatchButtons[key].gameObject);
			joinMatchButtons.Remove(key);
		}

		foreach (MatchInfoSnapshot match in lobbyManager.matches)
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
		this.lobbyManager.matchName = match.name;
		_ = this.lobbyManager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, this.lobbyManager.OnMatchJoined);

		this.CurrentMenu = this.matchGuestMenu;
	}
}