using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour 
{
	public Camera playerOneCam, playerTwoCam, playerThreeCam, playerFourCam;

	// Use this for initialization
	private void Start () 
	{
		//Debug.Log();
		playerOneCam.rect = new Rect(0, .5f, .5f, .5f);
		playerTwoCam.rect = new Rect(.5f, .5f, .5f, .5f);
		playerThreeCam.rect = new Rect(0, 0, .5f, .5f);
		playerFourCam.rect = new Rect(.5f, 0, .5f, .5f);
	}
	
	// Update is called once per frame
	private void Update () 
	{
		if(Input.GetKey(KeyCode.H))
		{
			playerOneCam.rect = new Rect (0, 0f, .5f, 1);
			playerThreeCam.gameObject.SetActive(false);
		}
		else
		{
			//Debug.Log();
			playerOneCam.rect = new Rect(0, .5f, .5f, .5f);
			playerThreeCam.rect = new Rect(0f, 0, .5f, .5f);
			playerThreeCam.gameObject.SetActive (true);
		}
		if (Input.GetKey (KeyCode.K)) 
		{
			playerOneCam.rect = new Rect (0, 0, 1, 1);
			playerTwoCam.gameObject.SetActive (false);
			playerThreeCam.gameObject.SetActive (false);
			playerFourCam.depth = 0;
		}
		else
		{
			playerOneCam.rect = new Rect(0, .5f, .5f, .5f);
			playerTwoCam.gameObject.SetActive (true);
			playerThreeCam.gameObject.SetActive (true);
			playerFourCam.depth = -1;
		}
	}
}
