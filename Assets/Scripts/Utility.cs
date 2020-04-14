using System;
using UnityEngine;

public struct ClampedValue<Type> where Type : IComparable
{
	public readonly Type min;
	public readonly Type max;

	private Type value;
	public Type Value
	{
		get => this.value;
		set => this.value = Clamp(value, this.min, this.max);
	}

	public ClampedValue(Type min, Type max, Type initialValue) {
		if (min.CompareTo(max) > 0)
			throw new ArgumentOutOfRangeException(paramName: "max", max, "max >= min is required");

		this.min = min;
		this.max = max;
		this.value = Clamp(initialValue, this.min, this.max);
	}

	public ClampedValue(Type min, Type max) : this(min, max, initialValue: min) { }

	static Type Clamp(Type value, Type min, Type max) {
		if (value.CompareTo(min) < 0)
			value = min;
		else if (value.CompareTo(max) > 0)
			value = max;
		else
			Debug.Assert(0 <= value.CompareTo(min) && value.CompareTo(max) <= 0);

		return value;
	}
}