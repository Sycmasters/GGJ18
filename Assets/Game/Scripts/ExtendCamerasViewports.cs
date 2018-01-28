using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendCamerasViewports : MonoBehaviour 
{
	public CameraBehaviour[] playerCameras;

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
		if(usedCameras == 2)
		{
			StartCoroutine (CheckCameraOneFusion ());
		}
		else if(usedCameras == 3)
		{
			StartCoroutine (CheckCameraOneFusion ());
			StartCoroutine (CheckCameraTwoFusion ());
		}
		else if(usedCameras == 4)
		{
			StartCoroutine (CheckCameraOneFusion ());
			StartCoroutine (CheckCameraTwoFusion ());
			StartCoroutine (CheckCameraThreeFusion ());
		}

	}

	private void SetCameraViewports (int camLength)
	{
		camerasViewports = new Rect[4];

		if(camLength == 1)
		{
			camerasViewports [0] = new Rect (0, 0, 1, 1);
			playerCameras [0].thisCam.rect = camerasViewports [0];
		}
		else if(camLength == 2)
		{
			camerasViewports [0] = new Rect (0, 0, 0.5f, 1);
			camerasViewports [1] = new Rect (0.5f, 0, 0.5f, 1);
			playerCameras [0].thisCam.rect = camerasViewports [0];
			playerCameras [1].thisCam.rect = camerasViewports [1];
		}
		else if(camLength == 3)
		{
			camerasViewports [0] = new Rect (0f, 0.5f, 0.5f, 0.5f);
			camerasViewports [1] = new Rect (0.5f, 0.5f, 0.5f, 0.5f);
			camerasViewports [2] = new Rect (0f, 0f, 0.5f, 0.5f);
			playerCameras [0].thisCam.rect = camerasViewports [0];
			playerCameras [1].thisCam.rect = camerasViewports [1];
			playerCameras [2].thisCam.rect = camerasViewports [2];
		}
		else if(camLength == 4)
		{
			camerasViewports [0] = new Rect (0, 0.5f, 0.5f, 0.5f);
			camerasViewports [1] = new Rect (0.5f, 0.5f, 0.5f, 0.5f);
			camerasViewports [2] = new Rect (0, 0, 0.5f, 0.5f);
			camerasViewports [3] = new Rect (0.5f, 0, 0.5f, 0.5f);
			playerCameras [0].thisCam.rect = camerasViewports [0];
			playerCameras [1].thisCam.rect = camerasViewports [1];
			playerCameras [2].thisCam.rect = camerasViewports [2];
			playerCameras [3].thisCam.rect = camerasViewports [3];
		}
	}

    private void Update ()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }

    public int ActiveCameraCount ()
	{
		return usedCameras;
	}

	private IEnumerator CheckCameraOneFusion ()
	{
		SettingsForChecker (0, 1, new Rect (0, 0, 1, 1), new Rect (0, 0.5f, 1f, 0.5f), new Rect (0f, 0.5f, 1, 0.5f));
		SettingsForChecker (0, 2, new Rect (), new Rect (0f, 0f, 0.5f, 1), new Rect (0, 0f, 0.5f, 1));
		// SettingsForChecker (0, 3, new Rect (), new Rect (), new Rect (0, 0.5f, 0.5f, 1));

		yield return new WaitForSeconds (0.5f);

		StartCoroutine(CheckCameraOneFusion ());
	}

	private IEnumerator CheckCameraTwoFusion ()
	{
		SettingsForChecker (1, 0, new Rect (0, 0, 1, 1), new Rect (0, 0.5f, 1, 0.5f), new Rect (0f, 0.5f, 1, 0.5f));
		//SettingsForChecker (1, 2, new Rect (), new Rect (0f, 0f, 1f, 0.5f), new Rect ());
		SettingsForChecker (1, 3, new Rect (), new Rect (), new Rect (0.5f, 0f, 0.5f, 1));

		yield return new WaitForSeconds (0.5f);

		StartCoroutine(CheckCameraTwoFusion ());
	}

	private IEnumerator CheckCameraThreeFusion ()
	{
		SettingsForChecker (2, 0, new Rect (), new Rect (0, 0, 0.5f, 1), new Rect (0f, 0f, 0.5f, 1f));
		//SettingsForChecker (2, 1, new Rect (), new Rect (0f, 0f, 1f, 0.5f), new Rect ());
		SettingsForChecker (2, 3, new Rect (), new Rect (), new Rect (0, 0f, 1f, 0.5f));

		yield return new WaitForSeconds (0.5f);

		StartCoroutine(CheckCameraThreeFusion ());
	}


	private void SettingsForChecker (int currCam, int playerToCheck, Rect rectForTwo, Rect rectForThree, Rect rectForFour)
	{
		if(playerToCheck >= usedCameras || currCam >= usedCameras)
		{
			return;
		}

		if(IsVisibleFrom(playerCameras[playerToCheck].charRender, playerCameras[currCam].thisCam))
		{
			Debug.Log ("Player " + playerToCheck + " detected by " + currCam + " using " + usedCameras + " cameras");
			switch(usedCameras)
			{
			case 2:
				playerCameras [currCam].thisCam.rect = rectForTwo;
				break;

			case 3:
				playerCameras [currCam].thisCam.rect = rectForThree;
				break;

			case 4:
				playerCameras [currCam].thisCam.rect = rectForFour;
				break;
			}
			playerCameras [currCam].thisCam.depth = 0;
		}
		else
		{
			playerCameras [currCam].thisCam.rect = camerasViewports[currCam];
			playerCameras [currCam].thisCam.depth = -1;
		}
	}

	private bool IsVisibleFrom(Renderer renderer, Camera camera)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
	}
}
