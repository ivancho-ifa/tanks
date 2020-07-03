using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class LobbyManager : NetworkLobbyManager
{
	public GameObject currentMenu;


	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) {
		NetworkLobbyPlayer lobbyPlayerComponent = lobbyPlayer.GetComponent<NetworkLobbyPlayer>();
		TankManager tankManager = gamePlayer.GetComponent<TankManager>();

		tankManager.playerNumber = lobbyPlayerComponent.netId.Value;
		tankManager.playerColor = tankManager.playerNumber % 2 == 0 ? Color.red : Color.green;

		return true;
	}
}