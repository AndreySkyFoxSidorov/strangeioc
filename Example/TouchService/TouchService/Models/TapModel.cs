using UnityEngine;


public class TapModel
{
	public TouchControllersState stateT = TouchControllersState.TCS_None;
	public Vector3 point1;
	public int tapCount;
	public int tapGlobalCount;
	public bool isTapOnDragObject;
}