using System.Collections.Generic;
using UnityEngine;

public class TapTouchControllers : BaseTouchControllers
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
			if( ( touchOne[2].phase == TouchPhase.Began && touchOne[1].phase == TouchPhase.Stationary && touchOne[0].phase == TouchPhase.Ended ) ||
					( touchOne[1].phase == TouchPhase.Began && touchOne[0].phase == TouchPhase.Ended ) ||
					( touchOne[2].phase == TouchPhase.Began && touchOne[1].phase == TouchPhase.Moved && touchOne[0].phase == TouchPhase.Ended && touchOneStateMoveTime < 0.15f ) )
			{
				touchOneStateMoveTime = 10.0f;
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
		int tapCount = 0;
		if( touchOne != null )
		{
			point1 = touchOne[0].position;
			tapCount = touchOne[0].tapCount;
			TapEvent( state, point1, tapCount, checkingRay( point1 ) );
		}
	}

	private static int tapGlobalCount = 0;

	private void TapEvent( TouchControllersState stateT, Vector3 point1, int tapCount, bool isTapOnDragObject )
	{
		tapGlobalCount++;
		/*
		    —обытие тапа по экрану, не имеет состо€ний срабатывает при отпускании тапа, без учета пальцев попавших в элементы UI.
		    isTapOnDragObject - параметер определ€юший произходил ли там над Drag обьектом
		*/
#if DEBUG_LOG
		Log.Write( "TapEvent p1:" + point1.ToString() + " tapCount:" + tapCount.ToString() + " isDragObject:" + isTapOnDragObject.ToString() );
#endif
		TapModel tapModel = new TapModel()
		{
			stateT = stateT,
			point1 = point1,
			tapCount = tapCount,
			isTapOnDragObject = isTapOnDragObject,
			tapGlobalCount = tapGlobalCount
		};
		MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Tap, tapModel );
	}
	public override void BreakTouch( TouchGroup touches, float touchOneStateMoveTime = 0.0f, float touchOneStateTime = 0.0f, CheckingRayIntoObject checkingRay = null )
	{

	}

}