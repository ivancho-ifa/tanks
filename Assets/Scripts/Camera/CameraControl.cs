using UnityEngine;

public class CameraControl : MonoBehaviour
{
	public readonly float dampTime = 0.2f;
	public readonly float screenEdgeBuffer = 4f;
	public readonly float minSize = 6.5f;

	[HideInInspector] public Transform[] targets;


	private new Camera camera;
	private float zoomSpeed;
	private Vector3 moveVelocity;
	private Vector3 newPosition;


	public void SetPositionAndSize() {
		this.MoveCamera();
		this.ZoomCamera();
	}


	private void Awake() => this.camera = this.GetComponentInChildren<Camera>();


	/// <remarks>This method is called one more time when a client disconnects and <c>this.targets</c> are all <c>null</c>.</remarks>
	//	TODO: Find a better way to not call this when a client disconnects. Currently I check for <c>null</c> before usage.
	private void FixedUpdate() => this.SetPositionAndSize();


	private void MoveCamera() {
		this.newPosition = this.CalculateTargetsAveragePosition();

		this.transform.position = Vector3.SmoothDamp(this.transform.position, this.newPosition, ref this.moveVelocity, this.dampTime);
	}


	private void ZoomCamera() {
		float requiredSize = this.FindRequiredSize();
		this.camera.orthographicSize = Mathf.SmoothDamp(this.camera.orthographicSize, requiredSize, ref this.zoomSpeed, this.dampTime);
	}


	private Vector3 CalculateTargetsAveragePosition() {
		var averagePosition = new Vector3(0f, this.transform.position.y, 0f);
		int targetsCount = 0;

		foreach (Transform target in this.targets)
			if (target && target.gameObject.activeSelf) {
				averagePosition.x += target.position.x;
				averagePosition.z += target.position.z;

				++targetsCount;
			}

		if (targetsCount > 0)
			averagePosition /= targetsCount;

		return averagePosition;
	}


	private float FindRequiredSize() {
		Vector3 newLocalPosition = this.transform.InverseTransformPoint(this.newPosition);

		float size = 0f;
		foreach (Transform target in this.targets)
			if (target && target.gameObject.activeSelf) {
				Vector3 targetLocalPosition = this.transform.InverseTransformPoint(target.position);
				Vector3 distanceToTarget = targetLocalPosition - newLocalPosition;

				size = Mathf.Max(size, Mathf.Abs(distanceToTarget.y));
				size = Mathf.Max(size, Mathf.Abs(distanceToTarget.x) / this.camera.aspect);
			}

		size += this.screenEdgeBuffer;
		size = Mathf.Max(size, this.minSize);

		return size;
	}
}