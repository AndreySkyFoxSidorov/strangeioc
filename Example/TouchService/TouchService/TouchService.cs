#define DEBUG_LOG


using strange.extensions.context.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Player Input
public class TouchService
{
	TouchGroup TouchesGroup = new TouchGroup();

	private List<BaseTouchControllers> TouchControllers = new List<BaseTouchControllers>();

	public TouchService()
	{
		TouchControllers.Add( new ZoomTouchControllers()
		{
			Priority = 0, isAbortOtherTouches = true
		} );
/*		TouchControllers.Add( new DragTouchControllers()
		{
			Priority = 1, isAbortOtherTouches = true
		} );*/
		//  TouchControllers.Add(new SwipeTouchControllers() { Priority = 2, isAbortOtherTouches = false });
		// TouchControllers.Add(new TapTouchControllers() { Priority = 3, isAbortOtherTouches = false });
		//  TouchControllers.Add(new HoldTouchControllers() { Priority = 4, isAbortOtherTouches = false });

		TouchControllers.Sort( Compare );


	}


	public virtual int Compare( BaseTouchControllers x, BaseTouchControllers y )
	{
		int compareDate = x.Priority.CompareTo( y.Priority );
		return compareDate;
	}

	public static IEventDispatcher dispatcher = null;
	public void Update()
	{
		UpdateMainContextInput();
	}



	public void UpdateMainContextInput()
	{
		updateTouchArray();
		foreach( BaseTouchControllers controller in TouchControllers )
		{
			TouchControllersState state = controller.HandlerFindTouch( TouchesGroup, TouchesGroup.touchOneStateMoveTime, TouchesGroup.touchOneStateTime, TestDragObjectPosition );
			if( state != TouchControllersState.TCS_None )
			{
				controller.HandlerActionTouch( state, TouchesGroup, TouchesGroup.touchOneStateMoveTime, TouchesGroup.touchOneStateTime, TestDragObjectPosition );
				if( controller.AbortOtherTouches() )
				{
					foreach( BaseTouchControllers controllerbreak in TouchControllers )
					{
						controllerbreak.BreakTouch( TouchesGroup, TouchesGroup.touchOneStateMoveTime, TouchesGroup.touchOneStateTime, TestDragObjectPosition );
					}
					return;
				}
			}
		}
	}



