using System.Collections.Generic;
using UnityEngine;

public class HoldTouchControllers : BaseTouchControllers
{
	public virtual int GetPriority()
	{
		return Priority;
	}
	public virtual bool AbortOtherTouches()
	{
		return isAbortOtherTouches;
	}
	public override TouchControllersState HandlerFindTouch( TouchGroup touches, float touchOneStateMoveTime = 0.0f, float touchOneStateTime = 0.0f, CheckingRayIntoObject checkingRay = null )
	{
		Touch[] touchOne = touches.touchOne;
		Touch[] touchTwo = touches.touchTwo;
		Touch[] touchTree = touches.touchTree;
		if( touchOne != null )
		{
			if( touchOne[0].phase == TouchPhase.Stationary )
			{
				return TouchControllersState.TCS_Action;
			}
		}

		return TouchControllersState.TCS_None;
	}


	public override void HandlerActionTouch( TouchControllersState state, TouchGroup touches, float touchOneStateMoveTime = 0.0f, float touchOneStateTime = 0.0f, CheckingRayIntoObject checkingRay = null )
	{
		Touch[] touchOne = touches.touchOne;
		Touch[] touchTwo = touches.touchTwo;
		Touch[] touchTree = touches.touchTree;
		Vector3 point1 = Vector3.zero;
		if( touchOne != null )
		{
			point1 = touchOne[0].position;
			HoldEvent( state, point1, touchOneStateTime, checkingRay( point1 ) );
		}
	}

	private void HoldEvent( TouchControllersState stateT, Vector3 point1, float EventTime, bool isHoldOnDragObject )
	{
		/*
		    Событие Зашатия пальца на экране, не имеет состояний срабатывает зажатии пальца в одном месте, без учета пальцев попавших в элементы UI.
		    isHoldOnDragObject - параметер определяюший произходил ли там над Drag обьектом
		*/
#if DEBUG_LOG
		Log.Write( "HoldEvent p1:" + point1.ToString() + " EventTime:" + EventTime.ToString() + " isDragObject:" + isHoldOnDragObject.ToString() );
#endif
		HoldModel tapModel = new HoldModel()
		{
			stateT = stateT,
			point1 = point1,
			EventTime = EventTime,
			isHoldOnDragObject = isHoldOnDragObject
		};
		MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Hold, tapModel );
	}

	public override void BreakTouch( TouchGroup touches, float touchOneStateMoveTime = 0.0f, float touchOneStateTime = 0.0f, CheckingRayIntoObject checkingRay = null )
	{

	}
}