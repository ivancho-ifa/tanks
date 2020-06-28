using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
	private Quaternion relativeRotation;


	private void Start() => this.relativeRotation = this.transform.parent.localRotation;

	private void Update() => this.transform.rotation = this.relativeRotation;
}
