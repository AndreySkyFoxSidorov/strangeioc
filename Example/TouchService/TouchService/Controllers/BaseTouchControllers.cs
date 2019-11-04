using System.Collections.Generic;
using UnityEngine;

public enum TouchControllersState
{
	TCS_None = 0,
	TCS_Start,
	TCS_Action,
	TCS_End
}


public class BaseTouchControllers
{
	public delegate bool CheckingRayIntoObject( Vector3 ScreenPoint );

	public int Priority =0;
	public bool isAbortOtherTouches = false;

	public virtual int GetPriority()
{
	return Priority;
}
public virtual bool AbortOtherTouches()
{
	return isAbortOtherTouches;
}

public virtual TouchControllersState HandlerFindTouch( TouchGroup touches, float touchOneStateMoveTime=0.0f, float touchOneStateTime = 0.0f, CheckingRayIntoObject checkingRay = null )
{
	return TouchControllersState.TCS_None;
}

public virtual void HandlerActionTouch( TouchControllersState state, TouchGroup touches, float touchOneStateMoveTime = 0.0f, float touchOneStateTime = 0.0f, CheckingRayIntoObject checkingRay=null )
{

}

public virtual void BreakTouch( TouchGroup touches, float touchOneStateMoveTime = 0.0f, float touchOneStateTime = 0.0f, CheckingRayIntoObject checkingRay = null )
{

}

}