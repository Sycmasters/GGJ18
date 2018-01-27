using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour 
{
	public Transform character;
	public bool follow = true;
	public float lerpAmount = 3;
	public Vector3 offset;
	
	// Update is called once per frame
	private void LateUpdate () 
	{
		if (follow) {
			transform.position = Vector3.Lerp (transform.position, 
				new Vector3 (character.position.x, transform.position.y, character.position.z) + offset, 
				lerpAmount * Time.deltaTime);
		}
	}
}
