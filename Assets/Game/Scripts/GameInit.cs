using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour 
{
	public Camera[] playerCameras;

	private Rect[] camerasViewports;
	private int usedCameras;

	// Use this for initialization
	private void Start () 
	{
		// Check how many are enabled
		for(int i = 0; i < playerCameras.Length; i++)
		{
			if(playerCameras[i].gameObject.activeInHierarchy)
			{
				usedCameras++;
			}
		}

		SetCameraViewports (usedCameras);

	}

	private void SetCameraViewports (int camLength)
	{
		camerasViewports = new Rect[4];

		if(camLength == 1)
		{
			camerasViewports [0] = new Rect (0, 0, 1, 1);
			playerCameras [0].rect = camerasViewports [0];
		}
		else if(camLength == 2)
		{
			camerasViewports [0] = new Rect (0, 0, 0.5f, 1);
			camerasViewports [1] = new Rect (0.5f, 0, 0.5f, 1);
			playerCameras [0].rect = camerasViewports [0];
			playerCameras [1].rect = camerasViewports [1];
		}
		else if(camLength == 3)
		{
			camerasViewports [0] = new Rect (0.25f, 0.5f, 0.5f, 0.5f);
			camerasViewports [1] = new Rect (0, 0, 0.5f, 0.5f);
			camerasViewports [2] = new Rect (0.5f, 0, 0.5f, 0.5f);
			playerCameras [0].rect = camerasViewports [0];
			playerCameras [1].rect = camerasViewports [1];
			playerCameras [2].rect = camerasViewports [2];
		}
		else if(camLength == 4)
		{
			camerasViewports [0] = new Rect (0, 0.5f, 0.5f, 0.5f);
			camerasViewports [1] = new Rect (0.5f, 0.5f, 0.5f, 0.5f);
			camerasViewports [2] = new Rect (0, 0, 0.5f, 0.5f);
			camerasViewports [3] = new Rect (0.5f, 0, 0.5f, 0.5f);
			playerCameras [0].rect = camerasViewports [0];
			playerCameras [1].rect = camerasViewports [1];
			playerCameras [2].rect = camerasViewports [2];
			playerCameras [3].rect = camerasViewports [3];
		}
	}
}
