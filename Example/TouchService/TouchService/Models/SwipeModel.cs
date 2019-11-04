using UnityEngine;

public class SwipeModel
{
	public TouchControllersState stateT = TouchControllersState.TCS_None;
	public Vector3 point1 = Vector3.zero;
	public Vector3 point1Delta = Vector3.zero;
	public float EventTime = 0.0f;
}