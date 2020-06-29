using UnityEngine;
using UnityEngine.Networking;

public class MatchMakerMenuController : Menu {
	public GameObject createMatchMenu;
	public GameObject joinMatchMenu;
	public GameObject matchMakerMenu;


	void Awake() {
		Debug.Log("Awake");

		this.createMatchMenu.SetActive(false);
		this.joinMatchMenu.SetActive(false);
		this.matchMakerMenu.SetActive(false);

		this.CurrentMenu = this.matchMakerMenu;
	}


	public void CreateMatchButton() {
		NetworkManager.singleton.StartMatchMaker();
		this.CurrentMenu = this.createMatchMenu;
	}


	public void JoinMatchButton() {
		NetworkManager.singleton.StartMatchMaker();
		this.CurrentMenu = this.joinMatchMenu;
	}
}
