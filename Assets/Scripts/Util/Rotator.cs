using UnityEngine;

public class Rotator : MonoBehaviour
{
	void Update() {
		// Spin the object around the target at 20 degrees/second.
		this.transform.RotateAround(this.gameObject.transform.position, Vector3.up, 20 * Time.deltaTime);
	}
}
