using UnityEngine;
using UnityEngine.UI;

public class CreateMenuController : Menu
{
	private InputField matchName;

	public GameObject createMatchMenu;
	public GameObject matchHostMenu;


	protected override void Awake() {
		base.Awake();

		this.matchName = this.GetComponentInChildren<InputField>();

		this.createMatchMenu.SetActive(false);
		this.matchHostMenu.SetActive(false);

		this.CurrentMenu = this.createMatchMenu;
	}


	public void CreateButton() {
		_ = lobbyManager.matchMaker.CreateMatch(this.matchName.text, lobbyManager.matchSize, matchAdvertise: true, "", "", "", 0, 0, lobbyManager.OnMatchCreate);

		this.CurrentMenu = this.matchHostMenu;
	}
}