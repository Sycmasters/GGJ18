using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour 
{
	public Transform character;
	public bool follow = true;
	public float lerpAmount = 3;
	public Vector3 offset;

	public Camera thisCam;
	public Renderer charRender;

	private void Awake ()
	{
		thisCam = GetComponent<Camera> ();
        if (charRender == null)
        {
            charRender = character.GetComponent<Renderer>();
        }
	}

	private void Start ()
	{
		transform.position = new Vector3 (character.position.x, transform.position.y, character.position.z);
	}

	private void FixedUpdate () 
	{
		if (follow) {
			transform.position = Vector3.Lerp (transform.position, 
				new Vector3 (character.position.x, transform.position.y, character.position.z) + offset, 
				lerpAmount * Time.deltaTime);
		}
	}
}
