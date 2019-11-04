//#define DEBUG_LOG
using System.Collections.Generic;
using UnityEngine;

public class ZoomTouchControllers : BaseTouchControllers
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

    bool MouseButtonState = false;
    Vector3 Start_editorMousposition = Vector3.zero;
    float mouseScrollWheel = 0.0f;
    float mouseScrollWheelOld = 0.0f;


    public override TouchControllersState HandlerFindTouch( TouchGroup touches, float touchOneStateMoveTime = 0.0f, float touchOneStateTime = 0.0f, CheckingRayIntoObject checkingRay = null )
	{
        Touch[] touchOne = touches.touchOne;
		Touch[] touchTwo = touches.touchTwo;
		Touch[] touchTree = touches.touchTree;
#if UNITY_EDITOR
        mouseScrollWheelOld = mouseScrollWheel;
        mouseScrollWheel += Input.GetAxis("Mouse ScrollWheel");
#endif

        if ( stateT == TouchControllersState.TCS_None )
		{
#if UNITY_EDITOR
            if (Input.GetMouseButton(1) && !MouseButtonState )
            {
                MouseButtonState = true;
                mouseScrollWheel = 0.0f;
                Start_editorMousposition = Input.mousePosition;
                stateT = TouchControllersState.TCS_Start;
                return stateT;

            }
#endif


            if ( touchTwo != null && touchOne != null )
			{
				/*
				Начало зума:
				Случай 1: (1 M)+(2 M)
				Случай 2: (1 M)+(2 S)
				Случай 3: (1 S)+(2 S)
				Случай 4: (1 S)+(2 M)
				*/
				if( ( touchOne[0].phase == TouchPhase.Moved && touchTwo[0].phase == TouchPhase.Moved ) ||
						( touchOne[0].phase == TouchPhase.Moved && touchTwo[0].phase == TouchPhase.Stationary ) ||
						( touchOne[0].phase == TouchPhase.Stationary && touchTwo[0].phase == TouchPhase.Stationary ) ||
						( touchOne[0].phase == TouchPhase.Stationary && touchTwo[0].phase == TouchPhase.Moved ) )
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

#if UNITY_EDITOR
            if (!Input.GetMouseButton(1) && MouseButtonState)
            {
                MouseButtonState = false;
                stateT = TouchControllersState.TCS_End;
                return stateT;
            }
#endif
            /*
			   Зум
			    Случай 1: (1 M)+(2 M)
			    Случай 2: (1 M)+(2 S)
			    Случай 3: (1 S)+(2 M)
			*/
            /*
			    Конец зума:
			    Случай 1: (1 M)+(2 E)
			    Случай 2: (1 S)+(2 E)
			    Случай 3: (1 E)+(2 S)
			    Случай 4: (1 E)+(2 M) */
            if ( touchTwo == null || touchOne == null ||
					( ( touchOne[0].phase == TouchPhase.Moved && touchTwo[0].phase == TouchPhase.Ended ) ||
					  ( touchOne[0].phase == TouchPhase.Stationary && touchTwo[0].phase == TouchPhase.Ended ) ||
					  ( touchOne[0].phase == TouchPhase.Ended && touchTwo[0].phase == TouchPhase.Stationary ) ||
					  ( touchOne[0].phase == TouchPhase.Ended && touchTwo[0].phase == TouchPhase.Moved ) ||
					  ( touchOne[0].phase == TouchPhase.Ended && touchTwo[0].phase == TouchPhase.Ended ) ) )
			{
				stateT = TouchControllersState.TCS_End;
				return stateT;
			}
		}
		else if( stateT == TouchControllersState.TCS_End )
		{
#if UNITY_EDITOR
                MouseButtonState = false;
#endif
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
		Vector3 point2 = Vector3.zero;
		Vector3 point1Delta = Vector3.zero;
		Vector3 point2Delta = Vector3.zero;
		if( touchOne != null )
		{
			point1 = touchOne[0].position;
			point1Delta = touchOne[0].deltaPosition;
		}
		if( touchTwo != null )
		{
			point2 = touchTwo[0].position;
			point2Delta = touchTwo[0].deltaPosition;
		}
#if UNITY_EDITOR
        if (MouseButtonState)
        {
            point1 = Start_editorMousposition;
            point2 = Input.mousePosition + new Vector3(100.0f * -mouseScrollWheel, 100.0f * -mouseScrollWheel, 100.0f * -mouseScrollWheel);
            point1Delta = new Vector3(100.0f * (mouseScrollWheelOld - mouseScrollWheel), 100.0f * (mouseScrollWheelOld - mouseScrollWheel), 100.0f * (mouseScrollWheelOld - mouseScrollWheel));
            point2Delta = new Vector3(100.0f * (mouseScrollWheelOld - mouseScrollWheel), 100.0f * (mouseScrollWheelOld - mouseScrollWheel), 100.0f * (mouseScrollWheelOld - mouseScrollWheel));
        }
#endif

        ZoomEvent( stateT, point1, point2, point1Delta, point2Delta );
	}

	private void ZoomEvent( TouchControllersState _stateT, Vector3 point1, Vector3 point2, Vector3 point1Delta, Vector3 point2Delta )
	{
		/*
		    Событие зума, имеет 3 состояния Старт,работа,конец
		    Происхотит при работе двумя пальцами, без учета пальцев попавших в элементы UI
		    Дополнительные проверки не нужны
		    point1 - палец 1
		    point2 - палец 2
		    point1Delta - дульта перемешения пальца 1
		    point2Delta - дульта перемешения пальца 2
		*/
#if DEBUG_LOG
		Log.Write( "ZoomEvent: " + _stateT.ToString() + " p1:" + point1.ToString() + " dp1:" + point1Delta.ToString() + " p2:" + point2.ToString() + " dp2:" + point2Delta.ToString() );
#endif
		ZoomModel zoom = new ZoomModel
		{
			point1 = point1,
			point1Delta = point1Delta,
			point2 = point2,
			point2Delta = point2Delta,
			stateT = _stateT
		};
		MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Zoom, zoom );
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
		Vector3 point2 = Vector3.zero;
		Vector3 point1Delta = Vector3.zero;
		Vector3 point2Delta = Vector3.zero;
		if( touchOne != null )
		{
			point1 = touchOne[0].position;
			point1Delta = touchOne[0].deltaPosition;
		}
		if( touchTwo != null )
		{
			point2 = touchTwo[0].position;
			point2Delta = touchTwo[0].deltaPosition;
		}
		ZoomModel zoom = new ZoomModel
		{
			point1 = point1,
			point1Delta = point1Delta,
			point2 = point2,
			point2Delta = point2Delta,
			stateT = stateT
		};
		if( stateT == TouchControllersState.TCS_Start )
		{
			MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Zoom, zoom );
			zoom.stateT = TouchControllersState.TCS_Action;
			MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Zoom, zoom );
			zoom.stateT = TouchControllersState.TCS_End;
			MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Zoom, zoom );
		}
		else if( stateT == TouchControllersState.TCS_Action )
		{
			MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Zoom, zoom );
			zoom.stateT = TouchControllersState.TCS_End;
			MainContextView.DispatchStrangeEvent( EventGlobal.E_TouchAndMouseGestures_Zoom, zoom );
		}



	}
}