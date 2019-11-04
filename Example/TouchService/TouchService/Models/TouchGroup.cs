using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchGroup
{
	public Touch[] touchClear1 = new Touch[5];
	public Touch[] touchClear2 = new Touch[5];
	public Touch[] touchClear3 = new Touch[5];
	public Touch[] touchOne = null;
	public Touch[] touchTwo = null;
	public Touch[] touchTree = null;
	public int touchOneFingerID = -1;
	public int touchTwoFingerID = -1;
	public int touchTreeFingerID = -1;
	public bool IsPointerOverGameObject0 = false;
	public bool IsPointerOverGameObject1 = false;
	public bool IsPointerOverGameObject2 = false;
	public float touchOneStateTime = 0;
	public float touchOneStateMoveTime = 0;


	public TouchGroup()
	{
		int clearSize = 4;
		for( int i = 0; i <= clearSize; i++ )
		{
			touchClear1[i] = new Touch();
		}
		for( int i = 0; i <= clearSize; i++ )
		{
			touchClear2[i] = new Touch();
		}
		for( int i = 0; i <= clearSize; i++ )
		{
			touchClear2[i] = new Touch();
		}
	}
}
