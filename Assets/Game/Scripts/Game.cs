using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour 
{
	private ExtendCamerasViewports _extendCameras;
	public ExtendCamerasViewports extendCameras
	{
		get 
		{
			if(_extendCameras == null) { _extendCameras = FindObjectOfType<ExtendCamerasViewports>(); }
			return _extendCameras;
		}
	}
}
