using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MatchHostMenu : MonoBehaviour
{
	private NetworkManager manager;


	private void Awake() {
		this.manager = NetworkManager.singleton;
	}


	void StartMatchButton() {
		
	}
}
