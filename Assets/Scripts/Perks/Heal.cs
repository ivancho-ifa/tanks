using UnityEngine;
using UnityEngine.Networking;

[System.Obsolete]
public class Heal : NetworkBehaviour
{
	private void OnTriggerEnter(Collider other) {
		TankHealth targetHealth = other.GetComponent<TankHealth>();
		
		if (targetHealth != null) {
			targetHealth.RefillHealth();
			Destroy(this.gameObject);
		}
	}
}
