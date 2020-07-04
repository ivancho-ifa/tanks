using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


[Obsolete]
public class PerksManager : NetworkBehaviour
{
	public GameObject[] perkPrefabs = new GameObject[1];


	private void Awake() {
		this.InvokeRepeating("SpawnRandomPerk", 10f, 10f);
	}

	void SpawnRandomPerk() {
		if (this.isServer) {
			int randomPerkId = UnityEngine.Random.Range(0, this.perkPrefabs.Length - 1);
			GameObject perk = this.Spawn(this.perkPrefabs[randomPerkId]);
			
			_ = this.StartCoroutine(this.DestroyPerk(perk, 5f));
		}
	}


	GameObject Spawn(GameObject prefab) {
		GameObject perk = Instantiate(prefab, position: this.GetRandomPositionFor(prefab), rotation: prefab.transform.rotation);
		NetworkServer.Spawn(perk);
		return perk;
	}


	Vector3 GetRandomPositionFor(GameObject prefab) {
		GameManager gameManager = this.GetComponent<GameManager>();
		Vector3 position0 = gameManager.tanks[0].transform.position;
		Vector3 position1 = gameManager.tanks[1].transform.position;

		return new Vector3 {
			x = UnityEngine.Random.Range(Math.Min(position0.x, position1.x), Math.Max(position0.x, position1.x)),
			y = prefab.transform.position.y,
			z = UnityEngine.Random.Range(Math.Min(position0.z, position1.z), Math.Max(position0.z, position1.z))
		};
	}

	IEnumerator DestroyPerk(GameObject perk, float delayTime) {
		yield return new WaitForSeconds(delayTime);

		Destroy(perk);
	}
}
