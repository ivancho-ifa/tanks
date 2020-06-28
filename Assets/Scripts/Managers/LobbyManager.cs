using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager : NetworkLobbyManager
{
	public override void OnLobbyClientAddPlayerFailed() {
		Debug.Log("LobbyManager.OnLobbyClientAddPlayerFailed");

		base.OnLobbyClientAddPlayerFailed();
	}

	public override void OnLobbyClientConnect(NetworkConnection conn) {
		Debug.Log("LobbyManager.OnLobbyClientConnect");

		base.OnLobbyClientConnect(conn);
	}

	public override void OnLobbyClientDisconnect(NetworkConnection conn) {
		Debug.Log("LobbyManager.OnLobbyClientDisconnect");

		base.OnLobbyClientDisconnect(conn); 
	}

	public override void OnLobbyClientEnter() {
		Debug.Log("LobbyManager.OnLobbyClientEnter");

		base.OnLobbyClientEnter();
	}

	public override void OnLobbyClientExit() {
		Debug.Log("LobbyManager.OnLobbyClientExit");

		base.OnLobbyClientExit();
	}

	public override void OnLobbyClientSceneChanged(NetworkConnection conn) {
		Debug.Log("LobbyManager.OnLobbyClientSceneChanged");

		base.OnLobbyClientSceneChanged(conn);
	}

	public override void OnLobbyServerConnect(NetworkConnection conn) {
		Debug.Log("LobbyManager.OnLobbyServerConnect");

		base.OnLobbyServerConnect(conn);
	}

	public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn, short playerControllerId) {
		Debug.Log("LobbyManager.OnLobbyServerCreateGamePlayer");

		return base.OnLobbyServerCreateGamePlayer(conn, playerControllerId);
	}

	public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn, short playerControllerId) {
		Debug.Log("LobbyManager.OnLobbyServerCreateLobbyPlayer");

		return base.OnLobbyServerCreateLobbyPlayer(conn, playerControllerId);
	}

	public override void OnLobbyServerDisconnect(NetworkConnection conn) {
		Debug.Log("LobbyManager.OnLobbyServerDisconnect");

		base.OnLobbyServerDisconnect(conn);
	}

	public override void OnLobbyServerPlayerRemoved(NetworkConnection conn, short playerControllerId) {
		Debug.Log("LobbyManager.OnLobbyServerPlayerRemoved");

		base.OnLobbyServerPlayerRemoved(conn, playerControllerId);
	}

	// NOTE: This method should call its base.
	public override void OnLobbyServerPlayersReady() {
		Debug.Log("LobbyManager.OnLobbyServerPlayersReady");

		base.OnLobbyServerPlayersReady();
	}

	public override void OnLobbyServerSceneChanged(string sceneName) {
		Debug.Log("LobbyManager.OnLobbyServerSceneChanged");

		base.OnLobbyServerSceneChanged(sceneName);
	}

	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer) {
		Debug.Log("LobbyManager.OnLobbyServerSceneLoadedForPlayer");

		NetworkLobbyPlayer lobbyPlayerComponent = lobbyPlayer.GetComponent<NetworkLobbyPlayer>();
		TankManager tankManager = gamePlayer.GetComponent<TankManager>();

		tankManager.playerNumber = lobbyPlayerComponent.netId.Value;
		tankManager.playerColor = tankManager.playerNumber % 2 == 0 ? Color.red : Color.green;

		return true;
	}

	public override void OnLobbyStartClient(NetworkClient lobbyClient) {
		Debug.Log("LobbyManager.OnLobbyStartClient");

		base.OnLobbyStartClient(lobbyClient);
	}

	public override void OnLobbyStartHost() {
		Debug.Log("LobbyManager.OnLobbyStartHost");

		base.OnLobbyStartHost();
	}

	public override void OnLobbyStartServer() {
		Debug.Log("LobbyManager.OnLobbyStartServer");

		base.OnLobbyStartServer();
	}

	public override void OnLobbyStopClient() {
		Debug.Log("LobbyManager.OnLobbyStopClient");

		base.OnLobbyStopClient();
	}

	public override void OnLobbyStopHost() {
		Debug.Log("LobbyManager.OnLobbyStopHost");

		base.OnLobbyStopHost();
	}
}