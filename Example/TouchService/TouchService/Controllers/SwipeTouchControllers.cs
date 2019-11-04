//#define DEBUG_LOG
using System.Collections.Generic;
using UnityEngine;

public class SwipeTouchControllers : BaseTouchControllers
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
		if( stateT == TouchControllersState.TCS_None )
		{
			if( touchOne != null )
			{
				/*
				Начало свайпа:
				Случай 1: (1 B)(1 S)(1 M)
				Случай 2: (1 B)(1 M)
				*/

				if( ( /*touchOne[2].phase == TouchPhase.Began&&*/ touchOne[1].phase == TouchPhase.Stationary && touchOne[0].phase == TouchPhase.Moved ) ||
						( /*touchOne[2].phase == TouchPhase.Began &&*/ touchOne[0].phase == TouchPhase.Moved ) )
				{
					stateT = TouchControllersState.TCS_Start;
					return stateT;
				}
			}
		}
		else if( stateT == TouchControllersState.TCS_Start )
		{
			stateT = TouchControllersState.TCS_Action;
			return stateT;
		}
		else if( stateT == TouchControllersState.TCS_Action )
		{
			/*
			    Свайп
			    Случай 1: (1 M)
			    Случай 2: (1 S)
			    Конец свайпа:
			    Случай 1: (1 M)(1 E)
			    Случай 1: (1 S)(1 E)
			*/
			if( touchOne == null || touchTwo != null ||
					( touchOne[1].phase == TouchPhase.Stationary && touchOne[0].phase == TouchPhase.Ended ) ||
					( touchOne[1].phase == TouchPhase.Moved && touchOne[0].phase == TouchPhase.Ended ) )
			{
				stateT = TouchControllersState.TCS_End;
				return stateT;
			}
		}
		else if( stateT == TouchControllersState.TCS_End )
		{
			stateT = TouchControllersState.TCS_None;
			return stateT;
		}
		return stateT;
	}

	TouchControllersState stateT = TouchControllersState.TCS_None;


	public override void HandlerActionTouch( TouchControllersState state, TouchGroup touches, float touchOneStateMoveTime = 0.0f, float touchOneStateTime = 0.0f, CheckingRayIntoObject checkingRay = null )
	{
		Touch[] touchOne = touches.touchOne;
		Touch[] touchTwo = touches.touchTwo;
		Touch[] touchTree = touches.touchTree;
		Vector3 point1 = Vector3.zero;
		Vector3 point1Delta = Vector3.zero;
		if( touchOne != null )
		{
			point1 = touchOne[0].position;
			point1Delta = touchOne[0].deltaPosition;
		}
		SwipeEvent( state, point1, point1Delta, touchOneStateTime );

	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	private void SwipeEvent( TouchControllersState _stateT, Vector3 point1, Vector3 point1Delta, float EventTime )
	{
		/*
		    Событие протягивания пальца, имеет 3 состояния Старт,работа,конец
		    Происхотит при резком здвижении пальца куда либо, без учета попадания в Drag обьекты, без учета пальцев попавших в элементы UI
		    Дополнительные проверки не нужны
		    point1 - палец 1
		    point1Delta - дульта перемешения пальца 1
		    EventTime - Время нахождения пальца в одном состоянии
		*/
#if DEBUG_LOG
		Log.Write( "SwipeEvent: " + _stateT.ToString() + " p1:" + point1.ToString() + " dp1:" + point1Delta.ToString() );
#endif
		SwipeModel swipe = new SwipeModel
		{
			point1 = point1,
			point1Delta = point1Delta,
			EventTime = EventTime,
			stateT = _stateT
		};
		MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Swipe, swipe );
	}

	public override void BreakTouch( TouchGroup touches, float touchOneStateMoveTime = 0.0f, float touchOneStateTime = 0.0f, CheckingRayIntoObject checkingRay = null )
	{
		if( stateT == TouchControllersState.TCS_End )
		{
			return;
		}
		Touch[] touchOne = touches.touchOne;
		Touch[] touchTwo = touches.touchTwo;
		Touch[] touchTree = touches.touchTree;
		Vector3 point1 = Vector3.zero;
		Vector3 point1Delta = Vector3.zero;
		if( touchOne != null )
		{
			point1 = touchOne[0].position;
			point1Delta = touchOne[0].deltaPosition;
		}
		SwipeModel swipe = new SwipeModel
		{
			point1 = point1,
			point1Delta = point1Delta,
			EventTime = touchOneStateTime,
			stateT = stateT
		};

		if( stateT == TouchControllersState.TCS_Start )
		{
			MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Swipe, swipe );
			swipe.stateT = TouchControllersState.TCS_Action;
			MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Swipe, swipe );
			swipe.stateT = TouchControllersState.TCS_End;
			MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Swipe, swipe );
		}
		else if( stateT == TouchControllersState.TCS_Action )
		{
			MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Swipe, swipe );
			swipe.stateT = TouchControllersState.TCS_End;
			MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Swipe, swipe );
		}

	}
}