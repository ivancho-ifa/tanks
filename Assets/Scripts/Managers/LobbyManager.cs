using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

[System.Obsolete]
public class LobbyManager : NetworkLobbyManager
{
	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) {
		NetworkLobbyPlayer lobbyPlayerComponent = lobbyPlayer.GetComponent<NetworkLobbyPlayer>();
		TankManager tankManager = gamePlayer.GetComponent<TankManager>();

		tankManager.playerNumber = lobbyPlayerComponent.netId.Value;
		tankManager.playerColor = tankManager.playerNumber % 2 == 0 ? Color.red : Color.green;

		return true;
	}
}