	private void updateTouchArray()
	{
		if( Input.touchCount > 0 )
		{
			if( Input.touches[0].phase == TouchPhase.Began )
			{
				TouchesGroup.IsPointerOverGameObject0 = checkHoveringUI( Input.touches[0].position );
				TouchesGroup.touchOneStateMoveTime = 0;
			}
			if( TouchesGroup.touchOne == null )
			{
				if( !TouchesGroup.IsPointerOverGameObject0 )
				{
					TouchesGroup.touchOne = TouchesGroup.touchClear1;
					TouchesGroup.touchOne[4] = Input.touches[0];
					TouchesGroup.touchOne[3] = Input.touches[0];
					TouchesGroup.touchOne[2] = Input.touches[0];
					TouchesGroup.touchOne[1] = Input.touches[0];
					TouchesGroup.touchOne[0] = Input.touches[0];
					TouchesGroup.touchOneFingerID = Input.touches[0].fingerId;
				}
			}
			else
			{
				if( TouchesGroup.touchOneFingerID != Input.touches[0].fingerId )
				{
					TouchesGroup.touchOne = null;
					TouchesGroup.touchOneFingerID = -1;
					TouchesGroup.touchOneStateTime = 0;
					return;
				}
				if( TouchesGroup.touchOne[0].phase != Input.touches[0].phase )
				{
					TouchesGroup.touchOne[4] = TouchesGroup.touchOne[3];
					TouchesGroup.touchOne[3] = TouchesGroup.touchOne[2];
					TouchesGroup.touchOne[2] = TouchesGroup.touchOne[1];
					TouchesGroup.touchOne[1] = TouchesGroup.touchOne[0];
					TouchesGroup.touchOne[0] = Input.touches[0];
					TouchesGroup.touchOneStateTime = 0;
				}
				else
				{
					if( Input.touches[0].phase == TouchPhase.Moved )
					{
						TouchesGroup.touchOne[0] = Input.touches[0];

					}
				}

				if( Input.touches[0].phase == TouchPhase.Moved )
				{
					TouchesGroup.touchOne[0] = Input.touches[0];
					TouchesGroup.touchOneStateMoveTime += Time.deltaTime;
				}
				TouchesGroup.touchOneStateTime += Time.deltaTime;
			}
		}
		else
		{
			TouchesGroup.touchOne = null;
			TouchesGroup.touchOneFingerID = -1;
			TouchesGroup.touchOneStateTime = 0;
		}

		if( Input.touchCount > 1 )
		{
			if( Input.touches[1].phase == TouchPhase.Began )
			{
				TouchesGroup.IsPointerOverGameObject1 = checkHoveringUI( Input.touches[1].position );
			}
			if( TouchesGroup.touchTwo == null )
			{
				if( !TouchesGroup.IsPointerOverGameObject1 )
				{
					TouchesGroup.touchTwo = TouchesGroup.touchClear2;
					TouchesGroup.touchTwo[4] = Input.touches[1];
					TouchesGroup.touchTwo[3] = Input.touches[1];
					TouchesGroup.touchTwo[2] = Input.touches[1];
					TouchesGroup.touchTwo[1] = Input.touches[1];
					TouchesGroup.touchTwo[0] = Input.touches[1];
					TouchesGroup.touchTwoFingerID = Input.touches[1].fingerId;
				}
			}
			else
			{
				if( TouchesGroup.touchTwoFingerID != Input.touches[1].fingerId )
				{
					TouchesGroup.touchTwo = null;
					TouchesGroup.touchTwoFingerID = -1;
					return;
				}
				if( TouchesGroup.touchTwo[0].phase != Input.touches[1].phase )
				{
					TouchesGroup.touchTwo[4] = TouchesGroup.touchTwo[3];
					TouchesGroup.touchTwo[3] = TouchesGroup.touchTwo[2];
					TouchesGroup.touchTwo[2] = TouchesGroup.touchTwo[1];
					TouchesGroup.touchTwo[1] = TouchesGroup.touchTwo[0];
					TouchesGroup.touchTwo[0] = Input.touches[1];
				}
				else
				{
					if( Input.touches[1].phase == TouchPhase.Moved )
					{
						TouchesGroup.touchTwo[0] = Input.touches[1];
					}
				}
			}
		}
		else
		{
			TouchesGroup.touchTwo = null;
			TouchesGroup.touchTwoFingerID = -1;
		}

		if( Input.touchCount > 2 )
		{
			if( Input.touches[2].phase == TouchPhase.Began )
			{
				TouchesGroup.IsPointerOverGameObject2 = checkHoveringUI( Input.touches[2].position );
			}
			if( TouchesGroup.touchTree == null )
			{
				if( !TouchesGroup.IsPointerOverGameObject2 )
				{
					TouchesGroup.touchTree = TouchesGroup.touchClear3;
					TouchesGroup.touchTree[4] = Input.touches[2];
					TouchesGroup.touchTree[3] = Input.touches[2];
					TouchesGroup.touchTree[2] = Input.touches[2];
					TouchesGroup.touchTree[1] = Input.touches[2];
					TouchesGroup.touchTree[0] = Input.touches[2];
					TouchesGroup.touchTreeFingerID = Input.touches[2].fingerId;
				}
			}
			else
			{
				if( TouchesGroup.touchTreeFingerID != Input.touches[2].fingerId )
				{
					TouchesGroup.touchTree = null;
					TouchesGroup.touchTreeFingerID = -1;
					return;
				}
				if( TouchesGroup.touchTree[0].phase != Input.touches[2].phase )
				{
					TouchesGroup.touchTree[4] = TouchesGroup.touchTree[3];
					TouchesGroup.touchTree[3] = TouchesGroup.touchTree[2];
					TouchesGroup.touchTree[2] = TouchesGroup.touchTree[1];
					TouchesGroup.touchTree[1] = TouchesGroup.touchTree[0];
					TouchesGroup.touchTree[0] = Input.touches[2];
				}
				else
				{
					if( Input.touches[2].phase == TouchPhase.Moved )
					{
						TouchesGroup.touchTree[0] = Input.touches[2];
					}
				}
			}
		}
		else
		{
			TouchesGroup.touchTree = null;
			TouchesGroup.touchTreeFingerID = -1;
		}
	}

	public bool checkHoveringUI( Vector2 position )
	{
		foreach( RaycastResult r in GetUIRaycastResults( Input.mousePosition ) )
		{
			if( r.gameObject != null )
			{

				if( r.gameObject.CompareTag( "blockForTouch" ) )
				{
					return true;
				}
			}
		}
		return false;
	}

	private static readonly List<RaycastResult> tempRaycastResults = new List<RaycastResult>();
	public List<RaycastResult> GetUIRaycastResults( Vector2 position )
	{
		tempRaycastResults.Clear();
		if( EventSystem.current != null )
		{
			PointerEventData eventDataCurrentPosition = new PointerEventData( EventSystem.current )
			{
				position = new Vector2( position.x, position.y )
			};
			EventSystem.current.RaycastAll( eventDataCurrentPosition, tempRaycastResults );
		}
		return tempRaycastResults;
	}

	///////////////////////////////////////////////////////////////////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////////////////////////////////////

	private bool TestDragObjectPosition( Vector3 ScreenPoint )
	{
		Camera cam = getCamera();
		if( cam != null )
		{
			Ray ray = cam.ScreenPointToRay( ScreenPoint );
			if( Physics.Raycast( ray, out RaycastHit hit, 10 ) )
			{
				if( hit.collider.gameObject != null )
					if( hit.collider.gameObject.CompareTag( "ArObject" ) )
					{
						return true;
					}
			}
		}
		return false;
	}


	Camera camera = null;
	public Camera getCamera()
	{
		if( camera == null )
		{

			GameObject CameraObject = null;
			if( Camera.main != null && Camera.main.gameObject != null )
			{
				CameraObject = Camera.main.gameObject;
			}

			if( CameraObject == null )
			{
				foreach( Camera camera in GameObject.FindObjectsOfType( typeof( Camera ) ) as Camera[] )
				{
					if( camera != null && camera.gameObject != null )
					{
						CameraObject = camera.gameObject;
						break;
					}
				}
			}

			if( CameraObject == null )
			{
				Log.Error( "CameraObject = null" );
			}
			camera = CameraObject.GetComponent<Camera>();

			if( camera == null )
			{
				Log.Error( "CameraObject = null" );
			}
			return camera;

		}
		else
		{
			return camera;
		}
	}

}
