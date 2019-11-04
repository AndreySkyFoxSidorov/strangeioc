//#define DEBUG_LOG
using System.Collections.Generic;
using UnityEngine;

public class DragTouchControllers : BaseTouchControllers
{
	public virtual int GetPriority()
	{
		return Priority;
	}
	public virtual bool AbortOtherTouches()
	{
		return isAbortOtherTouches;
	}



	TouchControllersState stateT = TouchControllersState.TCS_None;

	public override TouchControllersState HandlerFindTouch( TouchGroup touches, float touchOneStateMoveTime = 0.0f, float touchOneStateTime = 0.0f, CheckingRayIntoObject checkingRay = null )
	{
		Touch[] touchOne = touches.touchOne;
		Touch[] touchTwo = touches.touchTwo;
		Touch[] touchTree = touches.touchTree;
		if( stateT == TouchControllersState.TCS_None )
		{
			if( touchOne != null && touchTwo == null )
			{
				/*
				Начало Драга:
				Случай 1: (1 B)(1 S) + S(>0.3C)
				*/
				if( (touchOne[1].phase == TouchPhase.Began || touchOne[0].phase == TouchPhase.Stationary) && checkingRay( touchOne[0].position ))  
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
			    Драг
			    Случай 1: (1 M)
			    Конец Драга:
			    Случай 1: (1 E)
			*/
			if( ( touchOne == null || touchTwo != null ||
					touchOne[0].phase == TouchPhase.Ended ) )
			{
				touchOneStateTime = 0;
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
		DragEvent( stateT, point1, point1Delta, touchOneStateTime );
	}
	private void DragEvent( TouchControllersState state, Vector3 point1, Vector3 point1Delta, float EventTime )
	{
		/*
		    Событие перемешения обьекта, имеет 3 состояния Старт,работа,конец
		    Происхотит при попадании в Drag обьекты, без учета пальцев попавших в элементы UI.
		    При В старте нужно реализовать выдиление здания
		    При В работа перемешение, в конце проверку на отпускания браг обьекта.
		    Дополнительные проверки не нужны
		    point1 - палец 1
		    point1Delta - дульта перемешения пальца 1
		    EventTime - Время нахождения пальца в одном состоянии
		*/
#if DEBUG_LOG
		Log.Write( "DragEvent: " + state.ToString() + " p1:" + point1.ToString() + " dp1:" + point1Delta.ToString() );
#endif
		DragModel dragModel = new DragModel()
		{
			stateT = state,
			EventTime = EventTime,
			point1 = point1,
			point1Delta = point1Delta
		};

		MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Drag, dragModel );

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
		DragModel dragModel = new DragModel()
		{
			stateT = stateT,
			EventTime = touchOneStateTime,
			point1 = point1,
			point1Delta = point1Delta
		};

		if( stateT == TouchControllersState.TCS_Start )
		{
			MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Drag, dragModel );
			dragModel.stateT = TouchControllersState.TCS_Action;
			MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Drag, dragModel );
			dragModel.stateT = TouchControllersState.TCS_End;
			MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Drag, dragModel );
		}
		else if( stateT == TouchControllersState.TCS_Action )
		{
			MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Drag, dragModel );
			dragModel.stateT = TouchControllersState.TCS_End;
			MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Drag, dragModel );
		}
	}

}