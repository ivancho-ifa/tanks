public struct Input
{
	public Input(string axisName, float maxValue) {
		this.axisName = axisName;
		this.value = 0f;
		this.maxValue = maxValue;
	}

	public readonly string axisName;
	public readonly float maxValue;
	public float value;
}