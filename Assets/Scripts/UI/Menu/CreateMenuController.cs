using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CreateMenuController : Menu {
	private InputField matchName;
	private NetworkManager manager;

	public GameObject createMatchMenu;
	public GameObject matchHostMenu;


	private void Awake() { 
		this.matchName = this.GetComponentInChildren<InputField>();
		this.manager = NetworkManager.singleton;

		this.createMatchMenu.SetActive(false);
		this.matchHostMenu.SetActive(false);

		this.CurrentMenu = this.createMatchMenu;
	}


	public void CreateButton() {
		_ = manager.matchMaker.CreateMatch(this.matchName.text, manager.matchSize, matchAdvertise: true, "", "", "", 0, 0, manager.OnMatchCreate);

		this.CurrentMenu = this.matchHostMenu;
	}
}