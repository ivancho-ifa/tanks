using UnityEngine;
using UnityEngine.Networking;

public class Menu : MonoBehaviour
{
	protected LobbyManager lobbyManager;

	protected virtual void Awake() {
		lobbyManager = NetworkManager.singleton as LobbyManager;
	}


	private GameObject currentMenu;
	protected GameObject CurrentMenu {
		get => this.currentMenu;
		set {
			if (this.currentMenu)
				this.currentMenu.SetActive(false);

			this.currentMenu = value;
			this.currentMenu.SetActive(true);
			
			this.lobbyManager.currentMenu = this.currentMenu;
		}
	}
}
