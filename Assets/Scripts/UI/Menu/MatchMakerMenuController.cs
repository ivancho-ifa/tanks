using UnityEngine;
using UnityEngine.Networking;

public class MatchMakerMenuController : Menu {
	public GameObject createMatchMenu;
	public GameObject joinMatchMenu;
	public GameObject matchMakerMenu;


	protected override void Awake() {
		base.Awake();

		this.createMatchMenu.SetActive(false);
		this.joinMatchMenu.SetActive(false);
		this.matchMakerMenu.SetActive(false);

		this.CurrentMenu = this.matchMakerMenu;
	}


	public void CreateMatchButton() {
		lobbyManager.StartMatchMaker();
		this.CurrentMenu = this.createMatchMenu;
	}


	public void JoinMatchButton() {
		lobbyManager.StartMatchMaker();
		this.CurrentMenu = this.joinMatchMenu;
	}
}
