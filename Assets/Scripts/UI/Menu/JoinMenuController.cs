using UnityEngine;
using UnityEngine.Networking;

public class JoinMenuController : MonoBehaviour
{
	public void JoinButton() {
		NetworkManager manager = NetworkManager.singleton;

		_ = manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, matchAdvertise: true, "", "", "", 0, 0, manager.OnMatchCreate);
	}
}